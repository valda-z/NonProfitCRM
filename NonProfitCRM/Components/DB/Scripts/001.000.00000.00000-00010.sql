CREATE TABLE [dbo].[CompanySub]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CompanyId] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL,
    [CountryId] INT NOT NULL, 
    [RegionId] INT NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(255) NULL, 
    [Note] NTEXT NULL, 
	[Deleted] BIT NOT NULL DEFAULT 0,
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Updated] DATETIME NOT NULL,
    CONSTRAINT [FK_CompanySub_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id]),
    CONSTRAINT [FK_CompanySub_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id]),
    CONSTRAINT [FK_CompanySub_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)

GO

CREATE INDEX [IX_CompanySub_Name] ON [dbo].[CompanySub] ([Name])

GO

CREATE INDEX [IX_CompanySub_Company] ON [dbo].[CompanySub] ([CompanyId])

GO

CREATE TABLE [dbo].[Tag2Company]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [TagId] INT NOT NULL, 
    [CompanyId] INT NOT NULL, 
    CONSTRAINT [FK_Tag2Company_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]),
    CONSTRAINT [FK_Tag2Company_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id])
)

GO

CREATE INDEX [IX_Tag2Company_Tag] ON [dbo].[Tag2Company] ([TagId])

GO

CREATE INDEX [IX_Tag2Company_Company] ON [dbo].[Tag2Company] ([CompanyId])

GO

CREATE TABLE [dbo].[Tag2NonProfitOrg]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [TagId] INT NOT NULL, 
    [NonProfitOrgId] INT NOT NULL, 
    CONSTRAINT [FK_Tag2NonProfitOrg_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]),
    CONSTRAINT [FK_Tag2NonProfitOrg_NonProfitOrg] FOREIGN KEY ([NonProfitOrgId]) REFERENCES [NonProfitOrg]([Id])
)

GO

CREATE INDEX [IX_Tag2NonProfitOrg_Tag] ON [dbo].[Tag2NonProfitOrg] ([TagId])

GO

CREATE INDEX [IX_Tag2NonProfitOrg_NonProfitOrg] ON [dbo].[Tag2NonProfitOrg] ([NonProfitOrgId])

GO
