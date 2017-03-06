using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinForms_GridView.Data
{
    internal class ScrollRows
    {
        double rowHeight;
        double totalHeight;

        public IDictionary<int, double> RowIndexPair { get; set; }

        public ScrollRows(double tHeight, double rHeight)
        {
            totalHeight = tHeight;
            rowHeight = rHeight;
            RowIndexPair = new Dictionary<int, double>();
        }

        private void CaculateRecordIndexRatio()
        {
            var totalRecordsCount = Math.Round(totalHeight / rowHeight);
            for (int i = 0; i < totalRecordsCount; i++)
            {
                RowIndexPair.Add(i, rowHeight);
            }
        }
    }
}
