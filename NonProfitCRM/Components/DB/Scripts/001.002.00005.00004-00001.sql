ALTER TABLE dbo.Event ADD
	SelfOrganised bit NOT NULL CONSTRAINT DF_Event_SelfOrganised DEFAULT 0
