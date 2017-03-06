using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms_GridView.Data;

namespace XamarinForms_GridView.Control
{
    public class GridPanel : Layout<View>
    {
        internal Func<object, Task<View>> CreateTemplate;
        internal IList<View> RowPanel;
        internal GridView SGridView;

        private double viewPortHeight = 0;
        private double viewPortWidth = 0;

        public GridPanel(GridView gridView)
        {
            SGridView = gridView;
            initSetup();
        }

        private void initSetup()
        {
            RowPanel = Children;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var size = new SizeRequest();
            var height = double.MinValue;
            if (!double.IsInfinity(heightConstraint))
            {
                viewPortHeight = heightConstraint;
                height = (DependencyService.Get<IDataView>().ItemsSource as IList).Count * SGridView.GridSize.RowHeight;
            }
            if (!double.IsInfinity(widthConstraint))
            {
                viewPortWidth = widthConstraint;
            }
            size.Request = new Size(-1, height);
            return size;
        }

        internal async void GenerateRows()
        {
            var itemList = DependencyService.Get<IDataView>().ItemsSource;
            var itemsSource = itemList as IList;
            if (itemsSource.Count <= 0)
                return;
            int rowIndex = 1;
            foreach (var item in itemsSource)
            {
                var rowPanel = new GridRowPanel();
                rowPanel.RowIndex = rowIndex;
                for (int i = 0; i < 2; i++)
                {
                    var cellPanel = new GridCellPanel();
                    cellPanel.ColIndex = i;
                    var view = await CreateTemplate(item);
                    cellPanel.Children.Add(view);
                    rowPanel.Children.Add(cellPanel);
                }
                rowPanel.BindingContext = item;
                Children.Add(rowPanel);
                rowIndex++;
            }
            ForceLayout();
        }

        internal async void UpdateBindingContext()
        {

        }

        private bool inBindingContextChange;
        protected override void OnBindingContextChanged()
        {
            inBindingContextChange = true;
            base.OnBindingContextChanged();
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();
        }

        protected override void InvalidateMeasure()
        {
            base.InvalidateMeasure();
        }

        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return false;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double top = y;
            foreach (var child in Children)
            {
                child.Layout(new Rectangle(x, top, width, SGridView.GridSize.RowHeight));
                top = (child as GridRowPanel).RowIndex * SGridView.GridSize.RowHeight;
            }
        }
    }
}
