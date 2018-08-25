CREATE TABLE [dbo].[EventType] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[NonProfitOrgType] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_NonProfitOrgType] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

INSERT INTO [dbo].[EventType] ([Id], [Name]) VALUES (0, N'*Ostatní')
INSERT INTO [dbo].[EventType] ([Id], [Name]) VALUES (1, N'Manuální')
INSERT INTO [dbo].[EventType] ([Id], [Name]) VALUES (2, N'Expertní')
INSERT INTO [dbo].[EventType] ([Id], [Name]) VALUES (3, N'S Klienty')
GO

INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (0, N'*Ostatní')
INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (1, N'Příroda')
INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (2, N'Zvířata')
INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (3, N'Děti')
INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (4, N'Senioři')
INSERT INTO [dbo].[NonProfitOrgType] ([Id], [Name]) VALUES (5, N'Zdravotně Postižení')
GO
