using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms_StaggeredGrid.Control;

namespace XamarinForms_StaggeredGrid
{
    public class StaggeredGridView : Layout
    {

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(object), typeof(StaggeredGridView), null, BindingMode.Default, null, (bindable,oldvalue,newvalue) => { OnItemsSourceProperyChanged(bindable, oldvalue, newvalue); }, null, null, null);

        private static void OnItemsSourceProperyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {

        }

        public object ItemsSource
        {
            get
            {
                return GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public StaggeredGridView()
        {
            var panel = new GridView();
            panel.BackgroundColor = Color.Red;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {

        }
    }
}
