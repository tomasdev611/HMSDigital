using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HMSDigital.Core.BusinessLayer.Formatter
{
    public class LongConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string convertingString, IReaderRow row, MemberMapData memberMapData)
        {
            return Convert.ToInt64(Regex.Replace(convertingString, "[(),-/ ]", ""));
        }
    }
}
