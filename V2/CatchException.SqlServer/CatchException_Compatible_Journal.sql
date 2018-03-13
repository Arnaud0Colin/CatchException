CREATE TABLE [dbo].[CatchException_Compatible_Journal] (
    [Ordinateur]    NVARCHAR (50)   NOT NULL,
    [ApplicationID] INT             NOT NULL,
    [Date]          DATETIME        NOT NULL,
    [Exception]     VARBINARY (MAX) NOT NULL,
    [DataType]      INT             NOT NULL,
    [Hide]          BIT             NULL,
    PRIMARY KEY CLUSTERED ([Ordinateur] ASC, [ApplicationID] ASC, [Date] ASC)
);

