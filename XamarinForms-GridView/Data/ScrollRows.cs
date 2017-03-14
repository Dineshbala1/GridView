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
        private double viewPortHeight;
        private int resetRowIndex = 0;

        public IDictionary<int,RowInfoModel> VisibleRowCollection { get; set; }
        public IDictionary<int, RowInfoModel> TotalRowCollection { get; set; }

        public ScrollRows(double vpHeight,double tHeight, double rHeight)
        { 
            totalHeight = tHeight;
            rowHeight = rHeight;
            viewPortHeight = vpHeight;
            TotalRowCollection = new Dictionary<int, RowInfoModel>();
            CaculateRecordIndexRatio();
            CalculateVisibleRowsCalculation();

        }

        public void CalculateVisibleRowsCalculation()
        {
            if (VisibleRowCollection == null)
                VisibleRowCollection = new Dictionary<int, RowInfoModel>();
            var visibleRecordsCount = Math.Round((viewPortHeight) / rowHeight);
            double rowStartHeight = 0;
            for (var index = 1; index <= visibleRecordsCount; index++)
            {
                var rowInfoModel = new RowInfoModel()
                {
                    RowIndex = index,
                    StartValue = rowStartHeight,
                    EndValue = rowHeight * index
                };
                VisibleRowCollection.Add(index, rowInfoModel);
                rowStartHeight = rowHeight * index;
            }
        }

        public int? GetNextVisibleRowIndex(double offset)
        {
            var totalOffset = offset + viewPortHeight;
            return TotalRowCollection.Values.FirstOrDefault(row => totalOffset > row.StartValue && totalOffset < row.EndValue)?
                .RowIndex;
        }

        public int? GetResuableIndex(double offset)
        {
            var value = 
                VisibleRowCollection.Values.FirstOrDefault(
                        row => offset > row.StartValue && offset < row.EndValue)?
                    .RowIndex;
            if (value == null)
            {
                
            }
            return value;
        }

        public int? GetNextRowIndex(double offset)
        {
            var totalOffset = offset + viewPortHeight;
            return
                VisibleRowCollection.Values.FirstOrDefault(
                        row => totalOffset > row.StartValue && totalOffset < row.EndValue)?
                    .RowIndex;
        }

        private void CaculateRecordIndexRatio()
        {
            var totalRecordsCount = Math.Round((totalHeight) / rowHeight);
            double rowStartHeight = 0;
            for (var index = 1; index <= totalRecordsCount; index++)
            {
                var rowInfoModel = new RowInfoModel()
                {
                    RowIndex = index,
                    StartValue = rowStartHeight,
                    EndValue = rowHeight * index
                };
                TotalRowCollection.Add(index, rowInfoModel);
                rowStartHeight = rowHeight * index;
            }
        }

        //public IList<int> ResetRowIndex(double scrollY)
        //{
        //    return TotalRowCollection.Values.Where(row => CheckNumnber(row, Math.Round(scrollY)))
        //        .Select(row => row.RowIndex)
        //        .ToList();
        //}

        public void UpdateScrollRows(int rowIndex)
        {
            VisibleRowCollection[rowIndex] = TotalRowCollection[rowIndex];
        }

        public int ResetRowIndex(double scrollY)
        {
            try
            {
                var rowIndex =
                    TotalRowCollection.Values.Where(
                            row => row.RowIndex != resetRowIndex && CheckNumnber(row, Math.Round(scrollY)))?
                        .Select(row => row.RowIndex)?.First();
                resetRowIndex = rowIndex ?? -1;
                return resetRowIndex;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private bool GetNewRowIndex(RowInfoModel rowData, double checkableValue)
        {
            return checkableValue > rowData.StartValue && checkableValue <= rowData.EndValue;
        }

        private bool CheckNumnber(RowInfoModel rowData, double checkableValue)
        {
            return checkableValue > rowData.StartValue && checkableValue > rowData.EndValue;
        }

        public int? GetRowIndex(double offset, double vpHeight)
        {
            double calculatedHeight = 0;
            //if (offset > vpHeight)
            //    calculatedHeight = offset - vpHeight;
            //else
            calculatedHeight = offset + vpHeight;
            var newRowIndex =
                TotalRowCollection.Values.Where(row => GetNewRowIndex(row, Math.Round(calculatedHeight +120)))?
                    .Select(row => row.RowIndex)?.First();
            //if (resetRowIndex != -1)
            //    TotalRowCollection[resetRowIndex] = new RowInfoModel() { RowIndex = resetRowIndex,EndValue = TotalRowCollection[newRowIndex.Value].EndValue,StartValue = TotalRowCollection[newRowIndex.Value].StartValue};
            return newRowIndex;
        }

        //public IList<int> GetRowIndex(double offset, double vpHeight)
        //{
        //    double calculatedHeight = 0;
        //    if (offset > vpHeight)
        //        calculatedHeight = offset - vpHeight;
        //    else
        //        calculatedHeight = offset + vpHeight;
        //    return
        //        TotalRowCollection.Values.Where(row => CheckNumnber(row, Math.Round(calculatedHeight)))
        //            .Select(row => row.RowIndex)
        //            .ToList();
        //}
    }
}
