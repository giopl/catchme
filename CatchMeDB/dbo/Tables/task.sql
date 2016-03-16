CREATE TABLE [dbo].[task] (
    [task_id]     INT            IDENTITY (100, 1) NOT NULL,
    [project_id]  INT            NOT NULL,
    [status]      INT            NULL,
    [test_status] INT            NULL,
    [title]       NVARCHAR (500) NULL,
    [description] NVARCHAR (MAX) NULL,
    [creator]     NVARCHAR (30)  NOT NULL,
    [complexity]  INT            NULL,
    [due_date]    DATE           NULL,
    PRIMARY KEY CLUSTERED ([task_id] ASC),
    CONSTRAINT [FK_task_project_id] FOREIGN KEY ([project_id]) REFERENCES [dbo].[project] ([project_id])
);

