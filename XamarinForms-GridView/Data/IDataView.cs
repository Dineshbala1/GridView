using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamarinForms_GridView.Data
{
    public interface IDataView
    {
        
        object ItemsSource { get; set; }
    }
}
