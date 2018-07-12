SELECT 
	Id, 
	Name, 
	Description, 
	Target, 
	UserId, 
	StatusId as Status, 
	IsDefault 
FROM 
	saver.goal
WHERE
	UserId = @UserId
ORDER BY 
	Id ASC;