using DownByWind.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAPIWrapperCSharp;

namespace DownByWind.Down
{
    /// <summary>
    /// 下载K线数据
    /// </summary>
    class DownBar
    {
        public static unsafe Bar[] DumpBar(string code, string startDate, string endDate, BarType barType)
        {
            WindAPI wind = new WindAPI();
            wind.start();

            Console.WriteLine(startDate);
            Console.WriteLine(endDate);

            var ret = new List<Bar>();

            if (barType == BarType.Day || barType == BarType.Week)
            {
                WindData wd = wind.wsd(code, "open,high,low,close,volume,settle,oi", startDate, endDate, "");

                if (wd.data == null)
                    return ret.ToArray();

                object[,] data = wd.getDataByFunc("wsd", false) as object[,];
                var w = wd.fieldList.Length;
                var l = (int)(data.Length / w);

                for (var i = 0; i < l; i++)
                {
                    var bar = new Bar();

                    bar.TradingDay = int.Parse(wd.timeList[i].ToString("yyyyMMdd"));
                    bar.D = wd.timeList[i];

                    bar.O = (double)data[i, 0];
                    bar.H = (double)data[i, 1];
                    bar.L = (double)data[i, 2];
                    bar.C = (double)data[i, 3];
                    bar.V = (double)data[i, 4];
                    bar.Settle = (double)data[i, 5];
                    bar.I = (double)data[i, 6];

                    ret.Add(bar);
                }
            }
            else
            {
                var wd = wind.wsi(code, "open,high,low,close,volume,oi", startDate, endDate, $"BarSize={(int)barType}");

                if (wd.data == null)
                    return ret.ToArray();

                object[,] data = wd.getDataByFunc("wsi", false) as object[,];
                var w = wd.fieldList.Length;
                var l = (int)(data.Length / w);

                for (var i = 0; i < l; i++)
                {
                    var bar = new Bar();

                    bar.TradingDay = int.Parse(wd.timeList[i].ToString("yyyyMMdd"));
                    bar.D = wd.timeList[i];

                    bar.O = (double)data[i, 0];
                    bar.H = (double)data[i, 1];
                    bar.L = (double)data[i, 2];
                    bar.C = (double)data[i, 3];
                    bar.V = (double)data[i, 4];
                    bar.I = (double)data[i, 5];

                    ret.Add(bar);
                }
            }

            return ret.ToArray();
        }
    }
}
