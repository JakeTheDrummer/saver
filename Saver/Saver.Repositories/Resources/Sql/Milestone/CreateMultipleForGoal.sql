SET @previousMaximum = (SELECT MAX(Id) FROM saver.milestone WHERE goalid = @GoalId);

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
	Id > @previousMaximum
	AND Id <= LAST_INSERT_ID();