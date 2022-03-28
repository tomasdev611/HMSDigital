CREATE TABLE [core].[HospiceLocationMembers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HospiceLocationId] INT NULL, 
    [HospiceMemberId] INT NULL, 
    [NetSuiteContactId] INT NULL,
    [CanApproveOrder] BIT NULL DEFAULT 0,
    [CreatedDateTime] DATETIME2 NULL, 
    [CreatedByUserId] INT NULL   , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL ,
    CONSTRAINT [FK_HospiceLocationMember_HospiceLocation] FOREIGN KEY ([HospiceLocationId]) REFERENCES [core].[HospiceLocations]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HospiceLocationMember_HospiceMember] FOREIGN KEY ([HospiceMemberId]) REFERENCES [core].[HospiceMember]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_HospiceLocationMember_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_HospiceLocationMember_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
)
