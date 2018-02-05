using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    public class Calculations
    {

        public Calculations()
        {

        }

        public class AltCoinTraderDisplayData
        {
            public string DT_RowId { get; set; }
            public string CryptoCoin { get; set; }
            public string AskPrice { get; set; }
            public string Less25k { get; set; }
            public string OrderVolumeLess25k { get; set; }
            public string OrderCostLess25k { get; set; }
            public string MrgLess25k { get; set; }
            public string Btw25k50k { get; set; }
            public string OrderVolumeBtw25k50k { get; set; }
            public string OrderCostBtw25k50k { get; set; }
            public string MrgBtw25k50k { get; set; }
            public string Btw50k100k { get; set; }
            public string OrderVolumeBtw50k100k { get; set; }
            public string OrderCostBtw50k100k { get; set; }
            public string MrgBtw50k100k { get; set; }
            public string Btw100k150k { get; set; }
            public string OrderVolumeBtw100k150k { get; set; }
            public string OrderCostBtw100k150k { get; set; }
            public string MrgBtw100k150k { get; set; }
            public string Btw150k200k { get; set; }
            public string OrderVolumeBtw150k200k { get; set; }
            public string OrderCostBtw150k200k { get; set; }
            public string MrgBtw150k200k { get; set; }
            public string Grt200k { get; set; }
            public string OrderVolumeGrt200k { get; set; }
            public string OrderCostGrt200k { get; set; }
            public string MrgGrt200k { get; set; }
        }

        public class FixedWithdrawalFees
        {
            private decimal xbt;
            public decimal XBT
            {
                get
                {
                    if (xbt == 0)
                        xbt = 0.001m;

                    return xbt;
                }
                set { xbt = value; }
            }

            private decimal eth;
            public decimal ETH
            {
                get
                {
                    if (eth == 0)
                        eth = 0.005m;

                    return eth;
                }
                set { eth = value; }
            }

            private decimal xrp;
            public decimal XRP
            {
                get
                {
                    if (xrp == 0)
                        xrp = 0.02m;

                    return xrp;
                }
                set { xrp = value; }
            }

            private decimal ltc;
            public decimal LTC
            {
                get
                {
                    if (ltc == 0)
                        ltc = 0.01m;

                    return ltc;
                }
                set { ltc = value; }
            }


            private decimal zec;
            public decimal ZEC
            {
                get
                {
                    if (zec == 0)
                        zec = 0.0001m;

                    return zec;
                }
                set { zec = value; }
            }


            private decimal dash;
            public decimal DASH
            {
                get
                {
                    if (dash == 0)
                        dash = 0.005m;

                    return dash;
                }
                set { dash = value; }
            }
        }

        public List<AltCoinTraderDisplayData> GetAltCoinTraderData(decimal rateValue, decimal altCoinRate, decimal krakenRate)
        {

            try
            {
                //get the currency list for each item

                BusLayer.Currency currencyHelper = new Currency();
                DataLayer.MstCurrencyList.MstCurrencyListDataTable currListTbl = currencyHelper.GetCurrentListRows();

                //create a new table to store the data
                DataLayer.TrnAltCoinTraderValue.AltCoinTraderDisplayDataTable AltCoinTable = new DataLayer.TrnAltCoinTraderValue.AltCoinTraderDisplayDataTable();

                //foreach of the currency list items get the relevent current price
                foreach (DataLayer.MstCurrencyList.MstCurrencyListRow currListRow in currListTbl)
                {
                    try
                    {
                        //create a new row
                        DataLayer.TrnAltCoinTraderValue.AltCoinTraderDisplayRow newAltCoinTraderRow = AltCoinTable.NewAltCoinTraderDisplayRow();

                        newAltCoinTraderRow.DT_RowId = currListRow.MstCurrencyListGuid.ToString();
                        newAltCoinTraderRow.CryptoCoin = currListRow.Code.ToUpper();
                        newAltCoinTraderRow.SortOrder = currListRow.SortOrder;

                        AltCoinTable.AddAltCoinTraderDisplayRow(newAltCoinTraderRow);
                    }
                    catch (Exception ex)
                    {
                        //unable to get current list item
                    }
                }


                //values used for the coins fixed withdrawl fees
                FixedWithdrawalFees fwf = new FixedWithdrawalFees();

                //once we have added all the rows that we want, we need to go and get the ASk PRICE value from Kraken for that value
                //loop through each of the currency list items and get the their value based on the currency [USD]
                foreach (DataLayer.TrnAltCoinTraderValue.AltCoinTraderDisplayRow altCoinRow in AltCoinTable)
                {
                    try
                    {
                        DataLayer.TrnKrakenValue.TrnKrakenValueRow valueRow =
                            currencyHelper.GetLastKrakenDataForCurrecyListAndCurrency(1, Guid.Parse(altCoinRow.DT_RowId));

                        altCoinRow.AskPrice = valueRow.Ask.ToString();

                        //go and get the altcointrader data from the last screen scape, for this currency list item
                        DataLayer.TrnAltCoinTraderValue.AltCoinTraderSecurityRow securityRow = currencyHelper.GetLastSecurityRowForAltCoinTrader(Guid.Parse(altCoinRow.DT_RowId), (int)BusLayer.Currency.PrmSellBuy.Buy);

                        decimal feeWithRate = 0;

                        switch (altCoinRow.CryptoCoin)
                        {
                            case "DASH":
                                feeWithRate = fwf.DASH;
                                break;
                            case "ETH":
                                feeWithRate = fwf.ETH;
                                break;
                            case "LTC":
                                feeWithRate = fwf.LTC;
                                break;
                            case "BTC":
                                feeWithRate = fwf.XBT;
                                break;
                            case "XRP":
                                feeWithRate = fwf.XRP;
                                break;
                            case "ZEC":
                                feeWithRate = fwf.XRP;
                                break;
                            default:
                                break;
                        }

                        populateCalcValues(altCoinRow, securityRow, rateValue, altCoinRate, krakenRate, feeWithRate);

                        altCoinRow.AskPrice = Math.Round(valueRow.Ask, 2).ToString().Replace(",", ".");
                    }
                    catch (Exception ex)
                    {
                        //unable to get ask price from kraken data for --
                    }
                }


                if (AltCoinTable.Rows.Count > 0)
                {

                    return AltCoinTable.AsEnumerable().Select(item => new AltCoinTraderDisplayData
                    {
                        DT_RowId = item["DT_RowId"].ToString(),
                        CryptoCoin = item["CryptoCoin"].ToString(),
                        AskPrice = item["AskPrice"].ToString(),
                        Less25k = item["Less25k"].ToString(),
                        OrderVolumeLess25k = item["OrderVolumeLess25k"].ToString(),
                        OrderCostLess25k = item["OrderCostLess25k"].ToString(),
                        MrgLess25k = item["MrgLess25k"].ToString(),
                        Btw25k50k = item["Btw25k50k"].ToString(),
                        OrderVolumeBtw25k50k = item["OrderVolumeBtw25k50k"].ToString(),
                        OrderCostBtw25k50k = item["OrderCostBtw25k50k"].ToString(),
                        MrgBtw25k50k = item["MrgBtw25k50k"].ToString(),
                        Btw50k100k = item["Btw50k100k"].ToString(),
                        OrderVolumeBtw50k100k = item["OrderVolumeBtw50k100k"].ToString(),
                        OrderCostBtw50k100k = item["OrderCostBtw50k100k"].ToString(),
                        MrgBtw50k100k = item["MrgBtw50k100k"].ToString(),
                        Btw100k150k = item["Btw100k150k"].ToString(),
                        OrderVolumeBtw100k150k = item["OrderVolumeBtw100k150k"].ToString(),
                        OrderCostBtw100k150k = item["OrderCostBtw100k150k"].ToString(),
                        MrgBtw100k150k = item["MrgBtw100k150k"].ToString(),
                        Btw150k200k = item["Btw150k200k"].ToString(),
                        OrderVolumeBtw150k200k = item["OrderVolumeBtw150k200k"].ToString(),
                        OrderCostBtw150k200k = item["OrderCostBtw150k200k"].ToString(),
                        MrgBtw150k200k = item["MrgBtw150k200k"].ToString(),
                        Grt200k = item["Grt200k"].ToString(),
                        OrderVolumeGrt200k = item["OrderVolumeGrt200k"].ToString(),
                        OrderCostGrt200k = item["OrderCostGrt200k"].ToString(),
                        MrgGrt200k = item["MrgGrt200k"].ToString()
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
                throw new Exception("Error in Calculation Handler", exc);
            }

        }


        private class ReturnObject
        {
            public decimal UnitSellPrice { get; set; }
            public decimal OrderVolume { get; set; }
            public decimal OrderCost { get; set; }
            public decimal Margin { get; set; }
        }

        private void populateCalcValues(DataLayer.TrnAltCoinTraderValue.AltCoinTraderDisplayRow returnRow,
            DataLayer.TrnAltCoinTraderValue.AltCoinTraderSecurityRow securityRow, decimal rateValue, decimal altCoinRate, decimal krakenRate, decimal feeWithRate)
        {
            //go and get all the data for this security item
            BusLayer.Currency currencyHelper = new Currency();
            DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueDataTable allData = currencyHelper.GetAllAltCoinTraderRowsForSecurityKey(securityRow.SecurityKey, (int)BusLayer.Currency.PrmSellBuy.Buy);

            //create a dictionary for each of the required return types
            Dictionary<string, ReturnObject> returnValues = new Dictionary<string, ReturnObject>();

            ReturnObject emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;

            //add the initial set
            returnValues.Add("Less25k", emptyObject);
            emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;
            returnValues.Add("Btw25k50k", emptyObject);
            emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;
            returnValues.Add("Btw50k100k", emptyObject);
            emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;
            returnValues.Add("Btw100k150k", emptyObject);
            emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;
            returnValues.Add("Btw150k200k", emptyObject);
            emptyObject = new ReturnObject();
            emptyObject.UnitSellPrice = 0;
            emptyObject.OrderVolume = 0;
            emptyObject.Margin = 0;
            emptyObject.OrderCost = 0;
            returnValues.Add("Grt200k", emptyObject);

            decimal currentTotalValue = 0;
            decimal currentTotalVolume = 0;
            //loop through each of the rows and add the values to the dictionarys
            foreach (DataLayer.TrnAltCoinTraderValue.TrnAltCoinTraderValueRow currentScrapeRow in allData)
            {
                //assign the current total value of this item
                currentTotalValue += currentScrapeRow.TotalValue;
                currentTotalVolume += currentScrapeRow.CoinValue;

                if (currentTotalValue > 300000)
                    break;

                if (currentTotalValue < 25000)
                {
                    returnValues["Less25k"].OrderVolume = currentTotalVolume;
                    returnValues["Less25k"].OrderCost = currentTotalValue;
                }

                if (currentTotalValue > 25000 && currentTotalValue < 50000)
                {

                    //just add to the existing dict
                    returnValues["Btw25k50k"].OrderVolume = currentTotalVolume;
                    returnValues["Btw25k50k"].OrderCost = currentTotalValue;

                }

                if (currentTotalValue > 50000 && currentTotalValue < 100000)
                {
                    //just add to the existing dict
                    returnValues["Btw50k100k"].OrderVolume = currentTotalVolume;
                    returnValues["Btw50k100k"].OrderCost = currentTotalValue;

                }

                if (currentTotalValue > 100000 && currentTotalValue < 150000)
                {
                    //just add to the existing dict
                    returnValues["Btw100k150k"].OrderVolume = currentTotalVolume;
                    returnValues["Btw100k150k"].OrderCost = currentTotalValue;

                }

                if (currentTotalValue > 150000 && currentTotalValue < 200000)
                {

                    //just add to the existing dict
                    returnValues["Btw150k200k"].OrderVolume = currentTotalVolume;
                    returnValues["Btw150k200k"].OrderCost = currentTotalValue;


                }

                if (currentTotalValue > 200000)
                {
                    //just add to the existing dict
                    returnValues["Grt200k"].OrderVolume = currentTotalVolume;
                    returnValues["Grt200k"].OrderCost = currentTotalValue;
                }
            }


            foreach (KeyValuePair<string, ReturnObject> currObj in returnValues)
            {
                //loop through each of the rows that we know have data for
                //calculate the avg price, which is the total value / total volume

                if (currObj.Value.OrderCost > 0)
                {
                    //kraken calc
                    decimal krakenZARBuyPrice = decimal.Parse(returnRow.AskPrice) * rateValue;
                    //kraken rate fee for ZAR
                    decimal krakenWithdrawlFeeZAR = krakenZARBuyPrice * feeWithRate;
                    //kraken trade fee for ZAR
                    decimal krakenTradeFeeZAR = krakenZARBuyPrice * krakenRate;

                    decimal krakenBuyPricePlusFees = krakenZARBuyPrice + krakenWithdrawlFeeZAR + krakenTradeFeeZAR;
                    
                    returnValues[currObj.Key].UnitSellPrice = returnValues[currObj.Key].OrderCost / returnValues[currObj.Key].OrderVolume;

                    //alt coin calc
                    decimal altCoinSellPrice = returnValues[currObj.Key].UnitSellPrice;
                    //altcoin trade fee for ZAR
                    decimal altCoinTradeFeeZAR = altCoinSellPrice * altCoinRate;

                    decimal altCoinSellPriceLessFees = altCoinSellPrice - altCoinTradeFeeZAR;

                    decimal marginZAR = altCoinSellPriceLessFees - krakenBuyPricePlusFees;

                    decimal marginPercentage = marginZAR / krakenZARBuyPrice;


                    returnValues[currObj.Key].Margin = (marginPercentage) * 100;



                }

            }

            //update the current row with the values
            try
            {
                returnRow.Less25k = Math.Round(returnValues["Less25k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgLess25k = Math.Round(returnValues["Less25k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeLess25k = Math.Round(returnValues["Less25k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostLess25k = Math.Round(returnValues["Less25k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            { }
            try
            {
                returnRow.Btw25k50k = Math.Round(returnValues["Btw25k50k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgBtw25k50k = Math.Round(returnValues["Btw25k50k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeBtw25k50k = Math.Round(returnValues["Btw25k50k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostBtw25k50k = Math.Round(returnValues["Btw25k50k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            {
            }
            try
            {
                returnRow.Btw50k100k = Math.Round(returnValues["Btw50k100k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgBtw50k100k = Math.Round(returnValues["Btw50k100k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeBtw50k100k = Math.Round(returnValues["Btw50k100k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostBtw50k100k = Math.Round(returnValues["Btw50k100k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            {
            }
            try
            {
                returnRow.Btw100k150k = Math.Round(returnValues["Btw100k150k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgBtw100k150k = Math.Round(returnValues["Btw100k150k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeBtw100k150k = Math.Round(returnValues["Btw100k150k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostBtw100k150k = Math.Round(returnValues["Btw100k150k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            {
            }
            try
            {
                returnRow.Btw150k200k = Math.Round(returnValues["Btw150k200k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgBtw150k200k = Math.Round(returnValues["Btw150k200k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeBtw150k200k = Math.Round(returnValues["Btw150k200k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostBtw150k200k = Math.Round(returnValues["Btw150k200k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            {
            }
            try
            {
                returnRow.Grt200k = Math.Round(returnValues["Grt200k"].UnitSellPrice, 2).ToString().Replace(",", ".");
                returnRow.MrgGrt200k = Math.Round(returnValues["Grt200k"].Margin, 2).ToString().Replace(",", ".");
                returnRow.OrderVolumeGrt200k = Math.Round(returnValues["Grt200k"].OrderVolume, 6).ToString().Replace(",", ".");
                returnRow.OrderCostGrt200k = Math.Round(returnValues["Grt200k"].OrderCost, 2).ToString().Replace(",", ".");
            }
            catch (Exception ex)
            {

            }

        }
    }

}
