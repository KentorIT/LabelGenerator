using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public class LabelSettings_A4_3Columns8Rows : LabelSettings
    {
        public LabelSettings_A4_3Columns8Rows()
        {
            columnsPerPage = 3;
            rowsPerPage = 8;
            pageWidth = 210; // A4
            pageHeight = 297; // A4
            labelBaseWidth = 70;
            labelBaseHeight = 37;
            labelPaddingLeft = 5;
            labelPaddingTop = 5;
            labelPaddingRight = 5;
            labelPaddingBottom = 5;
            labelMarginTop = 5;
            labelMarginLeft = 0;
            labelpositionY = 37;
            labelpositionX = 70;
            fontSize = 8;
            fontFamily = "Arial";
            maxCharactersPerRow = 50;
        }
    }
}
