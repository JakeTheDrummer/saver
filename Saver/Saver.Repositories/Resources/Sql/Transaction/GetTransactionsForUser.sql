SELECT 
	`id`,
	`amount`,
	`sourcegoalid`,
	`targetgoalid`,
	`timestamp`
FROM 
	saver.transaction trans
	INNER JOIN saver.goal goal ON goal.Id = trans.sourcegoalid OR goal.Id = trans.targetgoalid
WHERE 
	goal.userid = @UserId
ORDER BY
	trans.Id ASC;