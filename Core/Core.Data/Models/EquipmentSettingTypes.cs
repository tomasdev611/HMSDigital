using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class EquipmentSettingTypes
    {
        public EquipmentSettingTypes()
        {
            EquipmentSettingsConfig = new HashSet<EquipmentSettingsConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EquipmentSettingsConfig> EquipmentSettingsConfig { get; set; }
    }
}
