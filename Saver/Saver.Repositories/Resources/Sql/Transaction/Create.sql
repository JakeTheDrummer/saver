INSERT INTO `saver`.`transaction`
(`amount`,`sourcegoalid`,`targetgoalid`,`timestamp`)
VALUES
(@Amount, @SourceGoalId, @TargetGoalId, @Timestamp);

SELECT 
	`id`,
	`amount`,
	`sourcegoalid`,
	`targetgoalid`,
	`timestamp`
FROM 
	saver.transaction 
WHERE 
	id = LAST_INSERT_ID();