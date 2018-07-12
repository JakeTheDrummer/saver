INSERT INTO
	saver.goal	(Name, Description, Target, UserId, StatusId, IsDefault)
	VALUES		(@Name, @Description, @Target, @UserId, @StatusId, @IsDefault);

SELECT Id, Name, Description, Target, UserId, StatusId as Status, IsDefault FROM saver.goal WHERE Id = LAST_INSERT_ID();