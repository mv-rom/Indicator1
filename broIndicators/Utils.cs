using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.BusinessLayer.Chart;
using TradingPlatform.BusinessLayer;

namespace broIndicators
{
    internal class Utils
    {
    }

    public abstract class CandleDrawIndicator : Indicator
    {
        // Token: 0x17000028 RID: 40
        // (get) Token: 0x0600004D RID: 77 RVA: 0x00002C5D File Offset: 0x00000E5D
        // (set) Token: 0x0600004E RID: 78 RVA: 0x00002C65 File Offset: 0x00000E65
/*
        protected Color UpBarColor
        {
            get
            {
                return this.겷곧;
            }
            set
            {
                this.겷곧 = value;
                this.겷괈 = new Pen(value, (float)this.겷곇);
            }
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x0600004F RID: 79 RVA: 0x00002C81 File Offset: 0x00000E81
        // (set) Token: 0x06000050 RID: 80 RVA: 0x00002C89 File Offset: 0x00000E89
        protected Color DownBarColor
        {
            get
            {
                return this.겷겾;
            }
            set
            {
                this.겷겾 = value;
                this.겷격 = new Pen(value, (float)this.겷곇);
            }
        }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x06000051 RID: 81 RVA: 0x00002CA5 File Offset: 0x00000EA5
        // (set) Token: 0x06000052 RID: 82 RVA: 0x00002CAD File Offset: 0x00000EAD
        private int BarWickLineWidth
        {
            get
            {
                return this.겷곇;
            }
            set
            {
                if (this.겷곇 == value)
                {
                    return;
                }
                this.겷곇 = value;
                if (this.겷격 != null)
                {
                    this.겷격.Width = (float)value;
                }
                if (this.겷괈 != null)
                {
                    this.겷괈.Width = (float)value;
                }
            }
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x06000053 RID: 83 RVA: 0x00002CEA File Offset: 0x00000EEA
        protected string LoadingMessage
        {
            get
            {
                //F76E122D - 494D - 43A5 - 9451 - 1648058B0963.골() - a1
                //F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곓() - a2
                return loc._(a1, null, a2);
            }
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x06000054 RID: 84 RVA: 0x00002CFC File Offset: 0x00000EFC
        // (set) Token: 0x06000055 RID: 85 RVA: 0x00002D04 File Offset: 0x00000F04
        protected bool IsLoading { get; set; }

        // Token: 0x06000056 RID: 86 RVA: 0x00002D10 File Offset: 0x00000F10
        public CandleDrawIndicator()
        {
            base.AddLineSeries(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.겶(), Color.Transparent, 1, LineStyle.Points);
            base.AddLineSeries(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곚(), Color.Transparent, 1, LineStyle.Points);
            base.AddLineSeries(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곁(), Color.Transparent, 1, LineStyle.Points);
            base.AddLineSeries(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.겯(), Color.Transparent, 1, LineStyle.Points);
            base.IsUpdateTypesSupported = false;
            base.LinesSeries[0].ShowLineMarker = false;
            base.LinesSeries[1].ShowLineMarker = false;
            base.LinesSeries[2].ShowLineMarker = false;
            base.SeparateWindow = true;
            this.UpBarColor = Color.FromArgb(55, 219, 186);
            this.DownBarColor = Color.FromArgb(235, 96, 47);
            this.겷곇 = 1;
            this.겷곎 = new Font(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곣(), 10f, FontStyle.Regular, GraphicsUnit.Point);
            this.겷겦 = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            this.IsLoading = true;
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00002E10 File Offset: 0x00001010
        protected override void OnInit()
        {
            this.겷곏 = new HistoricalDataCustom(this);
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00002E1E File Offset: 0x0000101E
        protected override void OnClear()
        {
            this.겷곏.Dispose();
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x06000059 RID: 89 RVA: 0x00002E2C File Offset: 0x0000102C
        // (set) Token: 0x0600005A RID: 90 RVA: 0x00002EAC File Offset: 0x000010AC
        public override IList<SettingItem> Settings
        {
            get
            {
                IList<SettingItem> settings = base.Settings;
                settings.Add(new SettingItemPairColor(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곂(), new PairColor(this.UpBarColor, this.DownBarColor, loc._(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.괕(), null, F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곓()), loc._(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곗(), null, F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곓())), 0)
                {
                    Text = loc._(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곯(), null, F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곓()),
                    SeparatorGroup = settings[0].SeparatorGroup
                });
                return settings;
            }
            set
            {
                SettingItem itemByName = value.GetItemByName(F76E122D - 494D - 43A5 - 9451 - 1648058B0963.곂());
                PairColor pairColor = ((itemByName != null) ? itemByName.Value : null) as PairColor;
                if (pairColor != null)
                {
                    this.UpBarColor = pairColor.Color1;
                    this.DownBarColor = pairColor.Color2;
                }
                base.Settings = value;
            }
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00002EF8 File Offset: 0x000010F8
        public override void OnPaintChart(PaintChartEventArgs args)
        {
            Graphics graphics = args.Graphics;
            RectangleF clipBounds = graphics.ClipBounds;
            graphics.SetClip(args.Rectangle);
            try
            {
                if (this.IsLoading)
                {
                    graphics.DrawString(this.LoadingMessage, this.겷곎, Brushes.DodgerBlue, args.Rectangle, this.겷겦);
                }
                else if (base.HistoricalData.Count != 0)
                {
                    DateTime time = base.CurrentChart.Windows[args.WindowIndex].CoordinatesConverter.GetTime((double)args.Rectangle.Left);
                    DateTime time2 = base.CurrentChart.Windows[args.WindowIndex].CoordinatesConverter.GetTime((double)args.Rectangle.Right);
                    int num = (int)base.HistoricalData.GetIndexByTime(time.Ticks, SeekOriginHistory.End);
                    int num2 = (int)base.HistoricalData.GetIndexByTime(time2.Ticks, SeekOriginHistory.End);
                    if (num2 == -1)
                    {
                        num2 = 0;
                    }
                    if (num == -1)
                    {
                        num = base.HistoricalData.Count - 1;
                    }
                    float num3 = 0f;
                    IChartWindow chartWindow = base.CurrentChart.Windows[args.WindowIndex];
                    float num4 = Math.Max(1f, (float)(base.CurrentChart.BarsWidth / 10));
                    float num5 = Math.Max(1f, (float)base.CurrentChart.BarsWidth - num4 * 2f);
                    this.BarWickLineWidth = ((base.CurrentChart.BarsWidth > 50) ? 3 : 1);
                    for (int i = num; i >= num2; i--)
                    {
                        if (this.겷곏.Count > i)
                        {
                            DateTime timeLeft = base.HistoricalData[i, SeekOriginHistory.End].TimeLeft;
                            if (num3 == 0f)
                            {
                                num3 = (float)chartWindow.CoordinatesConverter.GetChartX(timeLeft) + num4;
                            }
                            else
                            {
                                num3 += (float)base.CurrentChart.BarsWidth;
                            }
                            double num6 = this.겷곏[i, SeekOriginHistory.End][PriceType.Open];
                            double num7 = this.겷곏[i, SeekOriginHistory.End][PriceType.Close];
                            if (!double.IsNaN(num6) && !double.IsNaN(num7))
                            {
                                bool flag = num6 < num7;
                                float num8 = (float)chartWindow.CoordinatesConverter.GetChartY(num6);
                                float num9 = (float)chartWindow.CoordinatesConverter.GetChartY(num7);
                                float num10 = flag ? (num8 - num9) : (num9 - num8);
                                if (num10 < 1f)
                                {
                                    num10 = 1f;
                                }
                                if (flag)
                                {
                                    graphics.FillRectangle(this.겷괈.Brush, num3, num9, num5, num10);
                                }
                                else
                                {
                                    graphics.FillRectangle(this.겷격.Brush, num3, num8, num5, num10);
                                }
                                if (num5 != 1f)
                                {
                                    double num11 = this.겷곏[i, SeekOriginHistory.End][PriceType.High];
                                    double num12 = this.겷곏[i, SeekOriginHistory.End][PriceType.Low];
                                    if (!double.IsNaN(num11) && !double.IsNaN(num12) && (num11 != num6 || num12 != num7) && (num11 != num7 || num12 != num6))
                                    {
                                        float y = (float)base.CurrentChart.Windows[args.WindowIndex].CoordinatesConverter.GetChartY(num11);
                                        float y2 = (float)base.CurrentChart.Windows[args.WindowIndex].CoordinatesConverter.GetChartY(num12);
                                        float num13 = num3 + num5 / 2f;
                                        graphics.DrawLine(flag ? this.겷괈 : this.겷격, num13, y, num13, y2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExecutionEntity.Core.Loggers.Log(ex, null, LoggingLevel.Error, null);
            }
            finally
            {
                graphics.SetClip(clipBounds);
            }
        }

        // Token: 0x0600005C RID: 92 RVA: 0x000032E0 File Offset: 0x000014E0
        protected void SetValues(double open, double high, double low, double close, int offset)
        {
            if (!CandleDrawIndicator.IsValidPrice(open) || !CandleDrawIndicator.IsValidPrice(close))
            {
                return;
            }
            this.겷곏.SetValue(open, high, low, close, offset);
            base.SetValue(open, 0, offset);
            base.SetValue(high, 1, offset);
            base.SetValue(low, 2, offset);
            base.SetValue(close, 3, offset);
        }

        // Token: 0x0600005D RID: 93 RVA: 0x0000333C File Offset: 0x0000153C
        protected void SetHole(int offset)
        {
            this.겷곏.SetValue(double.NaN, double.NaN, double.NaN, double.NaN, offset);
            base.SetValue(double.NaN, 0, offset);
            base.SetValue(double.NaN, 1, offset);
            base.SetValue(double.NaN, 2, offset);
            base.SetValue(double.NaN, 3, offset);
        }

        // Token: 0x0600005E RID: 94 RVA: 0x000033BD File Offset: 0x000015BD
        protected static bool IsValidPrice(double price)
        {
            return !double.IsNaN(price) && price != double.MinValue && price != double.MaxValue;
        }

        // Token: 0x04000018 RID: 24
        private const int 겷겧 = 50;

        // Token: 0x04000019 RID: 25
        private Color 겷곧;

        // Token: 0x0400001A RID: 26
        private Pen 겷괈;

        // Token: 0x0400001B RID: 27
        private Color 겷겾;

        // Token: 0x0400001C RID: 28
        private Pen 겷격;

        // Token: 0x0400001D RID: 29
        private HistoricalDataCustom 겷곏;

        // Token: 0x0400001E RID: 30
        private int 겷곇;

        // Token: 0x0400001F RID: 31
        [CompilerGenerated]
        private bool 겷겼;

        // Token: 0x04000020 RID: 32
        private readonly Font 겷곎;

        // Token: 0x04000021 RID: 33
        private readonly StringFormat 겷겦;
*/
    }
}
