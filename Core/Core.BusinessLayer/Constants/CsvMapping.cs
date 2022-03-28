
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.BusinessLayer.Constants
{
    public static class CsvMapping
    {
        public const string MAPPING_DICTIONARY = "csv-header-mapping-dict";

        public static CsvMapping<InputMappedItem> GetInputCsvMapping(MappedItemTypes mappedItem)
        {
            if (mappedItem == MappedItemTypes.HospiceMember)
            {
                return new CsvMapping<InputMappedItem>()
                {
                    ColumnNameMappings = new Dictionary<string, InputMappedItem>()
                    {
                        {"firstName", new InputMappedItem(){ IsRequired = true, Name = "First Name", Type = "string", ColumnOrder = 1} },
                        {"lastName", new InputMappedItem(){ IsRequired = true, Name = "Last Name", Type = "string", ColumnOrder = 2} },
                        {"email", new InputMappedItem(){ IsRequired = true, Name = "Email", Type = "string", ColumnOrder = 3} },
                        {"phoneNumber", new InputMappedItem(){ IsRequired = false, Name = "Phone Number", Type = "numeric", ColumnOrder = 4} },
                        {"role", new InputMappedItem(){ IsRequired = false, Name = "Role", Type = "string", ColumnOrder = 5} },
                    }
                };
            }
            else if (mappedItem == MappedItemTypes.Facility)
            {
                return new CsvMapping<InputMappedItem>()
                {
                    ColumnNameMappings = new Dictionary<string, InputMappedItem>()
                    {
                        {"name", new InputMappedItem(){ IsRequired = true, Name = "Name", Type = "string", ColumnOrder = 1} },
                        {"hospiceLocationName", new InputMappedItem(){ IsRequired = true, Name = "Hospice Location", Type = "string", ColumnOrder = 2} },
                        {"phoneNumber", new InputMappedItem(){ IsRequired = false, Name = "Phone Number", Type = "numeric", ColumnOrder = 3} },
                        {"addressLine1", new InputMappedItem(){ IsRequired = true, Name = "Street Address", Type = "string", ColumnOrder = 4} },
                        {"addressLine2", new InputMappedItem(){ IsRequired = false, Name = "Apt/Unit/Building", Type = "string", ColumnOrder = 5} },
                        {"city", new InputMappedItem(){ IsRequired = false, Name = "City", Type = "string", ColumnOrder = 6} },
                        {"state", new InputMappedItem(){ IsRequired = false, Name = "State", Type = "string", ColumnOrder = 7} },
                        {"zipCode", new InputMappedItem(){ IsRequired = true, Name = "Zip Code", Type = "numeric", ColumnOrder = 8} },
                    }
                };
            }
            return new CsvMapping<InputMappedItem>();
        }

        public static CsvMapping<OutputMappedItem> GetOutputCsvMapping(MappedItemTypes mappedItem)
        {
            if (mappedItem == MappedItemTypes.HospiceMember)
            {
                return new CsvMapping<OutputMappedItem>()
                {
                    ColumnNameMappings = new Dictionary<string, OutputMappedItem>()
                    {
                        
                        {"firstName", new OutputMappedItem(){ Name = "First Name", Type = "string", ColumnOrder = 1, ShowInDisplay = true, IncludeInExport = true} },
                        {"lastName", new OutputMappedItem(){ Name = "Last Name", Type = "string", ColumnOrder = 2, ShowInDisplay = true, IncludeInExport = true} },
                        {"email", new OutputMappedItem(){ Name = "Email", Type = "string", ColumnOrder = 3, ShowInDisplay = true, IncludeInExport = true} },
                        {"phoneNumber", new OutputMappedItem(){ Name = "Phone Number", Type = "numeric", ColumnOrder = 4, ShowInDisplay = true, IncludeInExport = true} },
                        {"role", new OutputMappedItem(){ Name = "Role", Type = "string", ColumnOrder = 5, ShowInDisplay = true, IncludeInExport = true} },
                                            }
                };
            }
            else if (mappedItem == MappedItemTypes.Facility)
            {
                return new CsvMapping<OutputMappedItem>()
                {
                    ColumnNameMappings = new Dictionary<string, OutputMappedItem>()
                    {
                        {"name", new OutputMappedItem(){ Name = "Name", Type = "string", ColumnOrder = 1, ShowInDisplay = true, IncludeInExport = true} },
                        {"hospiceLocationName", new OutputMappedItem(){ Name = "Hospice Location", Type = "string", ColumnOrder = 2, ShowInDisplay = true, IncludeInExport = true} },
                        {"phoneNumber", new OutputMappedItem(){ Name = "phone Number", Type = "numeric", ColumnOrder = 3, ShowInDisplay = true, IncludeInExport = true} },
                        {"addressLine1", new OutputMappedItem(){ Name = "Street Address", Type = "string", ColumnOrder = 4, ShowInDisplay = true, IncludeInExport = true} },
                        {"addressLine2", new OutputMappedItem(){ Name = "Apt/Unit/Building", Type = "string", ColumnOrder = 5, ShowInDisplay = true, IncludeInExport = true} },
                        {"city", new OutputMappedItem(){ Name = "City", Type = "string", ColumnOrder = 6, ShowInDisplay = true, IncludeInExport = true} },
                        {"state", new OutputMappedItem(){ Name = "State", Type = "string", ColumnOrder = 7, ShowInDisplay = true, IncludeInExport = true} },
                        {"zipCode", new OutputMappedItem(){ Name = "Zip Code", Type = "numeric", ColumnOrder = 8, ShowInDisplay = true, IncludeInExport = true} },
                    }
                };
            }
            return new CsvMapping<OutputMappedItem>();
        }

    }
}
