CREATE TABLE [core].[ItemTransferRequests]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL,  
    [SourceLocationId] INT NOT NULL, 
    [DestinationLocationId] INT NOT NULL, 
    [StatusId] INT NOT NULL,
    [ItemCount] INT NOT NULL DEFAULT 1, 
    [DestinationSiteMemberId] INT NULL, 
    [CreatedByUserId] INT NOT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NOT NULL,
    [AdditionalField1] INT NULL,
    [AdditionalField2] INT NULL, 
    CONSTRAINT [FK_TransferReqeust_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_TransferReqeust_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_TransferRequest_ToTransferRequestStatusTypes] FOREIGN KEY ([StatusId]) REFERENCES [core].[TransferRequestStatusTypes]([Id]), 
    CONSTRAINT [FK_TransferRequest_ToItems] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]), 
    CONSTRAINT [FK_TransferRequests_ToSiteMembers] FOREIGN KEY ([DestinationSiteMemberId]) REFERENCES [core].[SiteMembers]([Id])
)
