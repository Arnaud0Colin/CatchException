CREATE TABLE [dbo].[CatchMe_Exception_Detail] (
    [CodeCatch]  BIGINT         NOT NULL,
    [Code]       BIGINT         NOT NULL,
    [Message]    NVARCHAR (MAX) NULL,
    [HResult]    INT            NULL,
    [Source]     NVARCHAR (300) NULL,
	[Exception]  NVARCHAR (300) NULL,
    [StackTrace] NVARCHAR (MAX) NULL,
    [HelpLink]   NVARCHAR (500) NULL,
    [TargetSite] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CatchMe_Exception_Detail] PRIMARY KEY CLUSTERED ([CodeCatch] ASC, [Code] ASC),
    CONSTRAINT [FK_CatchMe_Exception_Detail_ToException] FOREIGN KEY ([CodeCatch]) REFERENCES [dbo].[CatchMe_Exception] ([CodeCatch])
);

