using Kentor.LabelGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator
{
    public static class Create
    {
        public static byte[] PdfAsByteArray(string[][] labelRows, DocumentType documentType)
        {
            var pdfDocument = DocumentHelpers.CreateDocument(labelRows, documentType);
            var documentAsByteArray = DocumentHelpers.SaveToArray(pdfDocument);
            return documentAsByteArray;
        }
    }
}
