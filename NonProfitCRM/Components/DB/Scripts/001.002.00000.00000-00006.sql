CREATE VIEW ViewStatEventNonProfitOrgType
AS
SELECT 
	e.Id, t.Name AS Tag, e.DateOfEvent
	FROM Event e
	INNER JOIN NonProfitOrg n ON n.Id = e.NonProfitOrgId
	INNER JOIN NonProfitOrgType t ON t.Id=n.TypeId
	WHERE e.Deleted=0

