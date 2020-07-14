using DownByWind.DbSet;
using DownByWind.Down;
using DownByWind.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WAPIWrapperCSharp;

namespace DownByWSET
{
    class Program
    {
        unsafe static void Main(string[] args)
        {
            // InitDatabase();
            DownBar.DownAll();
            // DownCodeInfo.DownAllCodes();
        }


        static void InitDatabase()
        {
            var db = new QuantDBContext();
            var databaseCreator = db.GetService<IRelationalDatabaseCreator>();
            Console.WriteLine(databaseCreator.GenerateCreateScript());
            databaseCreator.CreateTables();            
        }

        static Regex codeReg = new Regex("([A-Z]|[a-z])*");


        //private static void DumpAllTick()
        //{

        //    var dumpDate = "2020-06-24";
        //    //var code = "RB2010.SHF";

        //    var codes = GetAllCodes(dumpDate);

        //    codes.WriteCSVFile("code.csv");

        //    foreach (var item in codes)
        //    {
        //        if (item.WindCode.Contains("TAS"))  // 仿真的先忽略了
        //            continue;

        //        var codeName = codeReg.Match(item.WindCode).Value;

        //        var path = Path.Combine("tick", codeName, item.WindCode);
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        var fileName = Path.Combine(path, item.WindCode + "_" + dumpDate + ".txt");
        //        Console.WriteLine(fileName);
        //        if (File.Exists(fileName))
        //            continue;

        //        List<Tick> list = DumpTicks(item.WindCode, dumpDate);
        //        list.WriteCSVFile(fileName);
        //    }
        //}

        ///// <summary>
        ///// 下载tisk的格式数据
        ///// </summary>
        ///// <param name="code">格式：RB2010.SHF</param>
        ///// <param name="dumpDate">格式： 2020-06-01</param>
        ///// <returns></returns>
        //private static unsafe List<Tick> DumpTicks(string code, string dumpDate)
        //{
        //    WindAPI wind = new WindAPI();
        //    wind.start();

        //    var endDateTime = DateTime.Parse(dumpDate).Date.AddHours(15);   // 3点结束

        //    var tradeDate = wind.tdays(endDateTime.AddDays(-7), endDateTime, "");
        //    var td = (object[])tradeDate.data;
        //    var startDateTime = (DateTime)td[td.Length - 2];  // 上一个交易日
        //    startDateTime = startDateTime.Date.AddHours(21);    // 晚上9点开盘

        //    Console.WriteLine(startDateTime);
        //    Console.WriteLine(endDateTime);

        //    WindData wd = wind.wst(code, "last,bid,bsize1,ask,asize1,amt,volume,oi", startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), endDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "");
        //    if (wd.data == null)
        //        return new List<Tick>();

        //    Console.WriteLine(wd.fieldList.Length);
        //    var data = (double[])wd.data;
        //    var w = wd.fieldList.Length;
        //    var l = (int)(data.Length / w);

        //    var d2 = new double[l, w];
        //    Buffer.BlockCopy(data, 0, d2, 0, data.Length * 8);  // 一个double 8个byte

        //    List<Tick> list = new List<Tick>();

        //    for (var i = 0; i < l; i++)
        //    {
        //        var tick = new Tick();
        //        tick.LastPrice = d2[i, 0];
        //        tick.BidPrice = d2[i, 1];
        //        tick.BidVolume = (int)d2[i, 2];
        //        tick.AskPrice = d2[i, 3];
        //        tick.AskVolume = (int)d2[i, 4];

        //        tick.AveragePrice = d2[i, 5] / d2[i, 6] / 10; // 用总金额/成交量，但是估计是wind的关系，需要再多除一个10

        //        tick.Volume = (int)d2[i, 6];
        //        tick.OpenInterest = d2[i, 7];

        //        tick.TradingDay = 0;
        //        tick.UpdateTime = wd.timeList[i].ToString("yyyy-MM-dd HH:mm:ss");
        //        tick.UpdateMillisec = int.Parse(wd.timeList[i].ToString("fff"));
        //        list.Add(tick);
        //    }

        //    return list;
        //}



    }



}
