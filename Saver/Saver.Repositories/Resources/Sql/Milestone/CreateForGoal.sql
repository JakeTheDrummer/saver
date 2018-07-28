INSERT INTO `saver`.`milestone`
(
	`target`,
	`description`,
	`goalid`,
	`datemet`
)
VALUES
(
	@Target,
	@Description,
	@GoalId,
	@DateMet
);


SELECT 
	id,
    target,
    description,
    dateMet
FROM
	saver.milestone
WHERE
	Id = LAST_INSERT_ID();