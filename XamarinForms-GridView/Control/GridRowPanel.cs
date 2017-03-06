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
        public int RowIndex { get; set; }

        public GridRowPanel()
        {
            BackgroundColor = Color.Transparent;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double left = x;
            foreach (var item in Children)
            {
                item.Layout(new Rectangle(x, y, width / 2, height));
                x += (width / 2);
            }
        }
    }
}
