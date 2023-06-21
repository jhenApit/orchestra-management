USE [OrchestraDb];
GO

-- Insert data into User table
INSERT INTO [Users] ([Username], [Email], [Password], [Role])
VALUES
	('userAdmin', 'admin@gmail.com', '$2a$11$DQcN4aYh0j43u2Xn8YicmesEZxq6255cf8fuCV5HTZu1vBo3QufeG', 1), --admin
	('sebastian', 'sebastian@gmail.com', '$2a$11$8r4XGvLkq7GDIDW4wJM1cergDEK025PSyv7MjZuvBjvb8UBYSvkkS', 1), --sebastianlove
	('justino', 'justino@gmail.com', '$2a$11$FZCps9lyP2TvE162o4PZWOLDGSYcP6o3sDGcqVOjYpCqK.SpUEOxC', 1), --justinokalipay
	('userPlayer', 'player@gmail.com', '$2a$11$eYnTNxsIXPGvmosNvyq9heCm78ONKcW7GmmkwFnWc2c5.AtTndWg2', 2), --player
	('johndave', 'johndave@gmail.com', '$2a$11$8xDSijrSfFBMASCdo1Sm/e4U.D0gU6vZ5Q/8Dvnxg8f9hHlhf6/Zq', 2), --johndave123
	('karina', 'karina@gmail.com', '$2a$11$z4Ry4L7rQ8VVpwXYxKoAsOT3/.eucD0IRAPKwE5VyX8M8TFn7fRj6', 2); --karinacutie

GO

-- Insert data into Conductor table
INSERT INTO [Conductor] ([Name], [UserId])
VALUES
	('User Admin', 1),
	('Sebastian Rodrigo', 2),
	('Justino Kalipay', 3);
GO
								
-- Insert data into Orchestra table
INSERT INTO [Orchestra] ([Name], [ConductorId], [Description])
VALUES
	('Manila Symphony Orchestra', 1, 'Manila Symphony Orchestra'),
	('Philippine Philharmonic Orchestra', 1, 'Philippine Philharmonic Orchestra'),
	('ABS-CBN Philharmonic Orchestra', 2, 'ABS-CBN Philharmonic Orchestra'),
	('UP Symphony Orchestra', 2, 'UP Symphony Orchestra'),
	('UST Symphony Orchestra', 3, 'UST Symphony Orchestra'),
	('Classic Youth Orchestra', 3, 'Classic Youth Orchestra');

GO

-- Insert data into Concert table
INSERT INTO [Concert] ([Name], [Description], [OrchestraId], [PerformanceDate])
VALUES
	('Meta Gala', 'Meta Gala is a description', 1, '2023-12-23 00:00:00'),
	('Concierto', 'Concierto is a description', 2, '2023-12-23 00:00:00');

-- Insert data into Section table
INSERT INTO [Section] ([Name])
VALUES
	('Strings'),
	('Woodwinds'),
	('Brass'),
	('Percussion'),
	('Keyboard');

-- Insert data into Instrument table
INSERT INTO [Instrument] ([Name], [SectionId])
VALUES
	('Violin', 1),
	('Violas', 1),
	('Cellos', 1),
	('Double Basses', 1),
	('Flutes', 2),
	('Oboes', 2),
	('Clarinets', 2),
	('Bassoons', 2),
	('Piccolo', 2),
	('Trumpets', 3),
	('French Horns', 3),
	('Trombones', 3),
	('Tubas', 3),
	('Timpani', 4),
	('Snare Drum', 4),
	('Bass Drum', 4),
	('Cymbals', 4),
	('Triangle', 4),
	('Tambourine', 4),
	('Glockenspiel', 4),
	('Marimba', 4),
	('Piano', 5),
	('Harp', 5);


-- Insert data into Player table
INSERT INTO [Player] ([Name], [UserId])
VALUES
	('User Player', 4),
	('John Dave', 5),
	('Karina Jelato', 6);

GO