SELECT 
	`id`,
	`amount`,
	`sourcegoalid`,
	`targetgoalid`,
	`timestamp`
FROM 
	saver.transaction 
WHERE 
	id = @Id;