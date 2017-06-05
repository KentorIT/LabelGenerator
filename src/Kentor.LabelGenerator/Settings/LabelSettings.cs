using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentor.LabelGenerator.Settings
{
    public class LabelSettings
    {
        protected int columnsPerPage;
        protected int rowsPerPage;
        protected double pageWidth;
        protected double pageHeight;
        protected double labelBaseWidth;
        protected double labelBaseHeight;
        protected double labelPaddingLeft;
        protected double labelPaddingTop;
        protected double labelPaddingRight;
        protected double labelPaddingBottom;
        protected double labelMarginLeft;
        protected double labelMarginTop;
        protected double labelpositionX;
        protected double labelpositionY;
        protected int fontSize;
        protected string fontFamily;
        protected int maxCharactersPerRow;

        public int ColumnsPerPage { get { return columnsPerPage; } }
        public int RowsPerPage { get { return rowsPerPage; } }
        public int LabelsPerPage { get { return columnsPerPage * rowsPerPage; } }
        public double PageWidth { get { return pageWidth; } }
        public double PageHeight { get { return pageHeight; } }
        public double LabelWidth { get { return labelBaseWidth - labelPaddingLeft - labelPaddingRight; } }
        public double LabelHeight { get { return labelBaseHeight - labelPaddingTop - labelPaddingBottom; } }
        public double LabelPaddingLeft { get { return labelPaddingLeft; } }
        public double LabelPaddingTop { get { return labelPaddingTop; } }
        public double LabelPaddingRight { get { return labelPaddingRight; } }
        public double LabelPaddingBottom { get { return labelPaddingBottom; } }
        public double LabelMarginLeft { get { return labelMarginLeft; } }
        public double LabelMarginTop { get { return labelMarginTop; } }
        public double LabelPositionX { get { return labelpositionX; } }
        public double LabelPositionY { get { return labelpositionY; } }
        public int FontSize { get { return fontSize; } }
        public string FontFamily { get { return fontFamily; } }
        public int MaxCharactersPerRow { get { return maxCharactersPerRow; } }
    }
}
