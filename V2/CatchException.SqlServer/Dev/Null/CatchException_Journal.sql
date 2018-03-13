CREATE TABLE [dbo].[CatchException_Journal] (
    [Ordinateur]    NVARCHAR (50)   COLLATE French_CS_AI NOT NULL,
    [ApplicationID] INT             NOT NULL,
    [Date]          DATETIME        NOT NULL,
    [Exception]     VARBINARY (MAX) NOT NULL,
    [DataType]      INT             NOT NULL,
    [Hide]          BIT             NULL,
    PRIMARY KEY CLUSTERED ([Ordinateur] ASC, [ApplicationID] ASC, [Date] ASC)
);

