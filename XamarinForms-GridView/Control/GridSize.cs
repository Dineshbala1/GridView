using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinForms_GridView.Control
{
    public class GridSize
    {
        public double ColWidth { get; set; }
        public double RowHeight { get; set; }

        public GridSize(double rowHeight, double colWidth)
        {
            RowHeight = rowHeight;
            ColWidth = colWidth;
        }
    }
}
