CREATE TABLE [dbo].[Company]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[StatusId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [IdentificationNumber] NVARCHAR(50) NOT NULL, 
    [Contact1Name] NVARCHAR(128) NULL, 
    [Contact1Phone] NVARCHAR(50) NULL, 
    [Contact1Email] NVARCHAR(128) NULL, 
    [Contact1Note] NVARCHAR(1024) NULL,
    [Contact2Name] NVARCHAR(128) NULL, 
    [Contact2Phone] NVARCHAR(50) NULL, 
    [Contact2Email] NVARCHAR(128) NULL, 
    [Contact2Note] NVARCHAR(1024) NULL, 
    [Www] NVARCHAR(128) NULL, 
    [CountryId] INT NOT NULL, 
    [RegionId] INT NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(255) NULL, 
    [Note] NTEXT NULL, 
	[Deleted] BIT NOT NULL DEFAULT 0,
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Updated] DATETIME NOT NULL,
    CONSTRAINT [FK_Company_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id]),
    CONSTRAINT [FK_Company_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id]),
    CONSTRAINT [FK_Company_Status] FOREIGN KEY ([StatusId]) REFERENCES [CompanyStatus]([Id])
)

GO

CREATE INDEX [IX_Company_Name] ON [dbo].[Company] ([Name])

GO

CREATE UNIQUE INDEX [IX_Company_IdentificationNumber] ON [dbo].[Company] ([IdentificationNumber])

GO

CREATE INDEX [IX_Company_Status] ON [dbo].[Company] ([StatusId], [Name])

GO

CREATE TABLE [dbo].[NonProfitOrg]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [IdentificationNumber] NVARCHAR(50) NOT NULL, 
    [Contact1Name] NVARCHAR(128) NULL, 
    [Contact1Phone] NVARCHAR(50) NULL, 
    [Contact1Email] NVARCHAR(128) NULL, 
    [Contact1Note] NVARCHAR(1024) NULL,
    [Contact2Name] NVARCHAR(128) NULL, 
    [Contact2Phone] NVARCHAR(50) NULL, 
    [Contact2Email] NVARCHAR(128) NULL, 
    [Contact2Note] NVARCHAR(1024) NULL, 
    [Www] NVARCHAR(128) NULL, 
    [CountryId] INT NOT NULL, 
    [RegionId] INT NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(255) NULL,
	[Capacity] INT NOT NULL,
    [Note] NTEXT NULL, 
	[Deleted] BIT NOT NULL DEFAULT 0,
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Updated] DATETIME NOT NULL,
    CONSTRAINT [FK_NonProfitOrg_Country] FOREIGN KEY ([CountryId]) REFERENCES [Country]([Id]),
    CONSTRAINT [FK_NonProfitOrg_Region] FOREIGN KEY ([RegionId]) REFERENCES [Region]([Id])
)

GO

CREATE INDEX [IX_NonProfitOrg_Name] ON [dbo].[NonProfitOrg] ([Name])

GO

CREATE UNIQUE INDEX [IX_NonProfitOrg_IdentificationNumber] ON [dbo].[NonProfitOrg] ([IdentificationNumber])

GO

