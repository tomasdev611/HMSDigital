CREATE TABLE [core].[EquipmentSettingsConfig]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [EquipmentSettingTypeId] INT NOT NULL, 
    CONSTRAINT [FK_EquipmentSettingsConfig_ToItems] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_EquipmentSettingsConfig_ToEquipmentSettingTypes] FOREIGN KEY ([EquipmentSettingTypeId]) REFERENCES [core].[EquipmentSettingTypes]([Id])
)
