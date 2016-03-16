CREATE TABLE [dbo].[category] (
    [category_id]   INT           IDENTITY (100, 1) NOT NULL,
    [description]   NVARCHAR (60) NULL,
    [display_order] INT           NULL,
    PRIMARY KEY CLUSTERED ([category_id] ASC)
);

