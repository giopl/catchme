CREATE TABLE [dbo].[task_hist] (
    [task_hist_id] INT            IDENTITY (100, 1) NOT NULL,
    [task_id]      INT            NOT NULL,
    [project_id]   INT            NULL,
    [status]       NVARCHAR (10)  NULL,
    [title]        NVARCHAR (500) NULL,
    [description]  NVARCHAR (MAX) NULL,
    [initiator]    NVARCHAR (100) NULL,
    [complexity]   NVARCHAR (10)  NULL,
    [due_date]     NVARCHAR (30)  NULL,
    [type]         NVARCHAR (10)  NULL,
    [severity]     NVARCHAR (10)  NULL,
    [priority]     NVARCHAR (10)  NULL,
    [created_by]   INT            NULL,
    [created_on]   DATETIME       NULL,
    [hist_status]  INT            NULL,
    [firstname]    NVARCHAR (50)  NULL,
    [fullname]     NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([task_hist_id] ASC),
    CONSTRAINT [FK_task_hist_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id])
);

















