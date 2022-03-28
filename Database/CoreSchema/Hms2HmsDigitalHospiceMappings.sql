CREATE TABLE [core].[Hms2HmsDigitalHospiceMappings]
(
	[HMS2Id] INT NOT NULL,
	[HMS2Name] VARCHAR(MAX) NULL, 
    [NetSuiteCustomerId] INT NULL, 
    [NetSuiteName] VARCHAR(MAX) NULL, 
    [HospiceId] INT NULL, 
    [HospiceName] VARCHAR(MAX) NULL, 
    [HospiceLocationId] INT NULL, 
    [HospiceLocationName] VARCHAR(MAX) NULL, 
    [DigitalType] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Hms2HmsDigitalHospiceMappings] PRIMARY KEY ([HMS2Id]),
    CONSTRAINT [FK_Hms2HmsDigitalHospiceMappings_Hospices] FOREIGN KEY ([HospiceId]) REFERENCES [core].[Hospices]([Id]),
    CONSTRAINT [FK_Hms2HmsDigitalHospiceMappings_HospiceLocations] FOREIGN KEY ([HospiceLocationId]) REFERENCES [core].[HospiceLocations]([Id])
)
