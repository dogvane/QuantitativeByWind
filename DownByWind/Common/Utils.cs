using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.Common
{
    class Utils
    {
    }

    /// <summary>
    /// 专注于扩展方法
    /// </summary>
    public static class UtilsExtend
    {
        public static double? GetDouble(this object source)
        {
            if (source == null)
                return null;

            if (source is double || source is float || source is int || source is long || source is short || source is ushort || source is byte)
            {
                var ret = (double)source;

                if (double.IsNaN(ret))
                    return null;

                return ret;
            }

            return null;
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
