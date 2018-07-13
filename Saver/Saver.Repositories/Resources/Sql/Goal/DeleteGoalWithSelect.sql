CREATE TEMPORARY TABLE IF NOT EXISTS removedGoal AS
SELECT Id, Name, Description, Target, UserId, StatusId as Status, IsDefault FROM saver.goal WHERE Id = @Id;

DELETE FROM saver.goal WHERE Id = @Id;

SELECT * FROM removedGoal;
DROP TABLE removedGoal;