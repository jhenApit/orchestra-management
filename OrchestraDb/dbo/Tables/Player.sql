CREATE TABLE [dbo].[Player]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,
	[UserId] INT NOT NULL,
	[SectionId] INT NULL,
	[ConcertId] INT NULL,
	[OrchestraId] INT NULL,
	[InstrumentId] INT NULL,
	[Score] DECIMAL(5,2) NOT NULL DEFAULT 0 CHECK (Score >= 0 AND Score <= 100),
	CONSTRAINT FK_PlayerUser FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users](Id),
	CONSTRAINT FK_PlayerSection FOREIGN KEY ([SectionId]) REFERENCES [dbo].[Section](Id),
	CONSTRAINT FK_PlayerConcert FOREIGN KEY ([ConcertId]) REFERENCES [dbo].[Concert](Id),
	CONSTRAINT FK_PlayerOrchestra FOREIGN KEY ([OrchestraId]) REFERENCES [dbo].[Orchestra](Id),
	CONSTRAINT FK_PlayerInstrument FOREIGN KEY ([InstrumentId]) REFERENCES [dbo].[Instrument](Id)
)