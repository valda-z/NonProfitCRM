ALTER VIEW [dbo].[ViewEventList]
	AS 
	SELECT 
		e.Id, e.DateOfEvent, e.Capacity, e.[Name],
		e.ContactCompanyName, e.ContactCompanyPhone, e.ContactCompanyEmail,
		e.ContactNonProfitOrgName, e.ContactNonProfitOrgPhone, e.ContactNonProfitOrgEmail, 
		e.Deleted, e.Updated, e.UpdatedBy,
		e.CompanyId, c.Name as CompanyName,
		e.NonProfitOrgId, n.Name as NonProfitOrgName,
		e.Closed, e.Insurance, et.[Name] as EventTypeName
		FROM [Event] e
		INNER JOIN [Company] c ON c.Id=e.CompanyId
		INNER JOIN [NonProfitOrg] n ON n.Id=e.NonProfitOrgId
		INNER JOIN [EventType] et ON et.Id=e.TypeId
GO

ALTER VIEW [dbo].[ViewNonProfitOrgList]
	AS 
	SELECT 
		org.Id, org.[Name], org.IdentificationNumber,
		org.Contact1Name, org.Contact1Phone, org.Contact1Email,
		org.Contact2Name, org.Contact2Phone, org.Contact2Email,
		org.Www, org.CountryId, org.RegionId, org.City, org.[Address],
		org.Capacity, org.Deleted, org.Updated, org.UpdatedBy,
		c.[Name] as CountryName,
		r.[Name] as RegionName,
		ot.[Name] as NonProfitOrgTypeName
		FROM [NonProfitOrg] org
		INNER JOIN [Country] c ON c.Id=org.CountryId
		INNER JOIN [Region] r ON r.Id=org.RegionId
		INNER JOIN [NonProfitOrgType] ot ON ot.Id=org.TypeId
GO

