CREATE TABLE [dbo].[Country] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Region] (
    [Id]        INT            NOT NULL,
    [CountryId] INT            NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Region_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id])
);
GO

INSERT INTO [dbo].[Country] ([Id], [Name]) VALUES (203, N'Česká republika')
INSERT INTO [dbo].[Country] ([Id], [Name]) VALUES (703, N'Slovensko')
GO

INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20301, 203, N'Hlavní město Praha')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20302, 203, N'Středočeský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20303, 203, N'Jihočeský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20304, 203, N'Plzeňský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20305, 203, N'Karlovarský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20306, 203, N'Ústecký')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20307, 203, N'Liberecký')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20308, 203, N'Královéhradecký')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20309, 203, N'Pardubický')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20310, 203, N'Olomoucký')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20311, 203, N'Moravskoslezský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20312, 203, N'Jihomoravský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20313, 203, N'Zlínský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (20314, 203, N'Kraj Vysočina')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70301, 703, N'Banskobystrický')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70302, 703, N'Bratislavský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70303, 703, N'Košický')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70304, 703, N'Nitranský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70305, 703, N'Prešovský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70306, 703, N'Trenčínský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70307, 703, N'Trnavský')
INSERT INTO [dbo].[Region] ([Id], [CountryId], [Name]) VALUES (70308, 703, N'Žilinský')
GO
