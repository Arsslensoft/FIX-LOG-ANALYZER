using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix.FIX42;

namespace TachyonFix.Core.Stats
{
   public class BrokerStatisticsCalculator
    {
        public Broker Broker { get; set; }
        public BrokerStats Stats { get; set; }
        public Solution Solution { get; set; }
        public BrokerStatisticsCalculator(Broker b, Solution s)
        {
            Solution = s;
            Broker = b;
        }


        public event Action<double, string> OnProgressChanged;

        public void CalculateStats()
        {
            if (Stats != null) return;
            int mc = Broker.AllMessages.Count();
            Stats = new BrokerStats(){ Broker = Broker};
            OnProgressChanged?.Invoke(0, "Calculating message distribution for broker "+ Broker.Name+"...");
            // MTD
            Stats.MessageTypeDistribution = (from p in Broker.AllMessages
                group p by p.GetType().Name into g
                select new KeyValuePair<string,int>(g.Key, g.Count())).ToList();

            OnProgressChanged?.Invoke(25, "Calculating Accept/Reject distribution for broker " + Broker.Name + "...");
            // AR
            Stats.AcceptRejectDistribution = new ARDistribution()
            {
                Rejected = Broker.AllMessages.Count(x => x.GetType().Name.Contains("Reject"))
            };
            Stats.AcceptRejectDistribution.Accepted =
                mc - Stats.AcceptRejectDistribution.Rejected;


            OnProgressChanged?.Invoke(50, "Calculating Admin/App messages distribution for broker " + Broker.Name + "...");
            // App/Admin
            Stats.AdminAppDistribution = new AdminAppDistribution()
            {
                Admin = Broker.AllMessages.Count(x => x.IsAdmin())
            };
            Stats.AdminAppDistribution.App =
                mc - Stats.AdminAppDistribution.Admin;

            // SP
            Stats.MessagesPercentage = (mc / (double) Solution.MessagesCount) * 100;

            OnProgressChanged?.Invoke(75, "Calculating error type distribution for broker " + Broker.Name + "...");

            // ED
            Stats.ErrorDistribution = new ErrorDistribution();
            Stats.ErrorDistribution.Syntax = Solution.Linker.Events.Where(x => x.Target == Broker).Select(x => x.Message).Count(x => x is Reject) + Broker.Errors.Count();
            Stats.ErrorDistribution.Semantic =
                Solution.Linker.Events.Where(x => x.Target == Broker).Select(x => x.Message).Count(x => !(x is Reject) && x.GetType().Name.Contains("Reject"));

            // TAT
            Stats.TotalActiveTime = Broker.Entries.LastOrDefault().DateTime - Broker.Entries.FirstOrDefault().DateTime;
            OnProgressChanged?.Invoke(100, "Ready");


        }
    }
}
