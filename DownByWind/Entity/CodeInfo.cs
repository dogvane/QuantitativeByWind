using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.Entity
{

    [Alias("CodeInfos")]
    public class CodeInfo
    {
        /// <summary>
        /// 在万得下的编码，编码规则和实际交易所编码还是有所差别的
        /// </summary>
        public string WindCode { get; set; }

        /// <summary>
        /// 实际编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上市日期
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 最后的交易日
        /// </summary>
        public DateTime LastTradeDate { get; set; }

        /// <summary>
        /// 交易月份
        /// </summary>
        public int DLMonth { get; set; }

        /// <summary>
        /// 整个产品的上市日期
        /// </summary>
        public DateTime ContractIssuedate { get; set; }

        /// <summary>
        /// 交易手续费
        /// </summary>
        public string Transactionfee { get; set; }

        /// <summary>
        /// 今平手续费
        /// </summary>
        public string TodayPositionfee { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public double margin { get; set; }

        public override string ToString()
        {
            return $"{Name}({WindCode}) {IssueDate.ToString("yyyy-MM-dd")} - {LastTradeDate.ToString("yyyy-MM-dd")}";
        }

        public static string GetTradeCode(string windCode)
        {
            return windCode
                .Replace(".CZC", "")    // 郑商所
                .Replace(".INE", "")    // 上海原油期货
                .Replace(".SHF", "")    //  上交所
                .Replace(".DCE", "");   // 大连交易所
        }
    }
}
