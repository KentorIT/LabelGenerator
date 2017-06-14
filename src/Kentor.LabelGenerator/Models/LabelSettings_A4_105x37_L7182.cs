using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public class LabelSettings_A4_105x37_L7182 : LabelSettings
    {
        public LabelSettings_A4_105x37_L7182()
        {
            columnsPerPage = 2;
            rowsPerPage = 8;
            pageWidth = 210; // A4
            pageHeight = 297; // A4
            labelBaseWidth = 105;
            labelBaseHeight = 37;
            labelPaddingLeft = 8;
            labelPaddingTop = 10;
            labelPaddingRight = 5;
            labelPaddingBottom = 0;
            labelMarginTop = 0;
            labelMarginLeft = 0;
            labelPositionX = 105;
            labelPositionY = 37;
            fontSize = 9;
            fontFamily = "Arial";
            maxCharactersPerRow = 65;
        }
    }
}
