using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Models
{
    public class LabelSettings_A4_2Columns8Rows : LabelSettings
    {
        public LabelSettings_A4_2Columns8Rows()
        {
            columnsPerPage = 2;
            rowsPerPage = 8;
            pageWidth = 210; // A4
            pageHeight = 297; // A4
            labelBaseWidth = 105;
            labelBaseHeight = 37;
            labelPaddingLeft = 5;
            labelPaddingTop = 5;
            labelPaddingRight = 5;
            labelPaddingBottom = 5;
            labelMarginTop = 5;
            labelMarginLeft = 0;
            fontSize = 9;
            fontFamily = "Arial";
            maxCharactersPerRow = 80;
        }
    }
}
