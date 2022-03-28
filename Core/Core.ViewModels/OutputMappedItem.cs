namespace HMSDigital.Core.ViewModels
{
    public class OutputMappedItem : MappedItemBase
    {
        public bool ShowInDisplay { get; set; }

        public bool IncludeInExport { get; set; }

        public bool IsFilterable { get; set; }

        public bool IsSortable { get; set; }
    }
}
