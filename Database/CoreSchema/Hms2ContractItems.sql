CREATE TABLE [core].[Hms2ContractItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Hms2ContractItemId] INT NOT NULL, 
    [Hms2ContractId] INT NOT NULL, 
    [Hms2ItemId] INT NOT NULL, 
    [ContractId] INT NOT NULL, 
    [IsPerDiem] BIT NOT NULL, 
    [RentalPrice] DECIMAL(8, 2) NULL, 
    [SalePrice] DECIMAL(8, 2) NULL, 
    [ShowOnOrderScreen] BIT NOT NULL, 
    [NoApprovalRequired] BIT NOT NULL, 
    [ItemId] INT NULL,
    CONSTRAINT [FK_Hms2ContractItems_Hms2Contracts] FOREIGN KEY ([ContractId]) REFERENCES [core].[Hms2Contracts]([Id]),
    CONSTRAINT [FK_Hms2ContractItems_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id])
)
