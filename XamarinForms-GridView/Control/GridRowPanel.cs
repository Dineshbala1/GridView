using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinForms_GridView.Control
{
    public class GridRowPanel : Layout<View>
    {
        public int VisibleRowIndex { get; set; }
        public int RowIndex { get; set; }
        public bool IsDirty { get; set; }

        public GridRowPanel()
        {
            BackgroundColor = Color.Transparent;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double left = x;
            var maxColIndex = Children.Max(row => (row as GridCellPanel).ColIndex);
            maxColIndex += 1;
            foreach (var item in Children)
            {
                item.Layout(new Rectangle(left, y, width / maxColIndex, height));
                left += (width / maxColIndex);
            }
            if (IsDirty)
                IsDirty = false;
        }

        public GridRowPanel Clone()
        {
            var rowPanel = this;
            return rowPanel;
        }
    }
}
