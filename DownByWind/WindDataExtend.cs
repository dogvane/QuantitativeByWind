using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWSET
{
    public static class WindDataExtend
    {
        public static void WriteCSVFile<T>(this List<T> items, string fileName)
        {
            var str = CsvSerializer.SerializeToCsv(items);
            File.WriteAllText(fileName, str);
        }
    }
}
