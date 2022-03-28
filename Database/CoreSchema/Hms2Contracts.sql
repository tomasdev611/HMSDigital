CREATE TABLE [core].[Hms2Contracts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Hms2ContractId] INT NOT NULL, 
    [ContractName] VARCHAR(MAX) NULL, 
    [ContractNumber] VARCHAR(MAX) NULL, 
    [Hms2HospiceId] INT NULL, 
    [Hms2CustomerId] INT NULL, 
    [PerDiemRate] DECIMAL(8, 2) NULL, 
    [StartDate] DATETIME2 NULL, 
    [EndDate] DATETIME2 NULL, 
    [HospiceId] INT NULL, 
    [HospiceLocationId] INT NULL,
    CONSTRAINT [FK_Hms2Contracts_Hospices] FOREIGN KEY ([HospiceId]) REFERENCES [core].[Hospices]([Id]),
    CONSTRAINT [FK_Hms2Contracts_HospiceLocations] FOREIGN KEY ([HospiceLocationId]) REFERENCES [core].[HospiceLocations]([Id])
)
