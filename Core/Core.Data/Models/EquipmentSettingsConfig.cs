using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class EquipmentSettingsConfig
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int EquipmentSettingTypeId { get; set; }

        public virtual EquipmentSettingTypes EquipmentSettingType { get; set; }
        public virtual Items Item { get; set; }
    }
}
