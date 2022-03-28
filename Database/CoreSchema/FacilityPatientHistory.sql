CREATE TABLE [core].[FacilityPatientHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacilityId] INT NULL, 
    [AdditionalField1] INT NULL, 
    [PatientUUID] UNIQUEIDENTIFIER NULL, 
    [Active] BIT NOT NULL DEFAULT 0,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL,
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL, 
    CONSTRAINT [FK_FacilityPatientHistory_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_FacilityPatientHistory_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_FacilityPatientHistory_Facilities] FOREIGN KEY ([FacilityId]) REFERENCES [core].[Facilities]([Id])
)
