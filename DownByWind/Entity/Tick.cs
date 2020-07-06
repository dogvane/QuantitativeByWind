using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.Entity
{

	public class Tick
	{
		/// <summary>
		/// 最新价
		/// </summary>
		public double LastPrice { get; set; }
		/// <summary>
		///申买价一
		/// </summary>
		public double BidPrice { get; set; }
		/// <summary>
		///申买量一
		/// </summary>
		public int BidVolume { get; set; }
		/// <summary>
		///申卖价一
		/// </summary>
		public double AskPrice { get; set; }
		/// <summary>
		///申卖量一
		/// </summary>
		public int AskVolume { get; set; }
		/// <summary>
		///当日均价
		/// </summary>
		public double AveragePrice { get; set; }
		/// <summary>
		///数量
		/// </summary>
		public int Volume { get; set; }
		/// <summary>
		///持仓量
		/// </summary>
		public double OpenInterest { get; set; }
		/// <summary>
		/// 交易日
		/// </summary>
		public int TradingDay { get; set; }
		/// <summary>
		///最后修改时间:yyyyMMdd HH:mm:ss(20141114:日期由主程序处理,因大商所取到的actionday==tradingday)
		/// </summary>
		public string UpdateTime { get; set; }
		/// <summary>
		///最后修改毫秒
		/// </summary>
		public int UpdateMillisec { get; set; }
	}
}
