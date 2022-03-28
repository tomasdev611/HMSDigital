using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApiContrib.Core.Formatter.Csv;

namespace HMSDigital.Core.BusinessLayer.Formatter
{
    public class MappedNameCsvOutputFormatter : CsvOutputFormatter
    {
        private readonly CsvFormatterOptions _options;

        public MappedNameCsvOutputFormatter(CsvFormatterOptions csvFormatterOptions) : base(csvFormatterOptions)
        {
            _options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
        }

        private string GetDisplayNameFromMapping(PropertyInfo pi, Dictionary<string, OutputMappedItem> caseInsensitiveMappingDictionary)
        {
            if (caseInsensitiveMappingDictionary.TryGetValue(pi.Name, out var mappedItem))
            {
                return !string.IsNullOrEmpty(mappedItem.Name) ? mappedItem.Name : "";
            }

            if (pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.PropertyName is string value)
            {
                return value;
            }

            return pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (!context.HttpContext.Items.TryGetValue(CsvMapping.MAPPING_DICTIONARY, out object mappingJson))
            {
                await base.WriteResponseBodyAsync(context);
            }
            else
            {
                var mapping = JsonConvert.DeserializeObject<CsvMapping<OutputMappedItem>>(mappingJson.ToString());
                var response = context.HttpContext.Response;

                Type type = context.Object.GetType();
                Type itemType;

                if (type.GetGenericArguments().Length > 0)
                {
                    itemType = type.GetGenericArguments()[0];
                }
                else
                {
                    itemType = type.GetElementType();
                }

                var streamWriter = new StreamWriter(response.Body, _options.Encoding);

                if (_options.IncludeExcelDelimiterHeader)
                {
                    await streamWriter.WriteLineAsync($"sep ={_options.CsvDelimiter}");
                }

                var caseInsensitiveMappingDictionary = new Dictionary<string, OutputMappedItem>(mapping.ColumnNameMappings, StringComparer.OrdinalIgnoreCase);
                if (_options.UseSingleLineHeaderInCsv)
                {
                    var values = itemType.GetProperties()
                            .Where(pi => pi.GetCustomAttributes<Attribute>().Any() ||
                            (!pi.GetCustomAttributes<JsonIgnoreAttribute>(false).Any()
                                && (caseInsensitiveMappingDictionary.ContainsKey(pi.Name) && caseInsensitiveMappingDictionary.TryGetValue(pi.Name, out var mappedItem) && mappedItem.IncludeInExport)))    // Only get the properties that do not define JsonIgnore
                            .Select(pi => new
                            {
                                Order = caseInsensitiveMappingDictionary.ContainsKey(pi.Name) ?
                                            caseInsensitiveMappingDictionary[pi.Name].ColumnOrder : pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order,
                                Prop = pi
                            })
                            .OrderBy(d => d.Order)
                            .Select(d => GetDisplayNameFromMapping(d.Prop, caseInsensitiveMappingDictionary));

                    await streamWriter.WriteLineAsync(string.Join(_options.CsvDelimiter, values));
                }


                foreach (var obj in (IEnumerable<object>)context.Object)
                {
                    var vals = obj.GetType().GetProperties()
                            .Where(pi => pi.GetCustomAttributes<Attribute>().Any() ||
                           (!pi.GetCustomAttributes<JsonIgnoreAttribute>().Any()
                            && (caseInsensitiveMappingDictionary.ContainsKey(pi.Name) && caseInsensitiveMappingDictionary.TryGetValue(pi.Name, out var mappedItem) && mappedItem.IncludeInExport)))
                            .Select(pi => new
                            {
                                Order = caseInsensitiveMappingDictionary.ContainsKey(pi.Name) ?
                                            caseInsensitiveMappingDictionary[pi.Name]?.ColumnOrder : pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order,
                                Value = pi.GetValue(obj, null)
                            }).OrderBy(d => d.Order).Select(d => new { d.Value });

                    string valueLine = string.Empty;

                    foreach (var val in vals)
                    {
                        if (val.Value != null)
                        {

                            var _val = val.Value.ToString();

                            //Escape quotas
                            _val = _val.Replace("\"", "\"\"");

                            //Check if the value contans a delimiter and place it in quotes if so
                            if (_val.Contains(_options.CsvDelimiter))
                                _val = string.Concat("\"", _val, "\"");

                            //Replace any \r or \n special characters from a new line with a space
                            if (_val.Contains("\r"))
                                _val = _val.Replace("\r", " ");
                            if (_val.Contains("\n"))
                                _val = _val.Replace("\n", " ");

                            valueLine = string.Concat(valueLine, _val, _options.CsvDelimiter);

                        }
                        else
                        {
                            valueLine = string.Concat(valueLine, string.Empty, _options.CsvDelimiter);
                        }
                    }

                    await streamWriter.WriteLineAsync(valueLine.Remove(valueLine.Length - _options.CsvDelimiter.Length));
                }

                await streamWriter.FlushAsync();
            }
        }
    }
}
