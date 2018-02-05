using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using KrakenClient;
using System.Timers;

namespace CurrencyServices
{
    public partial class GetBitStampData : ServiceBase
    {
        private int processTimerTick = 30000; 
        private System.Timers.Timer processTimer;
        private bool isStopped = false;
        private static object lockobject = new object();

        public GetBitStampData()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //Prosol.Common.Logging.Logger.Log("OnStart Begin",Prosol.Common.Enumerations.LogType.Debug);

                // every 30 seconds
                this.processTimer = new System.Timers.Timer(processTimerTick);

                processTimer.Elapsed += new ElapsedEventHandler(processTimer_Tick);
                processTimer.Enabled = true;

                isStopped = false;

                //Prosol.Common.Logging.Logger.Log("OnStart End", Prosol.Common.Enumerations.LogType.Debug);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected override void OnStop()
        {

            processTimer.Enabled = false;

            lock (lockobject)
                isStopped = true;
        }

        private void processTimer_Tick(object source, ElapsedEventArgs e)
        {

            lock (lockobject)
            {
                //Prosol.Common.Logging.Logger.Log("Calling RunProcess", Prosol.Common.Enumerations.LogType.Debug);
                // do code here
                try
                {
                    GetData();
                }
                catch (Exception ex)
                {

                }

                if (!isStopped)
                {
                    processTimer.Enabled = true;
                }
            }
        }

        public void DebugMe()
        {
            //   processTimerTick = 3000;
            //  OnStart(new string[0]);
            GetData();
        }

        private void GetData()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");
            BusLayer.Currency currencyHelper = new BusLayer.Currency();

            DataLayer.MstCurrencyList.MstCurrencyListDataTable currentLisTbl = currencyHelper.GetCurrentListRows();

            DataLayer.PrmCurrency.PrmCurrencyDataTable currencyTable = currencyHelper.GetCurrencyRows();


            foreach (DataLayer.MstCurrencyList.MstCurrencyListRow listRow in currentLisTbl)
            {
                string currencyCode = listRow.Code;

                foreach (DataLayer.PrmCurrency.PrmCurrencyRow currencyRow in currencyTable)
                {

                    try
                    {
                        string newcurrencyCode = currencyCode + currencyRow.Currency;

                        string js5on = webClient.DownloadString("https://www.bitstamp.net/api/v2/ticker/" + newcurrencyCode.ToLower());

                        Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(js5on);

                        //create the new row
                        DataLayer.TrnBitStampValue.TrnBitStampValueDataTable newTbl = new DataLayer.TrnBitStampValue.TrnBitStampValueDataTable();
                        DataLayer.TrnBitStampValue.TrnBitStampValueRow newRow = newTbl.NewTrnBitStampValueRow();

                        newRow.TrnBitStampValueGuid = Guid.NewGuid();
                        newRow.MstCurrencyListGuid = listRow.MstCurrencyListGuid;
                        newRow.ask = decimal.Parse(values["ask"]);
                        newRow.bid = decimal.Parse(values["bid"]);
                        newRow.last_trade = decimal.Parse(values["last"]);
                        newRow.timestamp = values["timestamp"];
                        newRow.CreateDate = DateTime.Now;
                        newRow.PrmCurrencyId = currencyRow.PrmCurrencyId;
                        newRow.PrmCurrencySourceId = 3;


                        newTbl.AddTrnBitStampValueRow(newRow);
                        currencyHelper.UpdateCurrencyValue(newRow);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }


            //kraken is ask
            /*
            XBTEUR
            XBTUSD

            ETHEUR
            ETHUSD

            XRPEUR
            XRPUSD


            LTC
            ZEC
            DASH

            <pair_name> = pair name
    a = ask array(<price>, <whole lot volume>, <lot volume>),
    b = bid array(<price>, <whole lot volume>, <lot volume>),
    c = last trade closed array(<price>, <lot volume>),
    v = volume array(<today>, <last 24 hours>),
    p = volume weighted average price array(<today>, <last 24 hours>),
    t = number of trades array(<today>, <last 24 hours>),
    l = low array(<today>, <last 24 hours>),
    h = high array(<today>, <last 24 hours>),
    o = today's opening price






            bitstamp
            https://www.bitstamp.net/api/

            btcusd, btceur,
            ethusd, etheur,
             xrpusd, xrpeur


            {"high": "1.74000", 
            "last": "1.68754", 
            "timestamp": "1516295176" ,  to use
            "bid": "1.68001",  to use
            "vwap": "1.33150", 
            "volume": "164572084.34820374", 
            "low": "0.94344", 
                    "ask": "1.68749",  to use
            "open": "1.29849"}

            */

            //deserialse the object


            //loop through each of the values and save the data
            //DataLayer.TrnLunoValue.TrnLunoValueDataTable currencyTable = new DataLayer.TrnLunoValue.TrnLunoValueDataTable();
            /*foreach (var item in values)
            {

                List<Dictionary<string, string>> itemList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(item.Value.ToString());


                foreach (Dictionary<string, string> thisItem in itemList)
                {
                    //get the current list item for the code
                    //if the code exists it means we want to add the data, else we just continue to the next
                    try
                    {
                        string searchCode = thisItem["pair"];
                        if (searchCode == "XBTZAR")
                            searchCode = "BTC";


                        DataLayer.MstCurrencyList.MstCurrencyListRow currentListRow = currencyHelper.GetCurrentListRowForCode(searchCode);

                        //add a new row and save the data
                        DataLayer.TrnLunoValue.TrnLunoValueRow newRow = currencyTable.NewTrnLunoValueRow();

                        newRow.TrnLunoValueGuid = Guid.NewGuid();
                        newRow.MstCurrencyListGuid = currentListRow.MstCurrencyListGuid;
                        newRow.timestamp = thisItem["timestamp"];
                        newRow.bid = decimal.Parse(thisItem["bid"]);
                        newRow.ask = decimal.Parse(thisItem["ask"]);
                        newRow.last_trade = decimal.Parse(thisItem["last_trade"]);
                        newRow.rolling_24_hour_volume = decimal.Parse(thisItem["rolling_24_hour_volume"]);
                        newRow.pair = thisItem["pair"];
                        newRow.CreateDate = DateTime.Now;
                        newRow.PrmCurrencySourceId = (int)BusLayer.Handler.CurrencySource.Sources.Luno;

                        currencyTable.AddTrnLunoValueRow(newRow);
                    }
                    catch
                    {

                    }
                }
            }

            //update the table
            currencyHelper.UpdateCurrencyValue(currencyTable);
            */
        }

    }
}
