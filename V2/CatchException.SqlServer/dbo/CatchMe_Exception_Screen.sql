CREATE TABLE [dbo].[CatchMe_Exception_Screen] (
    [FileKey]   UNIQUEIDENTIFIER           DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [CodeCatch] BIGINT                     NOT NULL,
    [Screen]    VARBINARY (MAX)  FILESTREAM NOT NULL,
    [Ext]       NVARCHAR (50)              NOT NULL,
    [Mime]      NVARCHAR (100)             NOT NULL,
    CONSTRAINT [PK_CatchMe_Exception_Screen] PRIMARY KEY CLUSTERED ([FileKey] ASC),
    CONSTRAINT [FK_CatchMe_Exception_Screen_ToException] FOREIGN KEY ([CodeCatch]) REFERENCES [dbo].[CatchMe_Exception] ([CodeCatch]),
    UNIQUE NONCLUSTERED ([FileKey] ASC)
) FILESTREAM_ON [SysApp];

