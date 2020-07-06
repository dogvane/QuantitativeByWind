using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.Entity
{
	public enum BarType
    {
		/// <summary>
		/// 日线
		/// </summary>
		Day =0,

		/// <summary>
		/// 1分钟
		/// </summary>
		M1 = 1,

		/// <summary>
		/// 3分钟
		/// </summary>
		M3 = 3,

		/// <summary>
		/// 5分钟
		/// </summary>
		M5 = 5,

		/// <summary>
		/// 5分钟
		/// </summary>
		M10 = 10,

		/// <summary>
		/// 15分钟
		/// </summary>
		M15 = 15,

		/// <summary>
		/// 30分钟
		/// </summary>
		M30 = 30,

		/// <summary>
		/// 60分钟
		/// </summary>
		M60 = 60,

		/// <summary>
		/// 周K
		/// </summary>
		Week = 101,
    }

	/// <summary>
	/// K线
	/// </summary>
	public class Bar
	{
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime D { get; set; }

		/// <summary>
		/// K线的类型
		/// </summary>
		public BarType BarType { get; set; }

		/// <summary>
		/// 交易日
		/// </summary>
		public int TradingDay { get; set; }

		/// <summary>
		/// 开盘价
		/// </summary>
		public double O { get; set; }

		/// <summary>
		/// 最高价
		/// </summary>
		public double H { get; set; }

		/// <summary>
		/// 最低价
		/// </summary>
		public double L { get; set; }

		/// <summary>
		/// 收盘价
		/// </summary>
		public double C { get; set; }

		/// <summary>
		/// 成交量
		/// </summary>
		public double V { get; set; }

		/// <summary>
		/// 持仓量
		/// </summary>
		public double I { get; set; }

		/// <summary>
		/// 结算价
		/// </summary>
		public double Settle { get; set; }

        public override string ToString()
        {
			return $"{D} Open:{O} Close:{C}";
        }
    }
}
