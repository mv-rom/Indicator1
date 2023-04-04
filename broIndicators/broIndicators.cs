using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TradingPlatform.BusinessLayer;
using TradingPlatform.BusinessLayer.Licence;

namespace broIndicators
{
	public class broDepthOfBidAsk : Indicator
    {
        #region Parameters
        private const PriceType BID_PRICE_TYPE = PriceType.Open;
        private const PriceType ASK_PRICE_TYPE = PriceType.Close;

        [InputParameter("Number of levels", 10, 1, 99999, 1, 0)]
        public int Level2Count = 10;

        [InputParameter("Highlight absorption bars", 40)]
        internal bool HighlightAbsorptionBars = true;

        [InputParameter("Absorption bar color", 15)]
        internal Color AbsorptionBarColor = Color.Yellow;
        #endregion Parameters

        public override string ShortName => $"{this.Name} ({this.Level2Count}; {this.Format(this.CurrenctDataType)})";

        internal DataType CurrenctDataType
        {
            get => this.currenctDataType;
            private set
            {
                if (this.currenctDataType == value)
                    return;

                this.currenctDataType = value;
                this.RedrawHistoryFromCache();
            }
        }
        private DataType currenctDataType;

        private HistoricalDataCustom cumulativeCache;
        private HistoricalDataCustom imbalanceCache;



        public broDepthOfBidAsk()
        {
            this.Name = "bro Depth of Bid/Ask";

            this.AddLineSeries("Asks depth", Color.FromArgb(235, 96, 47), 2, LineStyle.Histogramm); //Color.CadetBlue
            this.AddLineSeries("Bids depth", Color.FromArgb(55, 219, 186), 2, LineStyle.Histogramm);
            this.AddLineLevel(0, "0'Line", Color.Gray, 1, LineStyle.Solid);

            this.SeparateWindow = true;
            //this.FileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "slippage.csv");
        }

        public override IList<SettingItem> Settings
        {
            get
            {
                var settings = base.Settings;

                settings.Add(new SettingItemSelectorLocalized("DataType", new SelectItem("", (int)this.CurrenctDataType), new List<SelectItem>()
                {
                    new SelectItem(this.Format(DataType.Cumulative), (int)DataType.Cumulative),
                    new SelectItem(this.Format(DataType.ImbalancePerc), (int)DataType.ImbalancePerc),
                })
                { Text = loc._("Data type"), SeparatorGroup = settings.FirstOrDefault()?.SeparatorGroup ?? new SettingItemSeparatorGroup() });

                return settings;
            }
            set
            {
                base.Settings = value;

                if (value.GetItemByName("DataType")?.Value is SelectItem si)
                    this.CurrenctDataType = (DataType)si.Value;
            }
        }

        protected override void OnInit()
        {
            if (this.Symbol != null)
                this.Symbol.NewLevel2 += this.Symbol_NewLevel2;

            this.cumulativeCache = new HistoricalDataCustom(this);
            this.imbalanceCache = new HistoricalDataCustom(this);
        }
        protected override void OnUpdate(UpdateArgs args)
        {
            if (args.Reason == UpdateReason.HistoricalBar)
                return;

            var dom = this.Symbol.DepthOfMarket.GetDepthOfMarketAggregatedCollections(new GetLevel2ItemsParameters()
            {
                AggregateMethod = AggregateMethod.ByPriceLVL,
                LevelsCount = this.Level2Count,
                //CustomTickSize = this.CurrentChart.TickSize,
                CalculateCumulative = true
            });

            // get ask cumulative
            Level2Item askItem = null;
            if (dom.Asks.Length > 0)
            {
                askItem = dom.Asks.Last();
                this.cumulativeCache[ASK_PRICE_TYPE] = askItem.Cumulative;
            }

            // get bid cumulative
            Level2Item bidItem = null;
            if (dom.Bids.Length > 0)
            {
                bidItem = dom.Bids.Last();
                this.cumulativeCache[BID_PRICE_TYPE] = bidItem.Cumulative;
            }

            // calculate imbalance, %
            if (askItem != null && bidItem != null)
            {
                double ac = askItem.Cumulative;
                double bc = bidItem.Cumulative;
                double ac_diffPercent = 0;
                double bc_diffPercent = 0;
                double diff = 0;
                if (bc > ac) {
                    diff = bc - ac;
                    bc_diffPercent = diff * 100 / bc;
                }
                else { 
                    diff = ac - bc;
                    ac_diffPercent = diff * 100 / ac;
                }

                //double total = askItem.Cumulative + bidItem.Cumulative;
                //double bidImbalance = bidItem.Cumulative * 100 / total;

                //this.imbalanceCache[BID_PRICE_TYPE] = bidImbalance;
                //this.imbalanceCache[ASK_PRICE_TYPE] = 100 - bidImbalance;
                this.imbalanceCache[BID_PRICE_TYPE] = bc_diffPercent;
                this.imbalanceCache[ASK_PRICE_TYPE] = -ac_diffPercent;
            }

            var (bid, ask) = this.GetCachedItems(0);
            this.SetValue(ask, 0, 0);
            this.SetValue(bid, 1, 0);


            //-----------------------------------
/*
            var isNewBar = this.HistoricalData.Period == Period.TICK1 &&
                            this.HistoricalData.Aggregation is HistoryAggregationTick
                ? args.Reason == UpdateReason.NewTick
                : args.Reason == UpdateReason.HistoricalBar || args.Reason == UpdateReason.NewBar;
*/
            int offset = 0;
            var index = this.Count - 1 - offset;
            if (index < 0)
                return;

            var volumeAnalysis = this.HistoricalData[index, SeekOriginHistory.Begin].VolumeAnalysisData;
            if (volumeAnalysis != null) {
                var close = this.Close(offset);
                var open = this.Open(offset);

                var isGrownBar = close > open;
                var isDoji = close == open;
                var isAbsorptionBarAsk = !isDoji && (volumeAnalysis.Total.Delta < 0 && isGrownBar);
                var isAbsorptionBarBid = !isDoji && (volumeAnalysis.Total.Delta > 0 && !isGrownBar);


                //
                // Set markers
                //
                if (this.HighlightAbsorptionBars)
                {
                    if (isAbsorptionBarAsk)
                        this.LinesSeries[0].SetMarker(offset, this.AbsorptionBarColor);
                    else
                        this.LinesSeries[0].RemoveMarker(offset);
                    if (isAbsorptionBarBid)
                        this.LinesSeries[1].SetMarker(offset, this.AbsorptionBarColor);
                    else
                        this.LinesSeries[1].RemoveMarker(offset);
                }
            }
        }


        protected override void OnClear()
        {
            if (this.Symbol != null)
                this.Symbol.NewLevel2 -= this.Symbol_NewLevel2;
        }

        private void RedrawHistoryFromCache()
        {
            if (this.HistoricalData == null)
                return;

            for (int i = 0; i < this.HistoricalData.Count; i++)
            {
                int offset = this.HistoricalData.Count - i - 1;
                var (bid, ask) = this.GetCachedItems(offset);

                this.SetValue(ask, 0, offset);
                this.SetValue(bid, 1, offset);
            }
        }
        private (double bid, double ask) GetCachedItems(int offset)
        {
            if (this.CurrenctDataType == DataType.Cumulative)
                return (this.cumulativeCache[offset][BID_PRICE_TYPE], this.cumulativeCache[offset][ASK_PRICE_TYPE]);
            else
                return (this.imbalanceCache[offset][BID_PRICE_TYPE], this.imbalanceCache[offset][ASK_PRICE_TYPE]);
        }
        private string Format(DataType dataType) => dataType switch
        {
            DataType.Cumulative => loc._("Cumulative"),
            DataType.ImbalancePerc => loc._("Imbalance, %"),

            _ => string.Empty,
        };

        private void Symbol_NewLevel2(Symbol symbol, Level2Quote level2, DOMQuote dom) { }

        internal enum DataType { Cumulative, ImbalancePerc }
    }
}
