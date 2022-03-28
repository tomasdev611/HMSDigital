CREATE TABLE [patient].[PatientAddress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PatientId] INT NULL, 
    [AddressId] INT NULL, 
    [AddressTypeId] INT NULL, 
    CONSTRAINT [FK_PatientAddressMapping_To_Patient] FOREIGN KEY ([PatientId]) REFERENCES [patient].[PatientDetails]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PatientAddressMapping_To_Address] FOREIGN KEY ([AddressId]) REFERENCES [patient].[Addresses]([Id]), 
    CONSTRAINT [FK_Address_ToTable] FOREIGN KEY ([AddressTypeId]) REFERENCES [patient].[AddressType]([Id])
)
