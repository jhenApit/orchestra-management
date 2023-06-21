CREATE TABLE [dbo].[Enroll]
(
    [PlayerId] INT NOT NULL, 
    [OrchestraId] INT NOT NULL, 
    [SectionId] INT NOT NULL,
    [InstrumentId] INT NOT NULL,
    [Experience] INT NOT NULL,
    [isApproved] INT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Enroll] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [OrchestraId] ASC),
    CONSTRAINT [FK_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Player] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrchestraId] FOREIGN KEY ([OrchestraId]) REFERENCES [dbo].[Orchestra] ([Id]) ON DELETE CASCADE,
)
