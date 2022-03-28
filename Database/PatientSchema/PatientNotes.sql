CREATE TABLE [patient].[PatientNotes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PatientId] INT NULL, 
    [Note] VARCHAR(MAX) NULL,
    [CreatedDateTime] DATETIME2 NULL, 
    [CreatedByUserId] INT NULL   , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedDateTime] DATETIME2 NULL,  
    [DeletedByUserId] INT NULL DEFAULT NULL, 
    CONSTRAINT [FK_PatientNotes_patientDetails] FOREIGN KEY ([PatientId]) REFERENCES [patient].[PatientDetails]([Id]) ON DELETE CASCADE,
)
