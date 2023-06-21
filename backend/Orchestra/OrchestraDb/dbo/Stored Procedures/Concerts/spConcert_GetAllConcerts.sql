CREATE PROC spConcert_GetAllConcerts

AS
BEGIN
	SELECT c.Id, c.Name, c.Description, c.PerformanceDate, COUNT(p.ConcertId) AS Players
	FROM Concert c
	LEFT JOIN Player p
	ON p.ConcertId = c.Id
	GROUP BY c.Id, c.Name, c.Description, c.PerformanceDate
END