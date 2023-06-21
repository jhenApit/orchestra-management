CREATE PROCEDURE spConcert_GetConcertById
	@id INT
AS
BEGIN
	DECLARE @players INT = 
	(
		SELECT COUNT(p.ConcertId) 
		FROM Player p LEFT JOIN Concert c 
		ON c.Id = P.ConcertId
		WHERE c.Id = @id
		GROUP BY c.Id
	);
	SELECT *, @players AS Players 
	FROM Concert
	WHERE Concert.Id = @id;
END