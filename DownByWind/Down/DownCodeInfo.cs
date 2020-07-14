using DownByWind.DbSet;
using DownByWind.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAPIWrapperCSharp;

namespace DownByWind.Down
{
    class DownCodeInfo
    {
        public static CodeInfo[] Down(string[] windcode, string date)
        {
            WindAPI wind = new WindAPI();
            wind.start();
            if (string.IsNullOrEmpty(date))
                date = DateTime.Now.ToString("yyyyMMdd");

            var wd = wind.wss(string.Join(",", windcode), $"lasttrade_date,dlmonth,transactionfee,todaypositionfee,sccode,margin,contract_issuedate,ipo_date", $"tradeDate={date}");

            if (wd.data == null)
                return null;

            var ws = wd.getDataByFunc("wss", false) as object[,];

            var l = ws.GetLongLength(0);

            List<CodeInfo> ret = new List<CodeInfo>();
            for (var i = 0; i < l; i++)
            {
                var info = new CodeInfo();
                info.WindCode = windcode[i];

                info.LastTradeDate = (DateTime)ws[i, 0];
                info.DLMonth = int.Parse(ws[i, 1].ToString());
                info.Transactionfee = (string)ws[i, 2];
                info.TodayPositionfee = (string)ws[i, 3];
                info.Name = (string)ws[i, 4];

                if (ws[i, 5] != null)
                    info.margin = (double)ws[i, 5];

                info.ContractIssuedate = (DateTime)ws[i, 6];
                info.IssueDate = (DateTime)ws[i, 7];

                info.Code = CodeInfo.GetTradeCode(info.WindCode);

                ret.Add(info);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 下载所有的代码
        /// </summary>
        public static void DownAllCodes()
        {
            // 当然，下载所有的不现实，国内期货也才不到30年时间，按照20年的长度来获取就差不多了，再早的参考价值也不高了
            var now = DateTime.Now;
            for (var i = 2000; i < now.Year; i++)
            {
                var tradeDate = i + now.ToString("-MM-dd");
                var allCodes = GetAllCodes(tradeDate);

                using (var db = new QuantDBContext())
                {
                    var dbCodes = db.CodeInfos.Select(o => o.WindCode).ToArray();
                    var existsWindCode = new HashSet<string>(dbCodes);
                    var notfindWindCodes = allCodes.Where(o => !existsWindCode.Contains(o.WindCode)).Select(o => o.WindCode).ToArray();

                    if (notfindWindCodes.Length > 0)
                    {
                        var ci = Down(notfindWindCodes, tradeDate);
                        Console.WriteLine($"find new windCodes {ci.Length}");
                        db.CodeInfos.AddRange(ci);
                        db.SaveChanges();
                    }
                }
            }
        }

        public static List<CodeInfo> GetAllCodes(string dateTime = null)
        {
            WindAPI wind = new WindAPI();
            wind.start();

            var date = DateTime.Parse(dateTime);

            List<CodeInfo> rets = new List<CodeInfo>();

            rets.AddRange(GetSectorCodeInfo(wind, date, "a599010201000000"));  // 上海交易所
            rets.AddRange(GetSectorCodeInfo(wind, date, "a599010301000000"));  // 大商所
            rets.AddRange(GetSectorCodeInfo(wind, date, "a599010401000000"));  // 郑商所

            return rets;
        }


        private static List<CodeInfo> GetSectorCodeInfo(WindAPI wind, DateTime date, string sectorId)
        {
            var wd = wind.wset("sectorconstituent", $"date={date.ToString("yyyy-MM-dd")};sectorid={sectorId}");    // 获得上海交易所数据
            var df = (object[,])wd.getDataByFunc("wset", false);

            var length = df.GetUpperBound(0);

            List<CodeInfo> rets = new List<CodeInfo>();

            for (var i = 0; i <= length; i++)
            {
                var ci = new CodeInfo()
                {
                    WindCode = df[i, 1].ToString(),
                    Name = df[i, 2].ToString(),
                };

                rets.Add(ci);
            }

            return rets;
        }
    }
}
