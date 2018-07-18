CREATE VIEW [dbo].[ViewUserProfileList]
	AS SELECT 
		up.UserId,
		up.UserName,
		up.Deleted,
		up.Email,
		up.FirstName,
		up.LastName,
		up.GroupId,
		up.TimeZone,
		g.Name AS GroupName
		FROM [UserProfile] up
		LEFT OUTER JOIN [Group] g ON g.Id = up.GroupId
GO
