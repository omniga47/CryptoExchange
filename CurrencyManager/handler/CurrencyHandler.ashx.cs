using BusLayer.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CurrencyManager.handler
{
    /// <summary>
    /// Summary description for CurrencyHandler
    /// </summary>
    public class CurrencyHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        JavaScriptSerializer serializer = new JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;

            try
            {
                switch (context.Request.Params.Get("function"))
                {
                    case "GetData":
                        {
                            result = GetData(context.Session, context.Request.Params.Get("param"));
                            break;
                        }
                    case "GetAltCoinTraderData":
                        {
                            result = GetAltCoinTraderData(context.Session, context.Request.Params.Get("param"));
                            break;
                        }
                    case "GetLastUpdatedAltCoinTraderPage":
                        {
                            result = GetLastUpdatedAltCoinTraderPage(context.Session);
                            break;
                        }
                    default:
                        break;

                }
            }
            catch (Exception exc)
            {

                result = serializer.Serialize(new HandlerResponse(-1, "An error has occurred, please contact your administrator."));
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(result);
        }


        public string GetData(System.Web.SessionState.HttpSessionState session, string param)
        {

            Dictionary<string, string> input = serializer.Deserialize<Dictionary<string, string>>(param);
            string aaron = input["SSSSSSS"];

            string returnString = aaron;

            return serializer.Serialize(new HandlerResponse(0, "Success", returnString));

        }

        public string GetLastUpdatedAltCoinTraderPage(System.Web.SessionState.HttpSessionState session)
        {

            //go and get the data for the required sources.
            BusLayer.Currency currencyHelper = new BusLayer.Currency();
            DataLayer.TrnSourceUpdate.TrnSourceUpdateRow krakenSourceRow = currencyHelper.GetLastTrnSourceUpdateRowForSource((int)BusLayer.Handler.CurrencySource.Sources.Kraken);
            DataLayer.TrnSourceUpdate.TrnSourceUpdateRow altcointraderSourceRow = currencyHelper.GetLastTrnSourceUpdateRowForSource((int)BusLayer.Handler.CurrencySource.Sources.Altcointrader);

            string returnString = krakenSourceRow.CreateDate.ToString("HH:mm:ss dd/MM/yyyy") + "|" + altcointraderSourceRow.CreateDate.ToString("HH:mm:ss dd/MM/yyyy");

            return serializer.Serialize(new HandlerResponse(0, "Success", returnString));

        }


        public string GetBitCoin30secData(System.Web.SessionState.HttpSessionState session)
        {
            //Get list of saved data
            BusLayer.Currency currencyHelper = new BusLayer.Currency();
            BusLayer.BitCoin bitcoinHelper = new BusLayer.BitCoin();
            List<BusLayer.BitCoin.BitCoin30LiveView> bitcoinData = new List<BusLayer.BitCoin.BitCoin30LiveView>();
            bitcoinData = bitcoinHelper.Get30SecData("btc");


            return serializer.Serialize(new HandlerResponse(0, "Success", bitcoinData));

        }

        public string GetAltCoinTraderData(System.Web.SessionState.HttpSessionState session, string param)
        {
            Dictionary<string, string> input = serializer.Deserialize<Dictionary<string, string>>(param);

            //check if we have a currency value passed through, else get the current value
            string usdZarRate = input["USDRate"];
            string altCoinRate = input["AltCoinRate"];
            string krakenFeeRate = input["KrakenRate"];

            decimal usdRate = 0;

            if (usdZarRate == string.Empty)
            {

            }
            else
                usdRate = decimal.Parse(usdZarRate.Replace('.', ','));

            decimal altfeeRate = 0;

            if (altCoinRate == string.Empty)
            {

            }
            else
                altfeeRate = decimal.Parse(altCoinRate.Replace('.', ','));

            decimal krakfeeRate = 0;

            if (krakenFeeRate == string.Empty)
            {

            }
            else
                krakfeeRate = decimal.Parse(krakenFeeRate.Replace('.', ','));

            //Get list of saved data
            BusLayer.Currency currencyHelper = new BusLayer.Currency();
            BusLayer.Calculations calcHelper = new BusLayer.Calculations();
            List<BusLayer.Calculations.AltCoinTraderDisplayData> altCoinData = new List<BusLayer.Calculations.AltCoinTraderDisplayData>();
            altCoinData = calcHelper.GetAltCoinTraderData(usdRate, altfeeRate, krakfeeRate);


            return serializer.Serialize(new HandlerResponse(0, "Success", altCoinData));

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}