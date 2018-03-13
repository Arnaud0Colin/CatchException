CREATE TABLE [dbo].[CatchMe_Exception_Detail] (
    [CodeCatch]  BIGINT         NOT NULL,
    [Code]       BIGINT         NOT NULL,
    [Message]    NVARCHAR (500) NULL,
    [HResult]    INT            NULL,
    [Source]     NVARCHAR (300) NULL,
    [StackTrace] NVARCHAR (500) NULL,
    [HelpLink]   NVARCHAR (500) NULL,
    [TargetSite] NVARCHAR (500) NULL,
    CONSTRAINT [PK_CatchMe_Exception_Detail] PRIMARY KEY CLUSTERED ([CodeCatch] ASC, [Code] ASC),
    CONSTRAINT [FK_CatchMe_Exception_Detail_ToException] FOREIGN KEY ([CodeCatch]) REFERENCES [dbo].[CatchMe_Exception] ([CodeCatch])
);

