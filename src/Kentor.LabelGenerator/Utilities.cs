using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public static class Utilities
    {
        public static IEnumerable<DocumentType> GetDocumentTypes()
        {
            var types = Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>();
            return types;
        }

        public static LabelSettings GetSettings(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.A4_2Columns8Rows:
                    return new LabelSettings_A4_2Columns8Rows();
                case DocumentType.A4_3Columns8Rows:
                    return new LabelSettings_A4_3Columns8Rows();
                default:
                    return new LabelSettings();
            }
        }
    }
}
