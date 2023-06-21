CREATE PROC spConcert_GetPlayersByConcertId
	@ConcertId int
--GetPlayersByConcertId

AS
BEGIN
	SELECT p.Name AS "Players", 
		s.Name AS "Section", 
		i.Name AS "Instrument"
	FROM Concert c 
		INNER JOIN Player p 
		ON p.ConcertId = c.Id
		INNER JOIN Section s
		ON s.Id = p.SectionId
		INNER JOIN Instrument i
		ON i.Id = p.InstrumentId
	WHERE c.Id = @ConcertId
	ORDER BY c.Id, s.Name, i.Name;
END