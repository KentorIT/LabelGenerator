using Kentor.LabelGenerator.Models;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator
{
    public static class DocumentHelpers
    {
        public static PdfDocument CreateDocument(string[][] labelRows, DocumentType documentType)
        {
            LabelSettings settings = Utilities.GetSettings(documentType);

            // Document settings
            PdfDocument doc = new PdfDocument();
            PdfPage page = AddPage(doc, settings);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            // Variables used to calculate label position
            int currentColumn;
            int currentRow;
            int labelsInCurrentPage = 0;
            double contentPositionLeft;
            double contentPositionTop;

            foreach (var row in labelRows)
            {
                if (labelsInCurrentPage == settings.LabelsPerPage)
                {
                    // Page is full, add new page
                    page = AddPage(doc, settings);
                    gfx = XGraphics.FromPdfPage(page);
                    tf = new XTextFormatter(gfx);
                    labelsInCurrentPage = 0;
                }

                currentColumn = CalculateCurrentColumn(labelsInCurrentPage, settings.ColumnsPerPage);
                currentRow = CalculateCurrentRow(labelsInCurrentPage, settings.ColumnsPerPage, currentColumn);
                contentPositionLeft = CalculateContentPositionLeft(currentColumn, settings);
                contentPositionTop = CalculateContentPositionTop(currentRow, settings);

                XSize contentSize = GetContentSize(settings);
                XRect rectangle = CreateRectangle(contentPositionLeft, contentPositionTop, contentSize);
                gfx.DrawRectangle(XPens.Transparent, rectangle); // Transparent border

                XFont font = new XFont(settings.FontFamily, settings.FontSize, XFontStyle.Bold);
                var labelText = FormatLabelText(row, settings.MaxCharactersPerRow);
                tf.DrawString(labelText, font, XBrushes.Black, rectangle, XStringFormats.TopLeft);

                // Increase number of labels printed
                labelsInCurrentPage++;
            }
            return doc;
        }

        public static PdfPage AddPage(PdfDocument doc, LabelSettings settings)
        {
            PdfPage page = doc.AddPage();
            page.Width = XUnit.FromMillimeter(settings.PageWidth);
            page.Height = XUnit.FromMillimeter(settings.PageHeight);
            return page;
        }

        public static byte[] SaveToArray(PdfDocument document)
        {
            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }

        public static XSize GetContentSize(LabelSettings settings)
        {
            var contentWidth = XUnit.FromMillimeter(settings.LabelWidth);
            var contentHeight = XUnit.FromMillimeter(settings.LabelHeight);
            var contentSize = new XSize(contentWidth.Point, contentHeight.Point);
            return contentSize;
        }

        public static int CalculateCurrentColumn(int labelsInCurrentPage, int columnsPerPage)
        {
            // Algorithm to calculate which column label should be printed to
            var currentColumn = (labelsInCurrentPage + 1) % columnsPerPage;

            if (currentColumn == 0)
            {
                // Last column in row
                currentColumn = columnsPerPage;
            }
            return currentColumn;
        }

        public static int CalculateCurrentRow(int labelsInCurrentPage, int columnsPerPage, int currentColumn)
        {
            var currentRow = (labelsInCurrentPage + 1) / columnsPerPage;

            if (currentColumn != columnsPerPage)
            {
                // New row
                currentRow = ++currentRow;
            }
            return currentRow;
        }

        public static double CalculateContentPositionLeft(int currentColumn, LabelSettings settings)
        {
            // Set horisontal position of label based on how many labels is allready printed on same row
            var positionX = ((currentColumn - 1) * settings.LabelPositionX);
            positionX = positionX + settings.LabelMarginLeft + settings.LabelPaddingLeft;
            return positionX;
        }
        public static double CalculateContentPositionTop(double currentRow, LabelSettings settings)
        {
            // Set vertical position of label based on how many labels is allready printed above
            var positionY = ((currentRow - 1) * settings.LabelPositionY);
            positionY = positionY + settings.LabelMarginTop + settings.LabelPaddingTop;
            return positionY;
        }

        public static XRect CreateRectangle(double contentLeftPosition, double contentTopPosition, XSize contentSize)
        {
            var pointX = XUnit.FromMillimeter(contentLeftPosition).Point;
            var pointY = XUnit.FromMillimeter(contentTopPosition).Point;
            var rectangle = new XRect(new XPoint(pointX, pointY), contentSize);
            return rectangle;
        }

        public static string FormatLabelText(string[] rows, int maxCharacters)
        {
            for (int i = 0; i < rows.Count(); i++)
            {
                if (!string.IsNullOrWhiteSpace(rows[i]))
                {
                    if (rows[i].Length > maxCharacters)
                    {
                        rows[i] = rows[i].Substring(0, maxCharacters);
                    }
                    rows[i] += Environment.NewLine;
                }
            }
            return string.Concat(rows);
        }
    }
}
