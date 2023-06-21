CREATE PROCEDURE [dbo].[spPlayer_GetPlayerById]
	@id int
AS
BEGIN
	SELECT p.Name , s.Name , c.Name , i.Name FROM Player p 
				INNER JOIN Section s ON s.Id = p.SectionId 
				INNER JOIN Instrument i ON i.Id = p.InstrumentId
                INNER JOIN Concert c ON c.Id = p.ConcertId
                WHERE p.Id = @id;
END
