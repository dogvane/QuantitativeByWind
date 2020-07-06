using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAPIWrapperCSharp;
using WindCommon;

namespace WindTest
{
    class Program
    {
        static void Main(string[] args)
        {

            DoAPISameple();


            //             RBreakSample rbreakSample = new RBreakSample();
            //             rbreakSample.DoIt();

            Console.WriteLine("End......");
            Console.ReadKey();
        }

        static void DoAPISameple()
        {
            WindAPI w = new WindAPI();
            w.start();

            //wset取沪深300指数成分
            //WindData wd = w.wset("IndexConstituent", "date=20141215;windcode=000300.SH");
            //OutputWindData(wd, "wset");

            WindData wd = w.wsd("I2009.DCE", "open", "2020-05-24", "2020-05-24", "");
            OutputWindData(wd, "wsd");

            w.stop();
        }

        static void OutputWindData(WindData wd, string strFuncName)
        {
            string s = WindDataMethod.WindDataToString(wd, strFuncName);
            Console.Write(s);
        }
    }
}
