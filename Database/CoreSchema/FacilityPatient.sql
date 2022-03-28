CREATE TABLE [core].[FacilityPatient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacilityId] INT NULL, 
    [AdditionalField1] INT NULL, 
    [PatientUUID] UNIQUEIDENTIFIER NULL, 
    [PatientRoomNumber] VARCHAR(MAX) NULL, 
    CONSTRAINT [FK_FacilityPatient_Facilities] FOREIGN KEY ([FacilityId]) REFERENCES [core].[Facilities]([Id])
)
