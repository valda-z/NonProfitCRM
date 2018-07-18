CREATE TABLE [dbo].[CompanyStatus]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(128) NOT NULL
)
GO

INSERT INTO [dbo].[CompanyStatus] ([Id], [Name]) VALUES (0, N'Aktivní')
INSERT INTO [dbo].[CompanyStatus] ([Id], [Name]) VALUES (1, N'Lead')
INSERT INTO [dbo].[CompanyStatus] ([Id], [Name]) VALUES (-1, N'Neaktivní')
GO
