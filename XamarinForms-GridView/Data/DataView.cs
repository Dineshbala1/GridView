using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XamarinForms_GridView.Data;

[assembly: Xamarin.Forms.Dependency(typeof(DataView))]
namespace XamarinForms_GridView.Data
{
    internal class DataView : IDataView
    {
        private object itemssource;

        public object ItemsSource
        {
            get
            {
                return itemssource;
            }
            set
            {
                itemssource = value;
            }
        }
    }
}
