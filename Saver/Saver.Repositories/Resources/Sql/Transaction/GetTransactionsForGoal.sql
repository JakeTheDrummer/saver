SELECT 
	`id`,
	`amount`,
	`sourcegoalid`,
	`targetgoalid`,
	`timestamp`
FROM 
	saver.transaction 
WHERE 
	sourcegoalid = @GoalId OR targetgoalid = @GoalId;