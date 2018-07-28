UPDATE
	saver.milestone
SET
	Target = @Target,
	DateMet = @DateMet,
	Description = @Description
WHERE
	Id = @Id;

SELECT 
	id,
    target,
    description,
    dateMet
FROM
	saver.milestone
WHERE
	Id = @Id;