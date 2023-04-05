using System;
using System.Drawing;
using System.IO;
//using BarsDataIndicators.Utils;
using TradingPlatform.BusinessLayer;

namespace broIndicators
{
    public class broCumulativeDeltaChannel : Indicator //: CandleDrawIndicator, IVolumeAnalysisIndicator
    {
        public override string ShortName => this.Name;
        // Define 'Period' input parameter and set allowable range (from 1 to 999) 
        [InputParameter("Period of price channel", 0, 1, 999)]
        public int Period = 20;

        public int MinHistoryDepths => this.Period;


        public broCumulativeDeltaChannel()
            : base()
        {
            this.Name = "bro Cumulative delta";
            this.LinesSeries[0].Name = "Cumulative open";
            this.LinesSeries[1].Name = "Cumulative high";
            this.LinesSeries[2].Name = "Cumulative low";
            this.LinesSeries[3].Name = "Cumulative close";

            this.AddLineLevel(0d, "Zero line", Color.Gray, 1, LineStyle.DashDot);

            // Define two lines (on main window) with particular parameters 
           
 //           this.AddLineSeries("Highest", Color.Red, 2, LineStyle.Solid);
 //           this.AddLineSeries("Lowest", Color.CadetBlue, 2, LineStyle.Solid);

            this.SeparateWindow = true;
        }
/*

        protected override void OnInit()
        {
            this.IsLoading = true;
            base.OnInit();
        }
*/
        protected override void OnUpdate(UpdateArgs args)
        {
            if (args.Reason == UpdateReason.HistoricalBar)
                return;

            this.CalculateIndicatorByOffset(offset: 0);
        }

        private void CalculateIndicatorByOffset(int offset)
        {
            // Skip some period for correct calculation.
//            if (this.Count < this.MinHistoryDepths)
//                return;

            int prevOffset = offset + 1;

            if (this.HistoricalData.Count <= prevOffset)
                return;

            var currentItem = this.HistoricalData[offset].VolumeAnalysisData;
            var prevItem = this.HistoricalData[prevOffset].VolumeAnalysisData;

            if (currentItem != null && prevItem != null)
            {
                double prevCumulativeDelta = !currentItem.Total.CumulativeDelta.Equals(currentItem.Total.Delta)
                    ? prevItem.Total.CumulativeDelta
                    : 0d;

                double high = currentItem.Total.MaxDelta != double.MinValue
                    ? prevCumulativeDelta + Math.Abs(currentItem.Total.MaxDelta)
                    : Math.Max(currentItem.Total.CumulativeDelta, prevCumulativeDelta);

                double low = currentItem.Total.MinDelta != double.MaxValue
                    ? prevCumulativeDelta - Math.Abs(currentItem.Total.MinDelta)
                    : Math.Min(currentItem.Total.CumulativeDelta, prevCumulativeDelta);

//                this.SetValues(prevCumulativeDelta, high, low, currentItem.Total.CumulativeDelta, offset);

/*
                //-------------------------------------------------------
                int maxValueOffset = offset;
                for (int i = 0; i < this.Period; i++)
                {
                    if (this.GetPrice(PriceType.High, maxValueOffset) < this.GetPrice(PriceType.High, offset + i))
                        maxValueOffset = offset + i;
                }
                double highestPrice = this.GetPrice(PriceType.High, maxValueOffset);


                int minValueOffset = offset;
                for (int i = 0; i < this.Period; i++)
                {
                    if (this.GetPrice(PriceType.Low, minValueOffset) > this.GetPrice(PriceType.Low, offset + i))
                        minValueOffset = offset + i;
                }
                double lowestPrice = this.GetPrice(PriceType.Low, minValueOffset);

                // Set highestPrice to 'Highest' line buffer and lowestPrice to 'Lowest' line buffer.
                this.SetValue(highestPrice, 0);
                this.SetValue(lowestPrice, 1);
*/
            }
/*            else
                this.SetHole(offset);
            }

            #region IVolumeAnalysisIndicator

            bool IVolumeAnalysisIndicator.IsRequirePriceLevelsCalculation => false;
            void IVolumeAnalysisIndicator.VolumeAnalysisData_Loaded()
            {
                for (int i = 0; i < this.Count; i++)
                    this.CalculateIndicatorByOffset(i);

                this.IsLoading = false;
            }

            #endregion IVolumeAnalysisIndicator
*/
        }
    }
}
