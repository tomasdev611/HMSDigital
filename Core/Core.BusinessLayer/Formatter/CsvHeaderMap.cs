using CsvHelper.Configuration;
using HMSDigital.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.BusinessLayer.Formatter
{
    public class CsvHeaderMap<T> : ClassMap<T> where T : class
    {
        public CsvHeaderMap(CsvMapping<InputMappedItem> mapping)
        {
            if (mapping != null && mapping.ColumnNameMappings != null)
            {
                var csvInputMapping = new Dictionary<string, InputMappedItem>(mapping.ColumnNameMappings, StringComparer.OrdinalIgnoreCase);
                foreach (var property in typeof(T).GetProperties())
                {
                    if (csvInputMapping.ContainsKey(property.Name))
                    {
                        var csvConfiguration = csvInputMapping[property.Name];
                        if (property.PropertyType == typeof(long?) || property.PropertyType == typeof(long))
                        {
                            Map(typeof(T), property).Name(csvConfiguration.Name).TypeConverter<LongConverter<long>>();
                        }
                        else
                        {
                            Map(typeof(T), property).Name(csvConfiguration.Name);
                        }
                    }
                }
            }
        }
    }
}
