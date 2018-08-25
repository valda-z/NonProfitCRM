ALTER VIEW ViewStatEventType
AS
SELECT 
	e.Id, t.Name AS Tag, e.DateOfEvent
	FROM Event e
	INNER JOIN EventType t ON t.Id=e.TypeId
	WHERE e.Deleted=0

