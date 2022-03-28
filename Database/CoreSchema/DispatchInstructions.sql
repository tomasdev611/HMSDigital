﻿CREATE TABLE [core].[DispatchInstructions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [VehicleId] INT NOT NULL, 
    [SequenceNumber] INT NULL,
    [OrderHeaderId] INT NULL,
    [TransferRequestId] INT NULL,
    [DispatchStartDateTime] DATETIME2 NULL, 
    [DispatchEndDateTime] DATETIME2 NULL,
    [CreatedByUserId] INT NOT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NOT NULL,
    CONSTRAINT [AK_OrderHeader] UNIQUE ([OrderHeaderId]),
    CONSTRAINT [FK_DispatchInstructions_To_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [core].[Sites]([Id]), 
    CONSTRAINT [FK_DispatchInstructions_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_DispatchInstructions_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_DispatchInstructions_OrderHeaders] FOREIGN KEY ([OrderHeaderId]) REFERENCES [core].[OrderHeaders]([Id]),
    CONSTRAINT [FK_DispatchInstructions_TransferReqeusts] FOREIGN KEY ([TransferRequestId]) REFERENCES [core].[ItemTransferRequests]([Id]),
)
