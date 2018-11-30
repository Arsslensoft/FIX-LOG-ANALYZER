using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Testing
{
    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;

        iTextSharp.text.Font RunDateFont;
        public PageEventHelper()
        {
            BaseFont bf = BaseFont.CreateFont(Environment.CurrentDirectory + @"\arial.ttf", BaseFont.IDENTITY_H, true);
            RunDateFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        }
        public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            String text = "Page " + pageN.ToString() + " of ";
            float len = this.RunDateFont.BaseFont.GetWidthPoint(text, this.RunDateFont.Size);

            iTextSharp.text.Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(this.RunDateFont.BaseFont, this.RunDateFont.Size);
            cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
            cb.ShowText(text);

            cb.EndText();

            cb.AddTemplate(template, document.LeftMargin + len, pageSize.GetBottom(document.BottomMargin));
        }

        public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(this.RunDateFont.BaseFont, this.RunDateFont.Size);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber));
            template.EndText();
        }
    }
    class Program
    {
       static private ICellStyle ExpressionStyle;
        static private IFont CourierNew;
      static private IFont BoldCourierNew;
      static void InitStyles(IWorkbook workbook)
        {

            BoldCourierNew = workbook.CreateFont();
            BoldCourierNew.FontHeightInPoints = 11;
            BoldCourierNew.FontName = "Courier New";
            BoldCourierNew.IsBold = true;

            CourierNew = workbook.CreateFont();
            CourierNew.FontHeightInPoints = 11;
            CourierNew.FontName = "Courier New";

            ExpressionStyle = workbook.CreateCellStyle();
            ExpressionStyle.WrapText = true;
            ExpressionStyle.SetFont(CourierNew);
        }
        static void PrepareHeader(ISheet sheet, IWorkbook workbook, DataTable dt)
        {
            // header row
            var hr = sheet.CreateRow(0);
            // set header row
            //ICellStyle gstyle = workbook.CreateCellStyle();
            //gstyle.Alignment = HorizontalAlignment.Center;
            //gstyle.VerticalAlignment = VerticalAlignment.Center;
            //gstyle.WrapText = true;
            //gstyle.SetFont(CourierNew);
            //for (int col = 0; col < Store.Aircrafts.Count + 2; col++)
            //    sheet.SetDefaultColumnStyle(col, gstyle);

            //ICellStyle acstyle = workbook.CreateCellStyle();
            //acstyle.Alignment = HorizontalAlignment.Left;
            //acstyle.WrapText = false;
            //acstyle.Rotation = 255;
            //acstyle.VerticalAlignment = VerticalAlignment.Bottom;
            //acstyle.Indention = 0;
            //acstyle.SetFont(BoldCourierNew);

            //acstyle.BorderBottom = BorderStyle.Thick;
            //acstyle.BorderLeft = BorderStyle.Thick;
            //acstyle.BorderTop = BorderStyle.Thick;
            //acstyle.BorderRight = BorderStyle.Thick;


            ICellStyle aircraftstyle = workbook.CreateCellStyle();
            aircraftstyle.Alignment = HorizontalAlignment.Center;
            aircraftstyle.VerticalAlignment = VerticalAlignment.Center;
            aircraftstyle.WrapText = true;
            aircraftstyle.SetFont(BoldCourierNew);

            //aircraftstyle.BorderBottom = BorderStyle.Thick;
            //aircraftstyle.BorderLeft = BorderStyle.Thick;
            //aircraftstyle.BorderTop = BorderStyle.Thick;
            //aircraftstyle.BorderRight = BorderStyle.Thick;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var r = hr.CreateCell(i);
                r.SetCellValue(dt.Columns[i].ColumnName);
                r.CellStyle = aircraftstyle;
            }

        }

        static void ExportRows(ISheet sheet, IWorkbook workbook, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var rr = sheet.CreateRow(i+1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var expr = rr.CreateCell(j);
                    expr.SetCellValue(dt.Rows[i].ItemArray[j].ToString());
                    expr.CellStyle = ExpressionStyle;
                }
           
            }
        }
       static string ConcatStrings(string left, string right)
        {

            if ((left.Length + right.Length) < 110)
            {
                for (int i = (left.Length + right.Length); i < 110 - right.Length; i++)
                    left += " ";
            }
            return left + right;
        }
        static void ExportReport(iTextSharp.text.Document doc, DataTable dt)
        {

            iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(2f, 100f, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -1);
            iTextSharp.text.Chunk linebreak = new iTextSharp.text.Chunk(line);
            iTextSharp.text.Font black = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            var logo = new iTextSharp.text.Paragraph() { Alignment = 0 };
            logo.Add(new iTextSharp.text.Chunk("Tachyon", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 36, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            logo.Add(new iTextSharp.text.Chunk("FIX", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 36, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(26, 188, 156))));
            doc.Add(logo);
            doc.Add(new iTextSharp.text.Chunk(line));
            doc.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(ConcatStrings("Log List", DateTime.Now.ToString()), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 9f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))) { Alignment = 0 });

            doc.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk(" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 9f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))) { Alignment = 0 });

            doc.Add(linebreak);
            doc.Add(new iTextSharp.text.Paragraph(" "));

            var bf = BaseFont.CreateFont(Environment.CurrentDirectory + @"\arial.ttf", BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font NormalFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font TFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.GREEN);
            iTextSharp.text.Font XFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);

            PdfPTable table = new PdfPTable(dt.Columns.Count) { WidthPercentage = 100 };
            for (int i = 0; i < dt.Columns.Count; i++)
             table.AddCell(new PdfPCell() { Phrase = new iTextSharp.text.Phrase(dt.Columns[i].ColumnName), BackgroundColor = iTextSharp.text.BaseColor.GRAY });


            
           

            //table.SetWidths(new float[] { 3, 25, 25, 8 });

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow GridRow = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                    table.AddCell(new iTextSharp.text.Phrase(GridRow.ItemArray[j].ToString(), NormalFont));

            }

            doc.Add(table);


        }
        static void Main(string[] args)
        {
            var f = @"C:\Users\Arsslen\Desktop\projet gl5 parseur  log CAP FIX\a.pdf";
            var dt = new DataTable("S");
            dt.Columns.Add("X");
            dt.Columns.Add("Y");
            dt.Rows.Add(0, 5);
            dt.Rows.Add(1, 9);
            using (FileStream fs = File.OpenWrite(f))
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, fs);
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;
                document.Open();
                ExportReport(document,dt);
                document.Close();
            }
        }
    }
}
