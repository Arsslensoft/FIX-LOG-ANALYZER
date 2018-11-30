using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.DataDictionary;
using QuickFix.Fields;
using TachyonFix.Core.Insights;
using TachyonFix.Core.Scenarios;
using TachyonFix.Core.Stats;

namespace TachyonFix.Core
{
    public class Solution
    {
        public Linker Linker { get; set; }
        public DateTimeOffset? Start => Entries?.FirstOrDefault()?.DateTime;
        public DateTimeOffset? End => Entries?.LastOrDefault()?.DateTime;
        public TimeSpan? RecordedTime => End - Start;
        public int MessagesCount => Entries.Count(e => e.Error == null);
        public int UnknownMessagesCount => OtherEntries.Count;
        public string FileName { get; set; }

      
        public List<Error> LinkingErrors { get; set; } = new List<Error>();
        public IEnumerable<Error> Errors => Entries.Where(e => e.Error != null).Select(x => x.Error).Union(LinkingErrors);
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public List<OtherEntry> OtherEntries { get; set; } = new List<OtherEntry>();
        public QuickFix.DataDictionary.DataDictionary DataDictionary { get; set; }
        public Message TryParseMessage(string line, IMessageFactory messageFactory, QuickFix.DataDictionary.DataDictionary dd)
        {
        
            Message m = null;
            int pos = 0;
           
            try
            {
                StringField protocol = Message.ExtractField(line, ref pos);
                StringField t = Message.ExtractField(line, ref pos);
                StringField type = Message.ExtractField(line, ref pos);
                m = messageFactory.Create(protocol.getValue(), type.getValue());
                while (pos < line.Length)
                    m.SetField(Message.ExtractField(line, ref pos));

               m.FromString(line, false, dd, dd, messageFactory);

            }
            catch
            {

            }
            return m;
        }
        public static Solution Open(string file, BackgroundWorker worker, DataDictionary dd)
        {
            var sol = new Solution(){ FileName = file};
        
            var s = File.ReadAllText(file);
            var r = new Regex("\0\0", RegexOptions.IgnoreCase);
            var entries = r.Split(s);
            int i = 0;
            double progress = 0;
            double count = entries.Length;
            IMessageFactory _defaultMsgFactory = new QuickFix.FIX42.MessageFactory();
         
            sol.DataDictionary = dd;
            Entry e = null;
            foreach (var sFM in entries)
            {
                progress = (i / count)*100; 
                worker.ReportProgress((int)progress);
           
                DateTimeOffset dt = DateTimeOffset.MaxValue;
                


                if (!sFM.Contains("| Fix") && sFM.Contains("| DATA"))
                {
                     dt = DateTimeOffset.Parse(sFM.Substring(sFM.IndexOf(Message.SOH) + 1, sFM.IndexOf('|') - 2));
                    sol.OtherEntries.Add(new OtherEntry(){Content = sFM, Index = i, RawLogLine = sFM, Kind =  EntryType.Data, DateTime = dt,Direction = sFM.Contains("| IN") ? Direction.IN : Direction.OUT});
                    i++;
                    continue;

                }
                else if (!sFM.Contains("| Fix"))
                {
                    i++;
                    continue;
                }

                 dt = DateTimeOffset.Parse(sFM.Substring(sFM.IndexOf("2016/"), sFM.IndexOf('|') - 2));
                var FM = sFM.Remove(0, sFM.IndexOf('[') + 1);
                FM = FM.Substring(0, FM.LastIndexOf(']'));
                try
                {



                    int pos = 0;
                    StringField protocol = Message.ExtractField(FM, ref pos);
                    StringField t = Message.ExtractField(FM, ref pos);
                    StringField type = Message.ExtractField(FM, ref pos);
                    var m = _defaultMsgFactory.Create(protocol.getValue(), type.getValue());

                    m.FromString(FM, true, dd, dd, _defaultMsgFactory);
                    e = new Entry()
                    {
                        DateTime = dt,
                        Kind = EntryType.Fix,
                        Index = i,
                        Message = m,
                        Content = FM,
                        RawLogLine = sFM,
                        Direction = sFM.Contains("| IN") ? Direction.IN : Direction.OUT
                    };

                }
                catch (Exception ex)
                {
                    e = new Entry()
                    {
                        DateTime = dt,
                        Index = i,
                        Content = FM,
                        Error = new Error()
                        {
                            Exception = ex,
                            Index = i,
                            Content = FM,
                            ErrorType = ex is InvalidMessage ? ErrorType.Invalid : ErrorType.Syntax,
                            Message = ex.Message,
                            ParsedMessage = sol.TryParseMessage(FM, _defaultMsgFactory, dd)
                        },
                        RawLogLine = sFM,
                        Direction = sFM.Contains("| IN") ? Direction.IN : Direction.OUT
                    };
             

                }
                finally
                {
                    sol.Entries.Add(e);
                    sol.AssignEntryToBroker(e);
                }
                i++;
            }

            return sol;
        }
        internal string TryGetSenderIdFromMessage(Message m)
        {
            if (m.Header.IsSetField(new SenderCompID()))
            {
                var f = m.Header.GetField(new SenderCompID());
                return f.getValue();
            }
            else if (m.IsSetField(new SenderCompID()))
            {
                var f = m.GetField(new SenderCompID());
                return f.getValue();
            }
            return null;
        }
        internal string TryGetSenderNameFromMessage(Message m)
        {
            if (m.Header.IsSetField(new SenderSubID()))
            {
                var f =m.Header.GetField(new SenderSubID());
                return f.getValue();
            }
            else if (m.IsSetField(new SenderSubID()))
            {
                var f = m.GetField(new SenderSubID());
                return f.getValue();
            }
            return null;
        }

        internal string TryGetTargetIdFromMessage(Message m)
        {
            if (m.Header.IsSetField(new TargetCompID()))
            {
                var f = m.Header.GetField(new TargetCompID());
                return f.getValue();
            }
            else if (m.IsSetField(new TargetCompID()))
            {
                var f = m.GetField(new TargetCompID());
                return f.getValue();
            }
            return null;
        }
        internal string TryGetTargetNameFromMessage(Message m)
        {
            if (m.Header.IsSetField(new TargetSubID()))
            {
                var f = m.Header.GetField(new TargetSubID());
                return f.getValue();
            }
            else if (m.IsSetField(new TargetSubID()))
            {
                var f = m.GetField(new TargetSubID());
                return f.getValue();
            }
            return null;
        }
        public void AssignEntryToBroker(Entry entry)
        {
            string name = null, id = null;

            if (entry.Message != null)
            {
                // get name and id
                name = TryGetSenderNameFromMessage(entry.Message);
                id = TryGetSenderIdFromMessage(entry.Message);
                if(id != null && (id == "0" || id == "1"))
                    AssignEntryToTrader(entry,name);
                else if (name != null)
                    AssignEntryToBrokerByName(name, entry, id);
                else if(id != null)
                    AssignEntryToBrokerById(id, entry);
                else AssignEntryToBrokerByName("UNKNOWN", entry);
            }
            else if (entry.Error?.ParsedMessage != null)
            {
                name = TryGetSenderNameFromMessage(entry.Error.ParsedMessage);
                id = TryGetSenderIdFromMessage(entry.Error.ParsedMessage);

                if (name != null)
                    AssignEntryToBrokerByName(name, entry, id);
                else if (id != null)
                    AssignEntryToBrokerById(id, entry);
                else AssignEntryToBrokerByName("UNKNOWN", entry);
            }
        }

        //public void AssignEntryToBroker(Entry entry)
        //{
        //    if (entry.Message != null)
        //    {
        //        if (entry.Message.Header.IsSetField(new SenderSubID()))
        //        {
        //            var f = entry.Message.Header.GetField(new SenderSubID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() {Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>()});
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else if (entry.Message.IsSetField(new SenderSubID()))
        //        {
        //            var f = entry.Message.GetField(new SenderSubID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() {Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() {entry}});
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //       else if (entry.Message.Header.IsSetField(new SenderCompID()))
        //        {
        //            var f = entry.Message.Header.GetField(new SenderCompID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() { Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() });
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else if (entry.Message.IsSetField(new SenderCompID()))
        //        {
        //            var f = entry.Message.GetField(new SenderCompID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() { Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() { entry } });
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else
        //        {
        //            if (!Intermediaries.ContainsKey("UNKNOWN"))
        //                Intermediaries.Add("UNKNOWN",
        //                    new Broker() { Name = "UNKNOWN", Id = "UNKNOWN", Entries = new List<Entry>() { entry } });
        //            else Intermediaries["UNKNOWN"].Entries.Add(entry);
        //        }
        //    }
        //    else if(entry.Error?.ParsedMessage != null)
        //    {
        //        if (entry.Error.ParsedMessage.Header.IsSetField(new SenderSubID()))
        //        {
        //            var f = entry.Error.ParsedMessage.Header.GetField(new SenderSubID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() { Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() });
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else if (entry.Error.ParsedMessage.IsSetField(new SenderSubID()))
        //        {
        //            var f = entry.Error.ParsedMessage.GetField(new SenderSubID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() {Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() {entry}});
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else if (entry.Error.ParsedMessage.Header.IsSetField(new SenderCompID()))
        //        {
        //            var f = entry.Error.ParsedMessage.Header.GetField(new SenderCompID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() { Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() });
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else if (entry.Error.ParsedMessage.IsSetField(new SenderCompID()))
        //        {
        //            var f = entry.Error.ParsedMessage.GetField(new SenderCompID());
        //            if (!Intermediaries.ContainsKey(f.getValue()))
        //                Intermediaries.Add(f.getValue(),
        //                    new Broker() { Name = f.getValue(), Id = f.getValue(), Entries = new List<Entry>() { entry } });
        //            else Intermediaries[f.getValue()].Entries.Add(entry);
        //        }
        //        else
        //        {
        //            if (!Intermediaries.ContainsKey("UNKNOWN"))
        //                Intermediaries.Add("UNKNOWN",
        //                    new Broker() { Name = "UNKNOWN", Id = "UNKNOWN", Entries = new List<Entry>() { entry } });
        //            else Intermediaries["UNKNOWN"].Entries.Add(entry);
        //        }
        //    }

        //}
        private void AssignEntryToTrader(Entry entry, string name = null)
        {

            if (!Intermediaries.ContainsKey("TRADER"))
            {
                var b = new Broker() {Name = name ?? "TRADER", Entries = new List<Entry>() {entry}, Trader = true};
                b.IdList.Add("0");
                b.IdList.Add("1");
                Intermediaries.Add("TRADER", b);
            }
            else
            {
                Intermediaries["TRADER"].Entries.Add(entry);
                if (name != null)
                    Intermediaries["TRADER"].Name = name;
            }
      
        }
        private void AssignEntryToBrokerByName(string name, Entry entry, string id = null)
        {

            if (!Intermediaries.ContainsKey(name))
            {
                var b = new Broker() {Name = name, Entries = new List<Entry>() {entry}};
                if (id != null)
                    b.IdList.Add(id);

                Intermediaries.Add(name, b);
            }
            else
            {
                Intermediaries[name].Entries.Add(entry);
                if(id != null && !Intermediaries[name].IdList.Contains(id))
                    Intermediaries[name].IdList.Add(id);
            }
        }
        private void AssignEntryToBrokerById(string id, Entry entry)
        {
            // try find broker by id
            var b = Intermediaries.Where(x => x.Value.IdList.Contains(id));
            if (!b.Any())
            {
                var br = new Broker() { Name = "UNKNOWN_PARTY_ID_"+id, Entries = new List<Entry>() { entry } };
                br.IdList.Add(id);
                br.Unknown = true;
                Intermediaries.Add(br.Name, br);
            }
            else if (b.Count() == 1)
            {
                var br = b.FirstOrDefault();

                Intermediaries[br.Key].Entries.Add(entry);
            }
            else throw new ArgumentException("Duplicate Ids cross brokers "+ id);
         
        }

        public Broker TryGetBrokerByNameOrId(string name, string id)
        {
            if(name == null && id == null) throw new NullReferenceException();
     
            if (name != null)
            {
                if (Intermediaries.ContainsKey(name))
                    return Intermediaries[name];
                else if (Intermediaries.Any(x => x.Value.Name == name))
                    return Intermediaries.FirstOrDefault(x => x.Value.Name == name).Value;
            }
            if (id != null && Intermediaries.Any(x => x.Value.IdList.Contains(id)))
                    return Intermediaries.FirstOrDefault(x => x.Value.IdList.Contains(id)).Value;

            return null;
        }
        

        public Dictionary<string,Broker> Intermediaries = new Dictionary<string, Broker>();
        public void Analyze(BackgroundWorker worker)
        {
            Linker = new Linker(this);
            Linker.OnProgressChanged += (d, s) => worker.ReportProgress((int)d, s);
            Linker.DetectEvents();

            foreach (var intermediary in Intermediaries)
            {
                var bsc = new BrokerStatisticsCalculator(intermediary.Value, this);
                bsc.OnProgressChanged += (d, s) => worker.ReportProgress((int)d, s);
                bsc.CalculateStats();
                intermediary.Value.Stats = bsc;

                var ic = new InsightCalculator(this, intermediary.Value);
                ic.OnProgressChanged += (d, s) => worker.ReportProgress((int)d, s);
                ic.RecommendInsights();
                intermediary.Value.Insights = ic;
            }
        }
    }
}
