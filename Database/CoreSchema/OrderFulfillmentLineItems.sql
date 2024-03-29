﻿CREATE TABLE [core].[OrderFulfillmentLineItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderHeaderId] INT NOT NULL, 
    [OrderLineItemId] INT NOT NULL, 
    [NetSuiteWarehouseId] INT NULL, 
    [NetSuiteDispatchRecordId] INT NULL, 
    [NetSuiteCustomerId] INT NULL,
    [PatientUUID] UNIQUEIDENTIFIER NULL, 
    [NetSuiteOrderId] INT NULL, 
    [NetSuiteItemId] INT NULL, 
    [ItemId] INT NOT NULL, 
    [Quantity] INT NULL, 
    [AssetTag] VARCHAR(MAX) NULL,  
    [LotNumber] VARCHAR(MAX) NULL, 
    [SerialNumber] VARCHAR(MAX) NULL, 
    [OrderType] VARCHAR(MAX) NULL, 
    [DeliveredStatus] VARCHAR(MAX) NULL,  
    [IsFulfilmentConfirmed] BIT NOT NULL DEFAULT 0,
    [FulfillmentStartDateTime] DATETIME2 NULL,
    [FulfillmentStartAtLatitude] DECIMAL(11, 8) NULL, 
    [FulfillmentStartAtLongitude] DECIMAL(11, 8) NULL,
    [FulfillmentEndDateTime] DATETIME2 NULL, 
    [FulfillmentEndAtLatitude] DECIMAL(11, 8) NULL, 
    [FulfillmentEndAtLongitude] DECIMAL(11, 8) NULL,
    [FulfilledByVehicleId] INT NULL, 
    [FulfilledByDriverId] INT NULL, 
    [CreatedDateTime] DATETIME2 NULL, 
    [CreatedByUserId] INT NULL   , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL ,
    [IsWebportalFulfillment] BIT NULL, 
    CONSTRAINT [FK_OrderLineItemFulfilment_ToOrderLineItems] FOREIGN KEY ([OrderLineItemId]) REFERENCES [core].[OrderLineItems]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_ToOrderHeaders] FOREIGN KEY ([OrderHeaderId]) REFERENCES [core].[OrderHeaders]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_Drivers] FOREIGN KEY ([FulfilledByDriverId]) REFERENCES [core].[Drivers]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_Vehicles] FOREIGN KEY ([FulfilledByVehicleId]) REFERENCES [core].[Sites]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_OrderLineItemFulfilment_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
)
