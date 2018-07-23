ALTER TABLE [dbo].[Event]
    ADD [Closed] DATETIME NULL;
GO

CREATE NONCLUSTERED INDEX [IX_Event_Closed] ON [dbo].[Event]
(
	[Closed] ASC
)
GO

ALTER VIEW [dbo].[ViewEventList]
	AS 
	SELECT 
		e.Id, e.DateOfEvent, e.Capacity,
		e.ContactCompanyName, e.ContactCompanyPhone, e.ContactCompanyEmail,
		e.ContactNonProfitOrgName, e.ContactNonProfitOrgPhone, e.ContactNonProfitOrgEmail, 
		e.Deleted, e.Updated, e.UpdatedBy,
		e.CompanyId, c.Name as CompanyName,
		e.NonProfitOrgId, n.Name as NonProfitOrgName,
		e.Closed
		FROM [Event] e
		INNER JOIN [Company] c ON c.Id=e.CompanyId
		INNER JOIN [NonProfitOrg] n ON n.Id=e.NonProfitOrgId
