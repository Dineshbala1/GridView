using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinForms_GridView.Data;

namespace XamarinForms_GridView.Control
{
    public class GridView : ScrollView
    {
        private static float defaultColWidth = 120;
        internal ScrollRows scrollRows;
        internal GridSize GridSize;

        #region BindableProperties

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(GridView));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create("ClickCommand",
            typeof(ICommand), typeof(GridView));

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource",
            typeof(object), typeof(GridView), null, BindingMode.TwoWay, null,
            OnItemsSourcePropertyChanged, null, null, null);

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(BindableObject sender, object oldvalue, object newvalue)
        {
            var grid = sender as GridView;
            if (newvalue != null)
            {
                grid.DataView.ItemsSource = newvalue;
                //grid.gridPanel.GenerateRows();
            }
        }

        private IDataView dataView;

        public IDataView DataView
        {
            get { return dataView ?? (dataView = DependencyService.Get<IDataView>()); }
        }

        //TODO: Need to make TileHeight and TileWidth as Bindable Property.
        public static readonly BindableProperty TileWidthProperty = BindableProperty.Create("TileWidth",
            typeof(float), typeof(GridView), defaultColWidth, BindingMode.Default, null,
            onTileWidthPropertyChanged);

        private static void onTileWidthPropertyChanged(BindableObject sender, object oldvalue, object newvalue)
        {
            var grid = sender as GridView;
            if (newvalue != null)
            {
                //grid.CalcMaxWidth();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// ItemTemplate to create and customize the cell UI.
        /// There is no Default template available so this is a mandatory field.
        /// </summary>
        public DataTemplate ItemTemplate { get; set; }

        private int colCount;
        public int ColCount
        {
            get { return colCount; }
            set { colCount = value; }
        }

        private float tileHeight;
        public float TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        private float tilewidth;
        public float TileWidth
        {
            get { return (float)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        #endregion

        #region Ctor

        public GridView()
        {
            InitSetup();
        }

        private void InitSetup()
        {
            ColCount = 2;
            TileHeight = 120;
            TileWidth = 120;
            Padding = new Thickness(5);
            Content = new GridPanel(this);
            GridSize =  new GridSize(TileHeight, TileWidth);
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        //TODO: Option to enable StaggeredGrid needs to be added.
        private async Task<View> GenerateViewFromTemplate(object item1)
        {
            return await Task.Run(() =>
            {
                var buildTile = (View)ItemTemplate.CreateContent();
                var tapGestureRecognizer = new TapGestureRecognizer
                {
                    Command = ClickCommand,
                    CommandParameter = item1
                };
                buildTile.BindingContext = item1;
                buildTile.HeightRequest = TileHeight;
                buildTile.WidthRequest = TileWidth;
                buildTile.GestureRecognizers.Add(tapGestureRecognizer);
                return buildTile;
            });
        }

        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return false;
        }

        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return false;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var sizeRequest = Content.Measure(width, height);
            if (scrollRows == null)
                scrollRows = new ScrollRows(height,sizeRequest.Request.Height, TileHeight);
            Content.Layout(new Rectangle(x, y, width, sizeRequest.Request.Height));
        }
    }
}
