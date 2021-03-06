﻿CREATE VIEW [dbo].[ViewNonProfitOrgList]
	AS 
	SELECT 
		org.Id, org.[Name], org.IdentificationNumber,
		org.Contact1Name, org.Contact1Phone, org.Contact1Email,
		org.Contact2Name, org.Contact2Phone, org.Contact2Email,
		org.Www, org.CountryId, org.RegionId, org.City, org.[Address],
		org.Capacity, org.Deleted, org.Updated, org.UpdatedBy,
		c.[Name] as CountryName,
		r.[Name] as RegionName
		FROM [NonProfitOrg] org
		INNER JOIN [Country] c ON c.Id=org.CountryId
		INNER JOIN [Region] r ON r.Id=org.RegionId
