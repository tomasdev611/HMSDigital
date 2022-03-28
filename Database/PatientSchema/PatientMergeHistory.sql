CREATE TABLE [patient].[PatientMergeHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PatientUuid] UNIQUEIDENTIFIER NOT NULL, 
    [DuplicatePatientUuid] UNIQUEIDENTIFIER NOT NULL, 
    [ChangeLog] NVARCHAR(MAX) NULL,
    [CreatedDateTime] DATETIME2 NULL, 
    [CreatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL,
)
