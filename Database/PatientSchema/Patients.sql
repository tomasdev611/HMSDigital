﻿CREATE TABLE [patient].[PatientDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(MAX) NOT NULL, 
    [LastName] VARCHAR(MAX) NOT NULL, 
    [DateOfBirth] DATE NULL, 
    [PatientHeight] FLOAT NOT NULL DEFAULT 0.0000  , 
    [PatientWeight] INT NOT NULL,
    [IsInfectious] BIT NULL, 
    [HospiceId] INT NULL, 
    [HospiceLocationId] INT NULL DEFAULT NULL, 
    [FacilityId] INT NULL DEFAULT NULL, 
    [UniqueId] UNIQUEIDENTIFIER NULL DEFAULT NULL,
    [LastOrderNumber] VARCHAR(MAX) NULL, 
    [LastOrderDateTime] DATETIME2 NULL,
    [Diagnosis] NVARCHAR(MAX) NULL,    
    [Hms2Id] INT NULL,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL,
    [StatusId] INT NULL , 
    [StatusChangedDate] DATETIME NULL, 
    [StatusReasonId] INT NULL,
    [DataBridgeRunUUID] UNIQUEIDENTIFIER NULL,
	[DataBridgeRunDateTime] DATETIME2 NULL,
    [FhirPatientId] UNIQUEIDENTIFIER NULL,
    [AdditionalField3] BIT NULL, 
    [AdditionalField4] VARCHAR(MAX) NULL, 
    [AdditionalField5] DECIMAL(15, 4) NULL,
    [AdditionalField7] INT NULL,
    [AdditionalField8] INT NULL,
    [AdditionalField9] INT NULL, 
    [AdditionalField10] BIT NULL,
    CONSTRAINT [FK_PatientDetails_statusTypes] FOREIGN KEY ([StatusId]) REFERENCES [patient].[PatientStatusTypes]([Id]),
    CONSTRAINT [FK_PatientDetails_statusReasonTypes] FOREIGN KEY ([StatusReasonId]) REFERENCES [patient].[PatientStatusTypes]([Id]),
)
