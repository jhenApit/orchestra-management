CREATE PROC spOrchestra_PopulateDatabase

AS
BEGIN
/*** Drop Concert Table if it Exist ***/
IF EXISTS(SELECT * FROM Concert)
	ALTER TABLE Player
	DROP CONSTRAINT IF EXISTS FK_PlayerSection

	ALTER TABLE Player
	DROP CONSTRAINT IF EXISTS FK_PlayerConcert

    ALTER TABLE Player
	DROP CONSTRAINT IF EXISTS FK_PlayerInstrument

	ALTER TABLE Instrument
	DROP CONSTRAINT IF EXISTS FK_InstrumentSection
    TRUNCATE TABLE Concert

/*** Drop Player Table if it Exist ***/
IF EXISTS(SELECT * FROM Player)
    TRUNCATE TABLE Player

/*** Drop Instrument Table if it Exist ***/
IF EXISTS(SELECT * FROM Instrument)
    TRUNCATE TABLE Instrument

/*** Drop Section Table if it Exist ***/
IF EXISTS(SELECT * FROM Section)
    TRUNCATE TABLE Section

/** Insert section data **/
SET IDENTITY_INSERT [dbo].[Section] ON
INSERT [dbo].[Section] ([Id], [Name], [Principal], [Assistant])
VALUES 
    (1, 'String', 'Jhon Ambrad Gwapo', 'Queen Riza Montayre'),
    (2, 'Wind', 'Kerr Labajo', 'Hareram Chua'),
    (3, 'Percussion', 'Andre Uy', 'Marc Baguion'),
    (4, 'Brass', 'Christina Berglund', 'Thomas Hardy');
SET IDENTITY_INSERT [dbo].[Section] OFF

/** Insert concert data **/
SET IDENTITY_INSERT [dbo].[Concert] ON
INSERT [dbo].[Concert] ([Id], [Name], [Description], [PerformanceDate])
VALUES 
    (1,'Astana Ballet Symphony Orchestra', 'Premiere of the “Carmen Suite” one-act ballet in Astana Ballet', '10/10/2023'),
    (2,'Tokyo Philharmonic Orchestra', 'Join us at Tokyo Main Stadium at 2PM est', '07/10/2025');
SET IDENTITY_INSERT [dbo].[Concert] OFF

/** Insert player data **/
SET IDENTITY_INSERT [dbo].[Player] ON
INSERT [dbo].[Player] ([Id], [Name], [ConcertId], [SectionId], [InstrumentId])
VALUES 
    (1, 'Marie Florienta', 1, 1, 1),
    (2, 'Louise Gomez', 1, 1, 1),
    (3, 'Castiel Ovidio', 1, 2, 5),
    (4, 'Lole Isotta', 1, 2, 6),
    (5, 'Katerina Zoe', 1, 3, 9),
    (6, 'Garnett Tatianna', 1, 3, 10),
    (7, 'Flavia Rafael', 1, 4, 14),
    (8, 'Rudolf Ester', 1, 4, 15),
    (9, 'Antonio Moreno', 1, 1, 2),
    (10, 'Margaret Mason', 1, 4, 16),
    (11, 'Rei Guanson', 1, 3, 11),
    (12, 'Maddy Jones', 1, 2, 8),
    (13, 'Joseph Sabello', 1, 2, 7),
    (14, 'James Reid', 1, 1, 3),
    (15, 'Vice Ganda', 2, 4, 14),
    (16, 'Kim Chiu', 2, 1, 1),
    (17, 'Oliver Queen', 2, 1, 4),
    (18, 'Peter Parker', 2, 2, 6),
    (19, 'Clark Kent', 2, 3, 13),
    (20, 'Stephen Strange', 2, 2, 5),
    (21, 'Harry Potter', 2, 4, 15),
    (22, 'Ron Weasley', 2, 3, 12),
    (23, 'Branimira Gary', 1, 2, 8),
    (24, 'Leonardo Thom', 1, 3, 10),
    (25, 'Andrej Samu', 1, 4, 16),
    (26, 'Zinoviya Eduarda', 2, 1, 2),
    (27, 'Juan Carlos', 2, 1, 2),
    (28, 'Mary Jo', 2, 2, 7),
    (29, 'Van Marley', 2, 2, 8),
    (30, 'Hermoine Granger', 2, 3, 11),
    (31, 'Badang Loshi', 2, 3, 13),
    (32, 'Keshi Wong', 2, 4, 14),
    (33, 'Michelle Jane', 2, 4, 15),
    (34, 'Gwen Stacy', 1, 1, 3),
    (35, 'Ned Leeds', 2, 1, 4),
    (36, 'Mohini Dasi', 1, 2, 5),
    (37, 'Arjuna Das', 2, 2, 6),
    (38, 'Tom Cruise', 1, 1, 3),
    (39, 'Jason Statham', 2, 2, 7),
    (40, 'Bruce Lee', 2, 4, 16),
    (41, 'Jackie Chan', 2, 3, 11),
    (42, 'Doja Cat', 1, 4, 14),
    (43, 'Nicki Minaj', 1, 3, 12),
    (44, 'Ed Sheeran', 2, 1, 3),
    (45, 'Troye Sivan', 1, 1, 2),
    (46, 'Taylor Swift', 1, 2, 6),
    (47, 'Lauv Spy', 2, 1, 4),
    (48, 'Lany Beast', 2, 2, 7);
SET IDENTITY_INSERT [dbo].[Player] OFF

/** Insert instrument data **/
SET IDENTITY_INSERT [dbo].[Instrument] ON
INSERT [dbo].[Instrument] ([Id], [Name], [SectionId]) 
VALUES 
    (1, 'Violins', 1),
    (2, 'Violas', 1),
    (3, 'Cello', 1),
    (4, 'Bass', 1),
    (5, 'Flutes', 2),
    (6, 'Clarinets', 2),
    (7, 'Piccolos', 2),
    (8, 'Bassons', 2),
    (9, 'Timpani', 3),
    (10, 'Marimba', 3),
    (11, 'Drums', 3),
    (12, 'Cymbals', 3),
    (13, 'Glockenspiel', 3),
    (14, 'Trumpets', 4),
    (15, 'Trombones', 4),
    (16, 'French Horns', 4);
SET IDENTITY_INSERT [dbo].[Instrument] OFF

END