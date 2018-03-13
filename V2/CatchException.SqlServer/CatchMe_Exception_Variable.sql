CREATE TABLE [dbo].[CatchMe_Exception_Variable] (
    [CodeCatch] BIGINT         NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Type]      NVARCHAR (200) NOT NULL,
    [Value]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CatchMe_Exception_Variable] PRIMARY KEY CLUSTERED ([CodeCatch] ASC, [Name] ASC),
    CONSTRAINT [FK_CatchMe_Exception_Variable_ToException] FOREIGN KEY ([CodeCatch]) REFERENCES [dbo].[CatchMe_Exception] ([CodeCatch])
);

