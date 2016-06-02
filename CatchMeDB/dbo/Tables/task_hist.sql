CREATE TABLE [dbo].[task_hist] (
    [task_hist_id] INT            IDENTITY (100, 1) NOT NULL,
    [task_id]      INT            NULL,
    [project_id]   INT            NULL,
    [status]       INT            NULL,
    [test_status]  INT            NULL,
    [title]        NVARCHAR (500) NULL,
    [description]  NVARCHAR (MAX) NULL,
    [initiator]    NVARCHAR (100) NULL,
    [complexity]   INT            NULL,
    [due_date]     DATE           NULL,
    [type]         INT            NULL,
    [severity]     INT            NULL,
    [priority]     INT            NULL,
    [created_by]   INT            NULL,
    [created_on]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([task_hist_id] ASC)
);





