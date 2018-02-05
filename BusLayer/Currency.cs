using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    public class Currency
    {

        public Currency()
        {

        }

        private DataLayer.TrnSourceUpdateTableAdapters.TrnSourceUpdateTableAdapter trnSourceUpdateTableAdapter;

        protected DataLayer.TrnSourceUpdateTableAdapters.TrnSourceUpdateTableAdapter TrnSourceUpdateTableAdapter
        {
            get
            {
                if (trnSourceUpdateTableAdapter == null)
                {
                    trnSourceUpdateTableAdapter = new DataLayer.TrnSourceUpdateTableAdapters.TrnSourceUpdateTableAdapter();
                }
                return trnSourceUpdateTableAdapter;
            }
        }

        private DataLayer.TrnAltCoinTraderValueTableAdapters.TrnAltCoinTraderValueTableAdapter trnAltCoinTraderValueTableAdapter;

        protected DataLayer.TrnAltCoinTraderValueTableAdapters.TrnAltCoinTraderValueTableAdapter TrnAltCoinTraderValueTableAdapter
        {
            get
            {
                if (trnAltCoinTraderValueTableAdapter == null)
                {
                    trnAltCoinTraderValueTableAdapter = new DataLayer.TrnAltCoinTraderValueTableAdapters.TrnAltCoinTraderValueTableAdapter();
                }
                return trnAltCoinTraderValueTableAdapter;
            }
        }

        private DataLayer.TrnAltCoinTraderValueTableAdapters.AltCoinTraderSecurityTableAdapter altCoinTraderSecurityTableAdapter;

        protected DataLayer.TrnAltCoinTraderValueTableAdapters.AltCoinTraderSecurityTableAdapter AltCoinTraderSecurityTableAdapter
        {
            get
            {
                if (altCoinTraderSecurityTableAdapter == null)
                {
                    altCoinTraderSecurityTableAdapter = new DataLayer.TrnAltCoinTraderValueTableAdapters.AltCoinTraderSecurityTableAdapter();
                }
                return altCoinTraderSecurityTableAdapter;
            }
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


        private DataLayer.TrnBitStampValueTableAdapters.TrnBitStampValueTableAdapter trnBitStampValueTableAdapter;

        protected DataLayer.TrnBitStampValueTableAdapters.TrnBitStampValueTableAdapter TrnBitStampValueTableAdapter
        {
            get
            {
                if (trnBitStampValueTableAdapter == null)
                {
                    trnBitStampValueTableAdapter = new DataLayer.TrnBitStampValueTableAdapters.TrnBitStampValueTableAdapter();
                }
                return trnBitStampValueTableAdapter;
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

        private DataLayer.TrnLunoValueTableAdapters.TrnLunoValueTableAdapter trnLunoValueTableAdapter;

        protected DataLayer.TrnLunoValueTableAdapters.TrnLunoValueTableAdapter TrnLunoValueTableAdapter
        {
            get
            {
                if (trnLunoValueTableAdapter == null)
                {
                    trnLunoValueTableAdapter = new DataLayer.TrnLunoValueTableAdapters.TrnLunoValueTableAdapter();
                }
                return trnLunoValueTableAdapter;
            }
        }

        private DataLayer.TrnKrakenValueTableAdapters.TrnKrakenValueTableAdapter trnKrakenValueTableAdapter;

        protected DataLayer.TrnKrakenValueTableAdapters.TrnKrakenValueTableAdapter TrnKrakenValueTableAdapter
        {
            get
            {
                if (trnKrakenValueTableAdapter == null)
                {
                    trnKrakenValueTableAdapter = new DataLayer.TrnKrakenValueTableAdapters.TrnKrakenValueTableAdapter();
                }
                return trnKrakenValueTableAdapter;
            }
        }

        private DataLayer.MstCurrencyListTableAdapters.MstCurrencyListTableAdapter mstCurrencyListTableAdapter;

        protected DataLayer.MstCurrencyListTableAdapters.MstCurrencyListTableAdapter MstCurrencyListTableAdapter
        {
            get
            {
                if (mstCurrencyListTableAdapter == null)
                {
                    mstCurrencyListTableAdapter = new DataLayer.MstCurrencyListTableAdapters.MstCurrencyListTableAdapter();
                }
                return mstCurrencyListTableAdapter;
            }
        }

        private DataLayer.PrmCurrencyTableAdapters.PrmCurrencyTableAdapter prmCurrencyTableAdapter;

        protected DataLayer.PrmCurrencyTableAdapters.PrmCurrencyTableAdapter PrmCurrencyTableAdapter
        {
            get
            {
                if (prmCurrencyTableAdapter == null)
                {
                    prmCurrencyTableAdapter = new DataLayer.PrmCurrencyTableAdapters.PrmCurrencyTableAdapter();
                }
                return prmCurrencyTableAdapter;
            }
        }


        public DataLayer.TrnCurrencyValue.TrnCurrencyValue30SecTimerDataTable GetTrnCurrencyValueRowsForCurrencyCodeAndSeconds(string code)
        {
            return TrnCurrencyValue30SecTimerTableAdapter.GetRowdForCurrencyCodeSeconds(code);
        }


        public DataLayer.MstCurrencyList.MstCurrencyListRow GetCurrentListRowForCode(string code)
        {
            return MstCurrencyListTableAdapter.GetRowForCode(code)[0];
        }

        public DataLayer.PrmCurrency.PrmCurrencyDataTable GetCurrencyRows()
        {
            return PrmCurrencyTableAdapter.GetRows();
        }

        public DataLayer.MstCurrencyList.MstCurrencyListDataTable GetCurrentListRows()
        {
            return MstCurrencyListTableAdapter.GetData();
        }

        public DataLayer.TrnSourceUpdate.TrnSourceUpdateRow GetLastTrnSourceUpdateRowForSource(int PrmCurrencySourceId)
        {
            return TrnSourceUpdateTableAdapter.GetLastRowForSource(PrmCurrencySourceId)[0];
        }

        public DataLayer.TrnKrakenValue.TrnKrakenValueRow GetLastKrakenDataForCurrecyListAndCurrency(int prmCurrencyId, Guid mstCurrencyListGuid)
        {
            return TrnKrakenValueTableAdapter.GetKrakenDataForCurrecyListAndCurrency(prmCurrencyId, mstCurrencyListGuid)[0];
        }


        public DataLayer.TrnAltCoinTraderValue.AltCoinTraderSecurityRow GetLastSecurityRowForAltCoinTrader(Guid mstCurrencyListGuid, int PrmSellBuyId)
        {
            return AltCoinTraderSecurityTableAdapter.GetRow(mstCurrencyListGuid, PrmSellBuyId)[0];
        }

        public DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueDataTable GetAllAltCoinTraderRowsForSecurityKey(Guid SecurityKey, int PrmSellBuyId)
        {
            return TrnAltCoinTraderValueTableAdapter.GetRowsForSecurityKey(SecurityKey, PrmSellBuyId);
        }

        public enum PrmSellBuy
        {
            Sell = 1,
            Buy = 2
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

        public void UpdateCurrencyValue(DataLayer.TrnLunoValue.TrnLunoValueDataTable newTable)
        {
            TrnLunoValueTableAdapter.Update(newTable);
        }

        public void UpdateCurrencyValue(DataLayer.TrnKrakenValue.TrnKrakenValueRow newRow)
        {
            TrnKrakenValueTableAdapter.Update(newRow);
        }

        public void UpdateCurrencyValue(DataLayer.TrnBitStampValue.TrnBitStampValueRow newRow)
        {
            TrnBitStampValueTableAdapter.Update(newRow);
        }

        public void UpdateCurrencyValue(DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueRow newRow)
        {
            TrnAltCoinTraderValueTableAdapter.Update(newRow);
        }

        public void UpdateSourceRow(DataLayer.TrnSourceUpdate.TrnSourceUpdateDataTable newRow)
        {
            TrnSourceUpdateTableAdapter.Update(newRow);
        }

        public void UpdateSourceRow(DataLayer.TrnSourceUpdate.TrnSourceUpdateRow newRow)
        {
            TrnSourceUpdateTableAdapter.Update(newRow);
        }

    }
}
