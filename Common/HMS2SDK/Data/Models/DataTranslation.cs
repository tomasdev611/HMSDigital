using System;

namespace HMS2SDK.Data.Models
{
    public partial class DataTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string DataSource { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
