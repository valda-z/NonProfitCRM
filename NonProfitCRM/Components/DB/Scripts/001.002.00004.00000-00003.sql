﻿ALTER VIEW [dbo].[ViewEventList]
	AS 
	SELECT 
		e.Id, e.DateOfEvent, e.Capacity, e.[Name],
		e.ContactCompanyName, e.ContactCompanyPhone, e.ContactCompanyEmail, e.ContactCompanyNote,
		e.ContactNonProfitOrgName, e.ContactNonProfitOrgPhone, e.ContactNonProfitOrgEmail, 
		e.Deleted, e.Updated, e.UpdatedBy,
		e.CompanyId, c.Name as CompanyName,
		e.NonProfitOrgId, n.Name as NonProfitOrgName,
		e.Closed, e.Insurance, et.[Name] as EventTypeName
		FROM [Event] e
		INNER JOIN [Company] c ON c.Id=e.CompanyId
		INNER JOIN [NonProfitOrg] n ON n.Id=e.NonProfitOrgId
		INNER JOIN [EventType] et ON et.Id=e.TypeId