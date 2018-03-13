CREATE TABLE [dbo].[CatchMe_Exception_Detail_Data] (
    [CodeCatch] BIGINT         NOT NULL,
    [Code]      BIGINT         NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Type]      NVARCHAR (200) NOT NULL,
    [Value]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CatchMe_Exception_Detail_Data] PRIMARY KEY CLUSTERED ([CodeCatch] ASC, [Code] ASC, [Name] ASC),
    CONSTRAINT [FK_CatchMe_Exception_Detail_Data_ToException] FOREIGN KEY ([CodeCatch]) REFERENCES [dbo].[CatchMe_Exception] ([CodeCatch]),
    CONSTRAINT [FK_CatchMe_Exception_Detail_Data_ToException_Detail] FOREIGN KEY ([CodeCatch], [Code]) REFERENCES [dbo].[CatchMe_Exception_Detail] ([CodeCatch], [Code])
);

