CREATE TABLE [dbo].[Instrument]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [SectionId] INT NOT NULL,   
    CONSTRAINT FK_InstrumentSection FOREIGN KEY ([SectionId]) REFERENCES [dbo].[Section](Id)
)
