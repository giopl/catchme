CREATE TABLE [dbo].[optionset] (
    [optionset_id] INT           IDENTITY (100, 1) NOT NULL,
    [option_type]  NVARCHAR (30) NOT NULL,
    [code]         INT           NOT NULL,
    [name]         NVARCHAR (60) NULL,
    PRIMARY KEY CLUSTERED ([option_type] ASC, [code] ASC)
);

