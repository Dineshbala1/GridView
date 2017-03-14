using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinForms_GridView.Control
{
    public class GridCellPanel : Layout<View>
    {
        public int ColIndex { get; set; }

        public GridCellPanel()
        {
            BackgroundColor = Color.Transparent;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            foreach (var item in Children)
            {
                //item.BindingContext = BindingContext;
                item.Layout(new Rectangle(x, y, width, height));
            }
        }
    }
}
