CREATE TEMPORARY TABLE IF NOT EXISTS removedMilestone AS
SELECT 
	id,
    target,
    description,
    dateMet
FROM
	saver.milestone
WHERE
	id = @Id;

DELETE FROM saver.milestone WHERE Id = @Id;

SELECT * FROM removedMilestone;
DROP TABLE removedMilestone;