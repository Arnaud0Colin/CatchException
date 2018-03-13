CREATE TABLE [dbo].[CatchMe_Filter] (
    [Codefilter]    BIGINT         NOT NULL,
    [ApplicationId] INT            NULL,
    [Program]       NVARCHAR (100) NULL,
    [ProcessName]   NVARCHAR (250) NULL,
    [ComputerName]  NVARCHAR (250) NULL,
    [CodeAction]    INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Codefilter] ASC)
);

