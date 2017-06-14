using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kentor.LabelGenerator;
using FluentAssertions;
using System.IO;
using Kentor.LabelGenerator.Models;
using System.ComponentModel.DataAnnotations;

namespace Kentor.LabelGeneratorTests
{
    [TestClass]
    public class GeneratePdfTests
    {
        string[][] testAddresses;

        [TestInitialize]
        public void Init()
        {
            var addressList = new List<string[]>();
            for (int i = 0; i < 30; i++)
            {
                var address = new string[] { "TolvanTolvanTolvanTolvanTolvanTolvanTolvanTolvanTolvanTo TolvanssonTolvanTolvanssonTolvanTolvanssonTolvanTo", "c/o Elvan Elvansson", "Tolvvägen 12", "12345 Tolvstad" };
                addressList.Add(address);
                address = new string[] { "Anita Andersson", "c/o Ante Andersson", "Betavägen 2", "12345 Saltö", "Sverige" };
                addressList.Add(address);
                address = new string[] { "Bertil Cederqvist", "", "Djurövägen 2", "12345 Djurö" };
                addressList.Add(address);
            }
            testAddresses = addressList.ToArray();
        }

        [TestMethod]
        public void TestSettingsAreGeneratedForSpecificDocumentType()
        {
            var docType = DocumentType.A4_105x37_L7182;
            var settings = Utilities.GetSettings(docType);
            settings.Should().BeOfType<LabelSettings_A4_105x37_L7182>();

            docType = DocumentType.A4_70x37_L7180;
            settings = Utilities.GetSettings(docType);
            settings.Should().BeOfType<LabelSettings_A4_70x37_L7180>();

            docType = DocumentType.A4_70x36_L7181;
            settings = Utilities.GetSettings(docType);
            settings.Should().BeOfType<LabelSettings_A4_70x36_L7181>();

            docType = DocumentType.A4_99x33_L7162;
            settings = Utilities.GetSettings(docType);
            settings.Should().BeOfType<LabelSettings_A4_99x33_L7162>();
        }

        [TestMethod]
        public void TestNewPageWhenMaximumLabelCountIsReached()
        {
            testAddresses = testAddresses.Concat(testAddresses).ToArray();

            var documentType = DocumentType.A4_105x37_L7182;
            var result = DocumentHelpers.CreateDocument(testAddresses, documentType);
            result.PageCount.Should().BeGreaterThan(1);
        }

        [TestMethod]
        public void TestRectangleReturnsValue()
        {
            var settings = new LabelSettings_A4_105x37_L7182();
            var contentSize = DocumentHelpers.GetContentSize(settings);
            var result = DocumentHelpers.CreateRectangle(settings.LabelPositionX, settings.LabelPositionY, contentSize);
            result.IsEmpty.Should().BeFalse();
        }

        [TestMethod]
        public void TestAddPageHeightAndWidthShouldBeConvertedToTypePoint()
        {
            var settings = new LabelSettings_A4_105x37_L7182();
            var document = new PdfSharp.Pdf.PdfDocument();
            var result = DocumentHelpers.AddPage(document, settings);

            result.Width.Type.Should().Be(PdfSharp.Drawing.XGraphicsUnit.Point);
            result.Height.Type.Should().Be(PdfSharp.Drawing.XGraphicsUnit.Point);
        }

        [TestMethod]
        public void TestAddressesAreFormattedCorrectly()
        {
            string[] address = new string[] { "Tolvan Tolvansson", "c/o Elvan Elvansson", "Tolvgatan 12", "12345 Tolvstad", "Sverige" };
            var result = DocumentHelpers.FormatLabelText(address, 60);
            result.Should().Be("Tolvan Tolvansson\r\nc/o Elvan Elvansson\r\nTolvgatan 12\r\n12345 Tolvstad\r\nSverige\r\n");
        }

        [TestMethod]
        public void TestLongLabelTextIsTruncated()
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var randomText = new string(Enumerable.Repeat(chars, 80).Select(s => s[random.Next(s.Length)]).ToArray());
            string[] address = new string[] { $"Tolvan {randomText}" };

            var settings = new LabelSettings_A4_105x37_L7182();
            var totalMaxLength = Environment.NewLine.Length + settings.MaxCharactersPerRow;

            var result = DocumentHelpers.FormatLabelText(address, settings.MaxCharactersPerRow);
            result.Length.Should().Be(totalMaxLength);
        }

        [TestMethod]
        public void TestEmptyRowsAreExcluded()
        {
            string[] address = new string[] { "Tolvan Tolvansson", null, "", "Tolvgatan 12", "12345 Tolvstad" };

            var settings = new LabelSettings_A4_105x37_L7182();
            var result = DocumentHelpers.FormatLabelText(address, settings.MaxCharactersPerRow);
            result.Should().Be("Tolvan Tolvansson\r\nTolvgatan 12\r\n12345 Tolvstad\r\n");
        }

        [TestMethod]
        public void TestEmptyArrayReturnsEmptyLabel()
        {
            string[] address = new string[] { string.Empty, null, "" };

            var settings = new LabelSettings_A4_105x37_L7182();
            var result = DocumentHelpers.FormatLabelText(address, settings.MaxCharactersPerRow);
            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void TestColumnCalculation()
        {
            //---- 2 COLUMNS PER PAGE

            var columnsPerPage = 2;
            var labelsInPage = 0;
            var result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column

            columnsPerPage = 2;
            labelsInPage = 1;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(2); // Second column

            columnsPerPage = 2;
            labelsInPage = 2;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column

            //---- 3 COLUMNS PER PAGE

            columnsPerPage = 3;
            labelsInPage = 0;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column

            columnsPerPage = 3;
            labelsInPage = 1;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(2); // Second column

            columnsPerPage = 3;
            labelsInPage = 2;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(3); // Third column

            columnsPerPage = 3;
            labelsInPage = 3;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column

            //---- 4 COLUMNS PER PAGE

            columnsPerPage = 4;
            labelsInPage = 0;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column

            columnsPerPage = 4;
            labelsInPage = 1;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(2); // Second column

            columnsPerPage = 4;
            labelsInPage = 2;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(3); // Third column

            columnsPerPage = 4;
            labelsInPage = 3;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(4); // Forth column

            columnsPerPage = 4;
            labelsInPage = 4;
            result = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            result.Should().Be(1); // First column
        }

        [TestMethod]
        public void TestRowCalculation()
        {
            //---- 2 COLUMNS PER PAGE

            var columnsPerPage = 2;
            var labelsInPage = 0; // No labels added yet
            int currentRow;
            var currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            // New row
            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(2);

            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(2);

            // New row
            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(3);

            //---- 3 COLUMNS PER PAGE

            columnsPerPage = 3;
            labelsInPage = 0; // No labels added yet
            currentRow = 1; // Start value
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);


            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            // New row
            labelsInPage++;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(2);

            //----  4 COLUMNS PER PAGE

            columnsPerPage = 4;
            labelsInPage = 0; // No labels added yet
            currentRow = 1; // Start value
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            labelsInPage = 3;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(1);

            // New row
            labelsInPage = 4;
            currentColumn = DocumentHelpers.CalculateCurrentColumn(labelsInPage, columnsPerPage);
            currentRow = DocumentHelpers.CalculateCurrentRow(labelsInPage, columnsPerPage, currentColumn);
            currentRow.Should().Be(2);
        }

        [TestMethod]
        public void TestContentPositionCalculation()
        {
            var settings = new LabelSettings_A4_105x37_L7182();
            var currentColumn = 1;

            var result = DocumentHelpers.CalculateContentPositionLeft(currentColumn, settings);
            result.Should().Be(settings.LabelPaddingLeft + settings.LabelMarginLeft);

            currentColumn = 2;
            result = DocumentHelpers.CalculateContentPositionLeft(currentColumn, settings);
            result.Should().Be(settings.LabelPaddingLeft + settings.LabelMarginLeft + settings.LabelPositionX);

            var currentRow = 1;
            result = DocumentHelpers.CalculateContentPositionTop(currentRow, settings);
            result.Should().Be(settings.LabelPaddingTop + settings.LabelMarginTop);

            currentRow = 2;
            result = DocumentHelpers.CalculateContentPositionTop(currentRow, settings);
            result.Should().Be(settings.LabelPaddingTop + settings.LabelMarginTop + settings.LabelPositionY);
        }

        [TestMethod]
        public void TestDocumentTypeDisplayName()
        {
            var documentType = DocumentType.A4_105x37_L7182;
            var member = typeof(DocumentType).GetMember(documentType.ToString());
            var displayName = (DisplayAttribute)member.First()
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault();

            displayName.Name.Should().Be("Adressetiketter 105 x 37 mm");

            documentType = DocumentType.A4_70x37_L7180;
            member = typeof(DocumentType).GetMember(documentType.ToString());
            displayName = (DisplayAttribute)member.First()
               .GetCustomAttributes(typeof(DisplayAttribute), false)
               .FirstOrDefault();

            displayName.Name.Should().Be("Adressetiketter 70 x 37 mm");

            documentType = DocumentType.A4_70x36_L7181;
            member = typeof(DocumentType).GetMember(documentType.ToString());
            displayName = (DisplayAttribute)member.First()
               .GetCustomAttributes(typeof(DisplayAttribute), false)
               .FirstOrDefault();

            displayName.Name.Should().Be("Adressetiketter 70 x 36 mm");

            documentType = DocumentType.A4_99x33_L7162;
            member = typeof(DocumentType).GetMember(documentType.ToString());
            displayName = (DisplayAttribute)member.First()
               .GetCustomAttributes(typeof(DisplayAttribute), false)
               .FirstOrDefault();

            displayName.Name.Should().Be("Adressetiketter 99,1 x 33,9 mm");
        }

        // TEMP HACK FOR WRITING PDF TO DISC, TO BE REMOVED
        [TestMethod]
        public void TestWriteDocument2ColumnsToDisc()
        {
            var documentType = DocumentType.A4_105x37_L7182;
            var document = DocumentHelpers.CreateDocument(testAddresses, documentType);
            var documentAsByteArray = DocumentHelpers.SaveToArray(document);
            File.WriteAllBytes(@"C:\Temp\TestFile_2Columns.pdf", documentAsByteArray);
        }
        // TEMP HACK FOR WRITING PDF TO DISC, TO BE REMOVED
        [TestMethod]
        public void TestWriteDocument3ColumnsToDisc()
        {
            var documentType = DocumentType.A4_70x37_L7180;
            var document = DocumentHelpers.CreateDocument(testAddresses, documentType);
            var documentAsByteArray = DocumentHelpers.SaveToArray(document);
            File.WriteAllBytes(@"C:\Temp\TestFile_3Columns.pdf", documentAsByteArray);
        }
        // TEMP HACK FOR WRITING PDF TO DISC, TO BE REMOVED
        [TestMethod]
        public void TestWriteDocument2ColumnsWithMarginToDisc()
        {
            var documentType = DocumentType.A4_99x33_L7162;
            var document = DocumentHelpers.CreateDocument(testAddresses, documentType);
            var documentAsByteArray = DocumentHelpers.SaveToArray(document);
            File.WriteAllBytes(@"C:\Temp\TestFile_2Columns_Margin.pdf", documentAsByteArray);
        }
        // TEMP HACK FOR WRITING PDF TO DISC, TO BE REMOVED
        [TestMethod]
        public void TestWriteDocument3ColumnsWithMarginToDisc()
        {
            var documentType = DocumentType.A4_70x36_L7181;
            var document = DocumentHelpers.CreateDocument(testAddresses, documentType);
            var documentAsByteArray = DocumentHelpers.SaveToArray(document);
            File.WriteAllBytes(@"C:\Temp\TestFile_3Columns_Margin.pdf", documentAsByteArray);
        }
    }
}
