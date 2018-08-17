ALTER VIEW [dbo].[ViewEventList]
	AS 
	SELECT 
		e.Id, e.DateOfEvent, e.Capacity, e.[Name],
		e.ContactCompanyName, e.ContactCompanyPhone, e.ContactCompanyEmail,
		e.ContactNonProfitOrgName, e.ContactNonProfitOrgPhone, e.ContactNonProfitOrgEmail, 
		e.Deleted, e.Updated, e.UpdatedBy,
		e.CompanyId, c.Name as CompanyName,
		e.NonProfitOrgId, n.Name as NonProfitOrgName,
		e.Closed, e.Insurance
		FROM [Event] e
		INNER JOIN [Company] c ON c.Id=e.CompanyId
		INNER JOIN [NonProfitOrg] n ON n.Id=e.NonProfitOrgId

GO

ALTER VIEW [dbo].[ViewCompanyList]
	AS 
	SELECT 
		org.Id, org.[Name], org.IdentificationNumber,
		org.Contact1Name, org.Contact1Phone, org.Contact1Email,
		org.Contact2Name, org.Contact2Phone, org.Contact2Email,
		org.Www, org.CountryId, org.RegionId, org.City, org.[Address],
		org.StatusId, org.Deleted, org.Updated, org.UpdatedBy, org.Insurance,
		c.[Name] as CountryName,
		r.[Name] as RegionName
		FROM [Company] org
		INNER JOIN [Country] c ON c.Id=org.CountryId
		INNER JOIN [Region] r ON r.Id=org.RegionId

GO
