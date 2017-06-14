using Kentor.LabelGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator
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

                case DocumentType.A4_70x36_L7181:
                    return new LabelSettings_A4_70x36_L7181();
                case DocumentType.A4_70x37_L7180:
                    return new LabelSettings_A4_70x37_L7180();
                case DocumentType.A4_99x33_L7162:
                    return new LabelSettings_A4_99x33_L7162();
                case DocumentType.A4_105x37_L7182:
                    return new LabelSettings_A4_105x37_L7182();
                default:
                    return new LabelSettings();
            }
        }
    }
}
