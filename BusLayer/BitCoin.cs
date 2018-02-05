using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    public class BitCoin
    {

        public BitCoin()
        {

        }


        private DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValueTableAdapter trnCurrencyValueTableAdapter;

        protected DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValueTableAdapter TrnCurrencyValueTableAdapter
        {
            get
            {
                if (trnCurrencyValueTableAdapter == null)
                {
                    trnCurrencyValueTableAdapter = new DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValueTableAdapter();
                }
                return trnCurrencyValueTableAdapter;
            }
        }

        private DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValue30SecTimerTableAdapter trnCurrencyValue30SecTimerTableAdapter;

        protected DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValue30SecTimerTableAdapter TrnCurrencyValue30SecTimerTableAdapter
        {
            get
            {
                if (trnCurrencyValue30SecTimerTableAdapter == null)
                {
                    trnCurrencyValue30SecTimerTableAdapter = new DataLayer.TrnCurrencyValueTableAdapters.TrnCurrencyValue30SecTimerTableAdapter();
                }
                return trnCurrencyValue30SecTimerTableAdapter;
            }
        }


        /// <summary>
        /// get the first row for a code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataLayer.TrnCurrencyValue.TrnCurrencyValueDataTable GetCurrentListRowForCode(string currencyListCode)
        {
            return TrnCurrencyValueTableAdapter.GetRowsForCurrencyCodeSeconds(currencyListCode);
        }

        public DataLayer.TrnCurrencyValue.TrnCurrencyValue30SecTimerDataTable GetTrnCurrencyValueRowsForCurrencyCodeAndSeconds(string code)
        {
            return TrnCurrencyValue30SecTimerTableAdapter.GetRowdForCurrencyCodeSeconds(code);
        }


        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="newRow"></param>
        public void UpdateCurrencyValue(DataLayer.TrnCurrencyValue.TrnCurrencyValueRow newRow)
        {
            TrnCurrencyValueTableAdapter.Update(newRow);
        }

        public void UpdateCurrencyValue(DataLayer.TrnCurrencyValue.TrnCurrencyValueDataTable newTable)
        {
            TrnCurrencyValueTableAdapter.Update(newTable);
        }


        public class BitCoin30LiveView
        {
            public string DT_RowId { get; set; }
            public string Price { get; set; }
            public string Sell { get; set; }
            public string Buy { get; set; }
            public string High { get; set; }
            public string Low { get; set; }
            public string Volume { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string Date { get; set; }
        }


        public List<BitCoin30LiveView> Get30SecData(string currencyListCode)
        {

            try
            {
                DataLayer.TrnCurrencyValue.TrnCurrencyValue30SecTimerDataTable itemTable = GetTrnCurrencyValueRowsForCurrencyCodeAndSeconds(currencyListCode);

                if (itemTable.Rows.Count > 0)
                {
                    
                    return itemTable.AsEnumerable().Select(item => new BitCoin30LiveView
                    {
                        DT_RowId = item["TrnCurrencyValueGuid"].ToString(),
                        Price = item["Price"].ToString(),
                        Sell = item["Sell"].ToString(),
                        Buy = item["Buy"].ToString(),
                        Low = item["Low"].ToString(),
                        Volume = item["Volume"].ToString(),
                        Name = item["Name"].ToString(),
                        Code = item["Code"].ToString(),
                        Date = item["CreateDate"].ToString()
                    }).ToList();
                }
                else
                {
                    //no rows were found so we return an empty object
                    return null;
                }

            }
            catch (Exception exc)
            {
                throw new Exception("Error in Bitcoin Handler", exc);
            }

        }

    }
}
