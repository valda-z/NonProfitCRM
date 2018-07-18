CREATE TABLE [dbo].[Event]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(255) NOT NULL,
    [DateOfEvent] DATETIME NOT NULL,
    [CompanyId] INT NOT NULL,
    [NonProfitOrgId] INT NOT NULL,
	[Capacity] INT NOT NULL,
    [ContactCompanyName] NVARCHAR(128) NULL, 
    [ContactCompanyPhone] NVARCHAR(50) NULL, 
    [ContactCompanyEmail] NVARCHAR(128) NULL, 
    [ContactCompanyNote] NVARCHAR(1024) NULL,
    [ContactNonProfitOrgName] NVARCHAR(128) NULL, 
    [ContactNonProfitOrgPhone] NVARCHAR(50) NULL, 
    [ContactNonProfitOrgEmail] NVARCHAR(128) NULL, 
    [ContactNonProfitOrgNote] NVARCHAR(1024) NULL,
    [Note] NTEXT NULL,
	[Deleted] BIT NOT NULL DEFAULT 0,
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Updated] DATETIME NOT NULL, 
    CONSTRAINT [FK_Event_NonProfitOrg] FOREIGN KEY ([NonProfitOrgId]) REFERENCES [NonProfitOrg]([Id]), 
    CONSTRAINT [FK_Event_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id])
)

GO

CREATE INDEX [IX_Event_Name] ON [dbo].[Event] ([Name])

GO

CREATE INDEX [IX_Event_DateOfEvent] ON [dbo].[Event] ([DateOfEvent])

GO

CREATE INDEX [IX_Event_CompanyId] ON [dbo].[Event] ([CompanyId])

GO

CREATE INDEX [IX_Event_NonProfitOrgId] ON [dbo].[Event] ([NonProfitOrgId])

GO
