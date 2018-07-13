UPDATE
	saver.goal
SET
	Name = @Name,
	Description = @Description,
	Target = @Target,
	StatusId = @StatusId,
	IsDefault = @IsDefault
WHERE
	Id = @Id;

SELECT Id, Name, Description, Target, UserId, StatusId as Status, IsDefault FROM saver.goal WHERE Id = @Id;