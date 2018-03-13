CREATE TABLE [dbo].[CatchMe_Exception] (
    [CodeCatch]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Date]             DATETIME2 (7)  NOT NULL,
    [ApplicationId]    INT            NULL,
    [UrgenceLevel]     SMALLINT       NULL,
    [Method]           NVARCHAR (250) NULL,
    [sourceFilePath]   NVARCHAR (500) NULL,
    [sourceLineNumber] INT            DEFAULT ((0)) NOT NULL,
    [CurrentPath]      NVARCHAR (500) NULL,
    [ProcessName]      NVARCHAR (250) NULL,
    [ComputerName]     NVARCHAR (250) NULL,
    [OsVersion]        NVARCHAR (100) NULL,
    [OsServicePack]    NVARCHAR (100) NULL,
    [OsPlatform]       NVARCHAR (100) NULL,
    [Login]            NVARCHAR (100) NULL,
    [SID]              VARBINARY (85) NULL,
    [Program]          NVARCHAR (100) NULL,
    [Version]          NVARCHAR (100) NULL,
    [Path]             NVARCHAR (500) NULL,
    [Masquer]          BIT            DEFAULT ((0)) NOT NULL,
    [Supprimer]        BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CodeCatch] ASC)
);

