ALTER VIEW [dbo].[ViewTaskList]
	AS 
SELECT t.[Id]
      ,t.[StatusId]
      ,t.[EntityId]
      ,t.[Entity]
      ,t.[DueDate]
      ,t.[Description]
      ,t.[Note]
      ,t.[AssignedTo]
      ,t.[UpdatedBy]
      ,t.[Updated]
	  ,ts.[Name] AS StatusName
	  ,ts.[Color] AS StatusColor
	  ,c.[Name] AS EntityName
  FROM Task t
  INNER JOIN TaskStatus ts ON ts.Id=t.StatusId 
  INNER JOIN Company c ON c.Id=t.EntityId AND t.Entity='Company'
UNION ALL
SELECT t.[Id]
      ,t.[StatusId]
      ,t.[EntityId]
      ,t.[Entity]
      ,t.[DueDate]
      ,t.[Description]
      ,t.[Note]
      ,t.[AssignedTo]
      ,t.[UpdatedBy]
      ,t.[Updated]
	  ,ts.[Name] AS StatusName
	  ,ts.[Color] AS StatusColor
	  ,c.[Name] AS EntityName
  FROM Task t
  INNER JOIN TaskStatus ts ON ts.Id=t.StatusId 
  INNER JOIN NonProfitOrg c ON c.Id=t.EntityId AND t.Entity='NonProfitOrg'
UNION ALL
SELECT t.[Id]
      ,t.[StatusId]
      ,t.[EntityId]
      ,t.[Entity]
      ,t.[DueDate]
      ,t.[Description]
      ,t.[Note]
      ,t.[AssignedTo]
      ,t.[UpdatedBy]
      ,t.[Updated]
	  ,ts.[Name] AS StatusName
	  ,ts.[Color] AS StatusColor
	  ,np.[Name] + ' / ' + cm.[Name] AS EntityName
  FROM Task t
  INNER JOIN TaskStatus ts ON ts.Id=t.StatusId 
  INNER JOIN Event c ON c.Id=t.EntityId AND t.Entity='Event'
  INNER JOIN NonProfitOrg np ON np.Id=c.NonProfitOrgId
  INNER JOIN Company cm ON cm.Id=c.CompanyId

  GO
