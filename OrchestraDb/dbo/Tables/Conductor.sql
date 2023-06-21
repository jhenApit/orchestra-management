﻿CREATE TABLE [dbo].[Conductor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT FK_ConductorUser FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users](Id),
)
