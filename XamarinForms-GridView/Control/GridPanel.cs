using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinForms_GridView.Data;

namespace XamarinForms_GridView.Control
{
    public class GridPanel : Layout<View>
    {
        internal IList<View> RowPanel;
        internal GridView SGridView;

        private double viewPortHeight = 0;
        private double viewPortWidth = 0;
        private double verticalOffset = 0;
        private double prevOffset = 0;
        private double rowCount = -1;
        private double reusedRow = -1;
        private bool inReverse;

        public GridPanel(GridView gridView)
        {
            SGridView = gridView;
            initSetup();
        }

        private void initSetup()
        {
            RowPanel = Children;
            SGridView.Scrolled += SGridView_Scrolled;
        }

        //TODO : UI Virtualization needs to be added.
        //TODO : Data Virtualization needs to be added.
        private void SGridView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (SGridView.Orientation != ScrollOrientation.Vertical)
                return;
            verticalOffset = Math.Round(e.ScrollY);
            if (!(verticalOffset > SGridView.GridSize.RowHeight + 15))
            {
                var rowIndex = SGridView.scrollRows.GetNextRowIndex(e.ScrollY);
                if (rowIndex != null && rowIndex != -1)
                {
                    var recordValue = RowPanel.FirstOrDefault(row => (row as GridRowPanel).VisibleRowIndex == rowIndex);
                    if (recordValue == null)
                    {
                        UpdateBindingContext(rowIndex.Value, -1);
                    }
                }
                return;
            }
            var reusedRow = SGridView.scrollRows.GetNextVisibleRowIndex(e.ScrollY);
            if (reusedRow == null)
                return;
            {
                var reuseRecord =
                (RowPanel.FirstOrDefault(
                    row =>
                        prevOffset < verticalOffset
                            ? verticalOffset > ((row as GridRowPanel).Height * (row as GridRowPanel).RowIndex)
                            : verticalOffset < ((row as GridRowPanel).Height * (row as GridRowPanel).RowIndex)));
                var getReusableIndex = reuseRecord != null ? (reuseRecord as GridRowPanel).RowIndex : -1;
                if (getReusableIndex == -1)
                {

                }
                if (prevOffset <= verticalOffset)
                {
                    foreach (var child in Children)
                    {
                        if ((child as GridRowPanel).RowIndex == getReusableIndex)
                            (child as GridRowPanel).RowIndex = -1;
                    }
                    UpdateBindingContext(reusedRow.Value, -1);
                }
                else
                {
                    UpdateBindingContext(reusedRow.Value, getReusableIndex);
                }
            }
            prevOffset = verticalOffset;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var size = new SizeRequest();
            var height = heightConstraint;
            var width = widthConstraint;
            if (!double.IsInfinity(heightConstraint) && SGridView.Orientation == ScrollOrientation.Vertical)
            {
                viewPortHeight = heightConstraint;
                var list = DependencyService.Get<IDataView>().ItemsSource as IList;
                if (list != null)
                    height = Math.Round(Math.Round(list.Count * SGridView.GridSize.RowHeight) / 2);
                GenerateRows();
            }
            if (SGridView.Orientation == ScrollOrientation.Horizontal)
            {
                viewPortWidth = widthConstraint;
                width = (((IList)DependencyService.Get<IDataView>().ItemsSource).Count - 1) *
                        SGridView.GridSize.RowHeight;
            }
            size.Request = new Size(-1, height);
            return size;
        }

        internal void GenerateRows()
        {
            if (Children.Count > 0)
            {
                Debug.WriteLine("ReMeasured");
                return;
            }
            var itemList = DependencyService.Get<IDataView>().ItemsSource;
            var itemsSource = itemList as IList;
            if (itemsSource != null && itemsSource.Count <= 0)
                return;
            rowCount = Math.Round(viewPortHeight / SGridView.GridSize.RowHeight) * 2;
            var rowPanel = new GridRowPanel();
            for (var rowIndexer = 0; rowIndexer < rowCount; rowIndexer++)
            {
                if (rowPanel == null)
                    rowPanel = new GridRowPanel();
                var columnIndex = rowIndexer % 2;
                var rowIndex = (int)Math.Floor(rowIndexer / (float)2);
                var cellPanel = new GridCellPanel { ColIndex = columnIndex };
                var view = GenerateViewFromTemplate(itemsSource[rowIndexer]);
                cellPanel.BindingContext = itemsSource[rowIndexer];
                cellPanel.Children.Add(view);
                rowPanel.Children.Add(cellPanel);
                rowPanel.VisibleRowIndex = rowIndex;
                rowPanel.RowIndex = ++rowIndex;
                if (columnIndex < 1)
                    continue;
                Children.Add(rowPanel.Clone());
                rowPanel = null;
            }
            ForceLayout();
        }

        private View GenerateViewFromTemplate(object item1)
        {
            var tapGestureRecognizer = new TapGestureRecognizer
            {
                Command = SGridView.ClickCommand,
            };
            var buildTile = (View)SGridView.ItemTemplate.CreateContent();
            buildTile.HeightRequest = SGridView.GridSize.RowHeight;
            buildTile.WidthRequest = SGridView.GridSize.ColWidth;
            buildTile.GestureRecognizers.Add(tapGestureRecognizer);
            buildTile.BindingContextChanged += BuildTile_BindingContextChanged;
            return buildTile;
        }

        private void BuildTile_BindingContextChanged(object sender, EventArgs e)
        {
            var view = sender as View;
            var tapGestureRecognizer = view?.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGestureRecognizer != null)
                tapGestureRecognizer.CommandParameter =
                    view.BindingContext;
        }

        private bool isForceLayout;

        internal void UpdateBindingContext(int newRowIndex, int oldRowIndex)
        {
            if (newRowIndex == -1)
                return;
            UpdateAndCreateRow(newRowIndex, oldRowIndex);
        }

        private void UpdateAndCreateRow(int newRowIndex, int oldRowIndex)
        {
            var itemList = DependencyService.Get<IDataView>().ItemsSource;
            var itemsSource = itemList as IList;
            if (oldRowIndex != -1)
            {
                var record = RowPanel.FirstOrDefault(row => (row as GridRowPanel).RowIndex == newRowIndex);
                UpdateRecordBindingContext(oldRowIndex, record as GridRowPanel, itemsSource,newRowIndex);
                return;
            }
            var rowPanel = new GridRowPanel();
            for (var rowIndexer = 0; rowIndexer < 2; rowIndexer++)
            {
                if (rowPanel == null)
                    rowPanel = new GridRowPanel();
                var columnIndex = rowIndexer % 2;
                rowPanel.VisibleRowIndex = newRowIndex;
                var cellPanel = new GridCellPanel { ColIndex = columnIndex };
                var view = GenerateViewFromTemplate(itemsSource[(newRowIndex * 2) - rowIndexer]);
                cellPanel.BindingContext = itemsSource[(newRowIndex * 2) - rowIndexer];
                cellPanel.Children.Add(view);
                rowPanel.Children.Add(cellPanel);
                if (columnIndex < 1)
                    continue;
                rowPanel.RowIndex = ++newRowIndex;
                if (Children.All(row =>
                {
                    var gridRowPanel = row as GridRowPanel;
                    return gridRowPanel != null && gridRowPanel.RowIndex != rowPanel.RowIndex;
                }))
                {
                    if (Children.Any(row =>
                    {
                        var gridRowPanel = row as GridRowPanel;
                        return gridRowPanel != null && gridRowPanel.RowIndex == -1;
                    }))
                    {
                        UpdateRecordBindingContext(newRowIndex - 1, rowPanel, itemsSource,oldRowIndex);
                    }
                    else
                    {
                        Children.Add(rowPanel.Clone());
                        Debug.WriteLine("New Child added at row {0}", rowPanel.RowIndex);
                        RowPanel = Children;
                        ForceLayout();
                    }
                }
                rowPanel = null;
            }
        }

        private void UpdateRecordBindingContext(int newRowIndex, GridRowPanel rowPanel, IList itemsSource, int oldRowIndex)
        {
            var record =
                (Children.FirstOrDefault(row =>
                {
                    var gridRowPanel = row as GridRowPanel;
                    return gridRowPanel != null && gridRowPanel.RowIndex == oldRowIndex;
                }) as GridRowPanel);
            if (record == null)
                return;
            record.RowIndex = oldRowIndex !=-1 ? newRowIndex : rowPanel.RowIndex;
            var i = 1;
            foreach (var recordChild in record.Children)
            {
                if (i > -1)
                {
                    recordChild.BindingContext = null;
                    recordChild.BindingContext = itemsSource[(newRowIndex * 2) - i];
                }
                --i;
            }
        }

        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return false;
        }

        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return false;
        }

        protected override void OnChildMeasureInvalidated()
        {

        }

        bool inLayout = false;

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var top = y;
            RowPanel = RowPanel.OrderBy(row => ((GridRowPanel)row).RowIndex).ToList();
            inLayout = true;
            foreach (var child in RowPanel)
            {
                if (((GridRowPanel)child).RowIndex != 0)
                    top = ((((GridRowPanel)child).RowIndex - 1) * SGridView.GridSize.RowHeight);
                child.Layout(new Rectangle(x, top, width, SGridView.GridSize.RowHeight));
            }
            isForceLayout = false;
        }
    }
}
