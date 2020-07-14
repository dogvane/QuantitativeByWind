using DownByWind.Common;
using DownByWind.DbSet;
using DownByWind.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using WAPIWrapperCSharp;

namespace DownByWind.Down
{
    /// <summary>
    /// 下载K线数据
    /// </summary>
    class DownBar
    {
        static WindAPI wind = new WindAPI();
        static DownBar()
        {
            wind.start();
        }

        public static unsafe Bar[] DumpBarByWind(string code, string startDate, string endDate, BarType barType)
        {
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

                    bar.Settle = data[i, 5].GetDouble();
                    bar.O = data[i, 0].GetDouble();
                    bar.H = data[i, 1].GetDouble();
                    bar.L = data[i, 2].GetDouble();
                    bar.C = data[i, 3].GetDouble();
                    bar.V = data[i, 4].GetDouble();
                    bar.I = data[i, 6].GetDouble();
                    bar.WindCode = code;
                    bar.BarType = barType;
                    ret.Add(bar);
                }
            }
            else
            {
                var wd = wind.wsi(code, "open,high,low,close,volume,oi", startDate, endDate, $"BarSize={(int)barType}");

                if (wd.data == null)
                    return ret.ToArray();

                object[,] data = wd.getDataByFunc("wsi", false) as object[,];
                if(data == null)
                {
                    Console.WriteLine($"{code} not find wsi {startDate} ~ {endDate}");
                    return ret.ToArray();
                }

                var w = wd.fieldList.Length;
                var l = (int)(data.Length / w);

                for (var i = 0; i < l; i++)
                {
                    var bar = new Bar();

                    bar.TradingDay = int.Parse(wd.timeList[i].ToString("yyyyMMdd"));
                    bar.D = wd.timeList[i];

                    bar.O = data[i, 0].GetDouble();
                    bar.H = data[i, 1].GetDouble();
                    bar.L = data[i, 2].GetDouble();
                    bar.C = data[i, 3].GetDouble();
                    bar.V = data[i, 4].GetDouble();
                    bar.I = data[i, 5].GetDouble();
                    bar.WindCode = code;
                    bar.BarType = barType;
                    ret.Add(bar);
                }
            }

            return ret.ToArray();
        }

        public static void DownAll()
        {
            // DownExitsTrade();
            // DownFinishTrade();
            DownFinishMinTrade(BarType.M1);

        }

        /// <summary>
        /// 下载到现在还在交易的数据
        /// </summary>
        public static void DownTradeing()
        {
            using (var db = new QuantDBContext())
            {
                var codes = db.CodeInfos.Where(o => o.LastTradeDate > DateTime.Now).ToArray();
                foreach (var item in codes)
                {
                    try
                    {
                        // 先下载日K的看看效果
                        var dbItems = db.Bars.Where(o => o.WindCode == item.WindCode && o.BarType == BarType.Day).ToArray();
                        var endDateItem = dbItems.OrderByDescending(o => o.TradingDay).FirstOrDefault();
                        var startDate = item.IssueDate.ToString("yyyyMMdd");
                        var endDate = DateTime.Now.ToString("yyyyMMdd");

                        if (endDateItem != null)
                        {
                            startDate = endDateItem.TradingDay.ToString();
                            if (startDate == endDate)
                                continue;
                        }

                        var bars = DumpBarByWind(item.WindCode, startDate, endDate, BarType.Day);
                        Console.WriteLine($"{item.WindCode} {startDate} - {endDate} count:{bars.Length}");

                        var count = 0;
                        foreach (var newItem in bars)
                        {
                            if (!dbItems.Any(o => o.TradingDay == newItem.TradingDay))
                            {
                                db.Add(newItem);
                                count++;
                            }
                        }

                        if (count > 0)
                        {
                            Console.WriteLine($"add {item.WindCode} {count}");
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.WindCode);
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 下载已经结束交易的品种数据
        /// </summary>
        public static void DownFinishTrade()
        {
            CodeInfo[] codes;
            using (var db = new QuantDBContext())
            {
                codes = db.CodeInfos.Where(o => o.LastTradeDate < DateTime.Now.Date).OrderByDescending(o=>o.LastTradeDate).ToArray();
            }

            // Parallel.ForEach(codes, (item, ParallelLoopState) =>
            foreach (var item in codes)
            {
                using (var db = new QuantDBContext())
                {
                    try
                    {
                        // 先下载日K的看看效果
                        var dbItems = db.Bars.Where(o => o.WindCode == item.WindCode && o.BarType == BarType.Day).OrderBy(o => o.TradingDay).ToArray();

                        var startDate = item.IssueDate.ToString("yyyyMMdd");
                        var endDate = item.LastTradeDate.ToString("yyyyMMdd");

                        var firtItem = dbItems.FirstOrDefault();
                        var endItem = dbItems.LastOrDefault();
                        if (firtItem != null && firtItem.TradingDay.ToString() == startDate
                            && endItem != null && endItem.TradingDay.ToString() == endDate)
                        {
                            Console.WriteLine($"Ignore {item.WindCode}");
                        }
                        else
                        {
                            var bars = DumpBarByWind(item.WindCode, startDate, endDate, BarType.Day);
                            Console.WriteLine($"{item.WindCode} {startDate} - {endDate} count:{bars.Length}");

                            var count = 0;
                            foreach (var newItem in bars)
                            {
                                if (!dbItems.Any(o => o.TradingDay == newItem.TradingDay))
                                {
                                    db.Add(newItem);
                                    count++;
                                }
                            }

                            if (count > 0)
                            {
                                Console.WriteLine($"add {item.WindCode} {count}");
                                db.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.WindCode);
                        Console.WriteLine(ex);
                    }
                }
            }
            // });
        }

        /// <summary>
        /// 下载已经结束交易的品种数据
        /// 这里只下载分钟数据
        /// </summary>
        public static void DownFinishMinTrade(BarType barType)
        {
            if (barType == BarType.Day || barType == BarType.Week)
                return;

            CodeInfo[] codes;
            using (var db = new QuantDBContext())
            {
                codes = db.CodeInfos.Where(o => o.LastTradeDate < DateTime.Now.Date).OrderByDescending(o=>o.LastTradeDate).ToArray();
            }

            foreach (var item in codes)
            {
                using (var db = new QuantDBContext())
                {
                    try
                    {
                        // 先下载日K的看看效果
                        var dbItems = db.Bars.Where(o => o.WindCode == item.WindCode && o.BarType == barType).OrderBy(o => o.D).ToArray();

                        var startDate = item.IssueDate.ToString("yyyyMMdd");
                        var endDate = item.LastTradeDate.ToString("yyyyMMdd");

                        var firtItem = dbItems.FirstOrDefault();
                        var endItem = dbItems.LastOrDefault();
                        if (firtItem != null && firtItem.TradingDay.ToString() == startDate
                            && endItem != null && endItem.TradingDay.ToString() == endDate)
                        {
                            Console.WriteLine($"Ignore {item.WindCode}");
                        }
                        else
                        {
                            var startDateTime = item.IssueDate;
                            if (endItem != null)
                                startDateTime = endItem.D.AddMinutes(-1);

                            var bars = DumpBarByWind(item.WindCode, startDateTime.ToString("yyyy-MM-dd 21:00:00"), item.LastTradeDate.ToString("yyyy-MM-dd 13:00:00"), barType);
                            Console.WriteLine($"{item.WindCode} {startDate} - {endDate} count:{bars.Length}");

                            var count = 0;
                            foreach (var newItem in bars)
                            {
                                if (!dbItems.Any(o => o.D == newItem.D))
                                {
                                    db.Add(newItem);
                                    count++;
                                }
                            }

                            if (count > 0)
                            {
                                Console.WriteLine($"add {item.WindCode} {count}");
                                db.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.WindCode);
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        /// <summary>
        /// 下载所有的代码数据
        /// </summary>
        public static void DownAll2()
        {
            using (var db = QuantDB.GetCon())
            {
                var codes = db.Select<CodeInfo>(o => o.LastTradeDate > DateTime.Now).ToArray();

                foreach(var item in codes)
                {
                    // 先下载日K的看看效果
                    var startDate = item.IssueDate.ToString("yyyyMMdd");
                    var endDate = DateTime.Now.ToString("yyyyMMdd");

                    var bars = DumpBarByWind(item.WindCode, startDate, endDate, BarType.Day);
                    Console.WriteLine(bars.Length);

                    var dbItems = db.Select<Bar>(o => o.WindCode == item.WindCode && o.BarType == BarType.Day).ToArray();

                    var count = 0;
                    foreach (var newItem in bars)
                    {
                        if (!dbItems.Any(o => o.TradingDay == newItem.TradingDay))
                        {
                            try
                            {
                                db.Insert(newItem);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(newItem.ToJson());
                            }
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        Console.WriteLine($"add {item.WindCode} {count}");
                    }
                    break;
                }
            }
        }
    }
}
