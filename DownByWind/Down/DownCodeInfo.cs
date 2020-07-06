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
            for(var i =0; i < l; i++)
            {
                var info = new CodeInfo();
                info.WindCode = windcode[i];

                info.LastTradeDate = (DateTime)ws[i, 0];
                info.DLMonth = int.Parse(ws[i, 1].ToString());
                info.Transactionfee = (string)ws[i, 2];
                info.TodayPositionfee = (string)ws[i, 3];
                info.Name = (string)ws[i, 4];
                
                if(ws[i, 5] != null)
                    info.margin = (double)ws[i, 5];
                
                info.ContractIssuedate = (DateTime)ws[i, 6];
                info.IssueDate = (DateTime)ws[i, 7];

                info.Code = CodeInfo.GetTradeCode(info.WindCode);

                ret.Add(info);
            }

            return ret.ToArray();
        }
    }
}
