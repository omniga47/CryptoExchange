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
using System.Globalization;

namespace CurrencyServices
{
    public partial class GetKrakenData : ServiceBase
    {

        public GetKrakenData()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                System.Timers.Timer aTimer;
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 90000;
                aTimer.Enabled = true;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        BusLayer.Currency localCurrencyHelper = new BusLayer.Currency();
        Guid currentSourceGuid = Guid.Empty;
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                currentSourceGuid = Guid.NewGuid();
                //log that we are about to start this source import
                DataLayer.TrnSourceUpdate.TrnSourceUpdateDataTable sourceTable = new DataLayer.TrnSourceUpdate.TrnSourceUpdateDataTable();
                DataLayer.TrnSourceUpdate.TrnSourceUpdateRow sourceRow = sourceTable.NewTrnSourceUpdateRow();

                sourceRow.TrnSourceUpdateGuid = currentSourceGuid;
                sourceRow.PrmCurrencySourceId = (int)BusLayer.Handler.CurrencySource.Sources.Kraken;
                sourceRow.CreateDate = DateTime.Now;

                sourceTable.AddTrnSourceUpdateRow(sourceRow);

                localCurrencyHelper.UpdateSourceRow(sourceTable);
                
                try
                {
                    //your code
                    GetData();
                    
                    sourceRow.Completed = true;
                    sourceRow.CreateDate = DateTime.Now;

                    localCurrencyHelper.UpdateSourceRow(sourceRow);

                }
                catch (Exception ex)
                {
                    sourceRow.Completed = true;
                    sourceRow.CreateDate = DateTime.Now;
                    sourceRow.Failed = true;
                    sourceRow.ErrorMessage = ex.Message;
                    sourceRow.ErrorStackTrace = ex.StackTrace;

                    localCurrencyHelper.UpdateSourceRow(sourceRow);
                }
            }
            catch (Exception ex)
            {
                string fuck = "me";
            }
        }

        protected override void OnStop()
        {

        }


        public void DebugMe()
        {
            OnStart(new string[0]);
        }

        private void GetData()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");
            BusLayer.Currency currencyHelper = new BusLayer.Currency();

            KrakenClient.KrakenClient client = new KrakenClient.KrakenClient();

            DataLayer.MstCurrencyList.MstCurrencyListDataTable currentLisTbl = currencyHelper.GetCurrentListRows();
            
            DataLayer.PrmCurrency.PrmCurrencyDataTable currencyTable = currencyHelper.GetCurrencyRows();

            try
            {

                foreach (DataLayer.MstCurrencyList.MstCurrencyListRow listRow in currentLisTbl)
                {

                    string currencyCode = listRow.Code;

                    if (currencyCode == "BTC")
                        currencyCode = "XBT";

                    foreach (DataLayer.PrmCurrency.PrmCurrencyRow currencyRow in currencyTable)
                    {
                        try
                        {
                            string newcurrencyCode = currencyCode + currencyRow.Currency;

                            Jayrock.Json.JsonObject js5on = client.GetTicker(new List<string> { newcurrencyCode });

                            string innerRes = string.Empty;
                            if (newcurrencyCode == "XBTUSD")
                                innerRes = "XXBTZUSD";
                            else if (newcurrencyCode == "XBTEUR")
                                innerRes = "XXBTZEUR";
                            else if (newcurrencyCode == "ETHUSD")
                                innerRes = "XETHZUSD";
                            else if (newcurrencyCode == "ETHEUR")
                                innerRes = "XETHZEUR";
                            else if (newcurrencyCode == "XRPUSD")
                                innerRes = "XXRPZUSD";
                            else if (newcurrencyCode == "XRPEUR")
                                innerRes = "XXRPZEUR";
                            else if (newcurrencyCode == "LTCUSD")
                                innerRes = "XLTCZUSD";
                            else if (newcurrencyCode == "LTCEUR")
                                innerRes = "XLTCZEUR";
                            else if (newcurrencyCode == "ZECUSD")
                                innerRes = "XZECZUSD";
                            else if (newcurrencyCode == "ZECEUR")
                                innerRes = "XZECZEUR";
                            else if (newcurrencyCode == "DASHUSD")
                                innerRes = "DASHUSD";
                            else if (newcurrencyCode == "DASHEUR")
                                innerRes = "DASHEUR";



                            if (innerRes != string.Empty)
                            {

                                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(js5on["result"].ToString());

                                Dictionary<string, object> Innervalues = JsonConvert.DeserializeObject<Dictionary<string, object>>(values[innerRes].ToString());

                                //create the new row
                                DataLayer.TrnKrakenValue.TrnKrakenValueDataTable newTbl = new DataLayer.TrnKrakenValue.TrnKrakenValueDataTable();
                                DataLayer.TrnKrakenValue.TrnKrakenValueRow newRow = newTbl.NewTrnKrakenValueRow();

                                newRow.TrnKrakenValueGuid = Guid.NewGuid();
                                newRow.MstCurrencyListGuid = listRow.MstCurrencyListGuid;
                                newRow.Ask = decimal.Parse(JsonConvert.DeserializeObject<List<string>>(Innervalues["a"].ToString())[0], new CultureInfo("en-US"));
                                newRow.Bid = decimal.Parse(JsonConvert.DeserializeObject<List<string>>(Innervalues["b"].ToString())[0], new CultureInfo("en-US"));
                                newRow.CreateDate = DateTime.Now;
                                newRow.PrmCurrencyId = currencyRow.PrmCurrencyId;
                                newRow.PrmCurrencySourceId = 3;


                                newTbl.AddTrnKrakenValueRow(newRow);
                                currencyHelper.UpdateCurrencyValue(newRow);

                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {

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
            
        }

    }
}
