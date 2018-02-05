using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace CurrencyServices
{
    public partial class GetAltcointraderData : ServiceBase
    {

        public GetAltcointraderData()
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
                aTimer.Interval = 30000;
                aTimer.Enabled = true;

            }
            catch (Exception ex)
            {

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
                sourceRow.PrmCurrencySourceId = (int)BusLayer.Handler.CurrencySource.Sources.Altcointrader;
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
                    sourceRow.ErrorStackTrace = ex.InnerException.StackTrace;

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
            // why wait
            OnStart(new string[0]);

        }

        private void GetData()
        {
            //the name of the current page we are attempting to get data from
            string currentCurrencyListItem = string.Empty;

            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Only a test!");


                string websiteUrl = "https://www.altcointrader.co.za/|https://www.altcointrader.co.za/xrp|https://www.altcointrader.co.za/eth|https://www.altcointrader.co.za/dash|https://www.altcointrader.co.za/ltc|https://www.altcointrader.co.za/zec";

                string[] websiteUrls = websiteUrl.Split('|');

                foreach (string url in websiteUrls)
                {
                    Guid securityKey = Guid.NewGuid();

                    new WebClient();
                    webClient.Headers.Add("user-agent", "Only a test!");
                    string js5on = webClient.DownloadString(url);

                    currentCurrencyListItem = url.Replace("https://www.altcointrader.co.za/", "").ToUpper();

                    if (currentCurrencyListItem == string.Empty)
                    {
                        currentCurrencyListItem = "BTC";
                    }

                    BusLayer.Currency currencyHelper = new BusLayer.Currency();

                    DataLayer.MstCurrencyList.MstCurrencyListRow currencyListRow = currencyHelper.GetCurrentListRowForCode(currentCurrencyListItem);

                    // This expression looks for a sub-string in the form of
                    // "<p>...<a...>...ItemText...</a> and returns the item text.
                    //string expression = @"<p>[^<]*<a[^>]*>(?<item>[^<]*)</a>";

                    //string ex = @"<div class='trade-orders orange-border'></div>";
                    string className = "trade-table";
                    string globalPattern = String.Format("<table[^>]*?class=([\"'])[^>]*{0}[^>]*\\1[^>]*>(.*?)</table>", className);

                    // This executes the regular expression and returns all 
                    // matches found.
                    MatchCollection matches =
                        Regex.Matches(
                            js5on,
                            globalPattern,
                            RegexOptions.Singleline |
                                RegexOptions.Multiline |
                                RegexOptions.IgnoreCase
                    );



                    //go through the first 2 tables only
                    int count = 0;

                    while (count < 2)
                    {

                        string currclassName = string.Empty;
                        if (count == 0)
                            currclassName = "orderUdSell";
                        else
                            currclassName = "orderUdBuy";

                        string currglobalPattern = String.Format("<tr[^>]*?class = ([\"'])[^>]*{0}[^>]*\\1[^>]*>(.*?)</tr>", currclassName);
                        string currentTable = matches[count].ToString();

                        MatchCollection currMatches =
                       Regex.Matches(
                           currentTable,
                           currglobalPattern,
                           RegexOptions.Singleline |
                               RegexOptions.Multiline |
                               RegexOptions.IgnoreCase
                   );


                        foreach (var obj in currMatches)
                        {
                            string currentItem = obj.ToString();
                            int prmSellBuyId = 0;
                            //replace all the bad characters
                            if (count == 0)
                            {
                                prmSellBuyId = 1;
                                currentItem = currentItem.Replace("\t", "").Replace("\r\n", "").Replace("<tr class = 'orderUdSell'>", "").Replace("</tr>", "");
                            }
                            else
                            {
                                prmSellBuyId = 2;
                                currentItem = currentItem.Replace("\t", "").Replace("\r\n", "").Replace("<tr class = 'orderUdBuy'>", "").Replace("</tr>", "");
                            }
                            //[1] - Price
                            //[3] - Coin Amount
                            //[5] - Total
                            string[] spltRes = currentItem.Split('<');

                            string priceValue = string.Empty;
                            string coinValue = string.Empty;
                            string totalValue = string.Empty;

                            if (count == 0)
                            {
                                priceValue = spltRes[1].Replace("td class = 'orderUdSPr'>", "");
                                coinValue = spltRes[3].Replace("td class = 'orderUdSAm'>", "");
                                totalValue = spltRes[5].Replace("td>", "");
                            }
                            else
                            {
                                priceValue = spltRes[1].Replace("td class = 'orderUdBPr'>", "");
                                coinValue = spltRes[3].Replace("td class = 'orderUdBAm'>", "");
                                totalValue = spltRes[5].Replace("td>", "");
                            }

                            System.IO.File.WriteAllText(@"c:\temp\values.txt", "Coin: " + coinValue + " Converted Coin:" + decimal.Parse(coinValue, new CultureInfo("en-US")));

                            //create the new row
                            DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueDataTable newTbl = new DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueDataTable();
                            DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueRow newRow = newTbl.NewTrnAltCoinTraderValueRow();

                            newRow.TrnAltCoinTraderValueGuid = Guid.NewGuid();
                            newRow.SecurityKey = securityKey;
                            newRow.MstCurrencyListGuid = currencyListRow.MstCurrencyListGuid;
                            newRow.CoinValue = decimal.Parse(coinValue, new CultureInfo("en-US"));
                            newRow.PriceValue = decimal.Parse(priceValue, new CultureInfo("en-US"));
                            newRow.TotalValue = decimal.Parse(totalValue, new CultureInfo("en-US"));
                            newRow.CreateDate = DateTime.Now;
                            newRow.PrmCurrencyId = 3;// currencyRow.PrmCurrencyId;
                            newRow.PrmCurrencySourceId = 1;
                            newRow.PrmSellBuyId = prmSellBuyId;

                            newTbl.AddTrnAltCoinTraderValueRow(newRow);
                            currencyHelper.UpdateCurrencyValue(newRow);

                        }



                        count++;
                    }

                }

            }
            catch(Exception ex)
            {
                //throw an exception with the code of the page that we are laoding
                throw new Exception("Unable to get page data for : " + currentCurrencyListItem, ex);
            }

        }

        private class AltCoinTraderDef
        {
            public string Code { get; set; }
            public decimal price { get; set; }
            public decimal sell { get; set; }
            public decimal buy { get; set; }
            public decimal high { get; set; }
            public decimal low { get; set; }
            public decimal Volume { get; set; }
        }

    }
}
