using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public class LabelSettings_A4_99x33_L7162 : LabelSettings
    {
        public LabelSettings_A4_99x33_L7162()
        {
            columnsPerPage = 2;
            rowsPerPage = 8;
            pageWidth = 210; // A4
            pageHeight = 297; // A4
            labelBaseWidth = 99.1;
            labelBaseHeight = 33.9;
            labelPaddingLeft = 8;
            labelPaddingTop = 5;
            labelPaddingRight = 5;
            labelPaddingBottom = 0;
            labelMarginTop = 13.7;
            labelMarginLeft = 4.7;
            labelPositionX = 101.6;
            labelPositionY = 33.9;
            fontSize = 9;
            fontFamily = "Arial";
            maxCharactersPerRow = 65;
        }
    }
}
