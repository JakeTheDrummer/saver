SELECT 
	id,
    target,
    description,
    dateMet
FROM
	saver.milestone
WHERE
	goalId = @GoalId
ORDER BY 
	id ASC;