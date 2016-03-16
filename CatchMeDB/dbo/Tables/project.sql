CREATE TABLE [dbo].[project] (
    [project_id]  INT            IDENTITY (100, 1) NOT NULL,
    [name]        NVARCHAR (100) NULL,
    [description] NVARCHAR (300) NULL,
    [is_active]   BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([project_id] ASC)
);

