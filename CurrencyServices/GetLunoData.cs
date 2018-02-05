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
using System.Timers;

namespace CurrencyServices
{
    public partial class GetLunoData : ServiceBase
    {
        private int processTimerTick = 30000;
        private System.Timers.Timer processTimer;
        private bool isStopped = false; 
        private static object lockobject = new object();


        public GetLunoData()
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
            // why wait
            processTimerTick = 3000;
            OnStart(new string[0]);
        }

        private void GetData()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");

            string js5on = webClient.DownloadString("https://api.mybitx.com/api/1/tickers");

            //deserialse the object
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(js5on);
            BusLayer.Currency currencyHelper = new BusLayer.Currency();
            //loop through each of the values and save the data
            DataLayer.TrnLunoValue.TrnLunoValueDataTable currencyTable = new DataLayer.TrnLunoValue.TrnLunoValueDataTable();
            foreach (var item in values)
            {

                List<Dictionary<string, string>> itemList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(item.Value.ToString());

                int totalBids = 0;
                int maxTotal = 10;

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

                        decimal thisBid = Convert.ToDecimal(thisItem["bid"]);

                        int currentBid = Convert.ToInt32(thisBid);
                        totalBids += currentBid;

                        //if(totalBids >= maxTotal)
                        //{
                        //stop the counter now
                        //   break;
                        //}
                        //order data only - must take the ["Bid"] element and save the price and volume up to a max of 10 BTC coints

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
                    catch (Exception ex)
                    {

                    }
                }
            }

            //update the table
            currencyHelper.UpdateCurrencyValue(currencyTable);

        }

    }
}
