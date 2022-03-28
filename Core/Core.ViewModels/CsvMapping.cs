using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class CsvMapping<T>
    {
        [JsonProperty("column_name_mapping")]
        public Dictionary<string, T> ColumnNameMappings { get; set; }
    }
}
