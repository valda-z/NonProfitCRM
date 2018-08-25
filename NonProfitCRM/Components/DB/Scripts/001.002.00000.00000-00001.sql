CREATE VIEW ViewStatEventType
AS
SELECT 
	e.Id, ISNULL(MIN(t.Tag),'') AS Tag, e.DateOfEvent
	FROM Event e
	LEFT OUTER JOIN Tag2Event te ON te.EventId=e.Id
	LEFT OUTER JOIN Tag t ON t.Id=te.TagId
	GROUP BY e.Id, e.DateOfEvent

