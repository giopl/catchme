﻿CREATE TABLE [dbo].[task] (
    [task_id]     INT            IDENTITY (100, 1) NOT NULL,
    [project_id]  INT            NOT NULL,
    [status]      INT            NULL,
    [title]       NVARCHAR (500) NULL,
    [description] NVARCHAR (MAX) NULL,
    [initiator]   NVARCHAR (100) NULL,
    [complexity]  INT            NULL,
    [due_date]    DATE           NULL,
    [type]        INT            NULL,
    [severity]    INT            NULL,
    [priority]    INT            NULL,
    [created_by]  INT            NOT NULL,
    [created_on]  DATETIME       DEFAULT (getdate()) NULL,
    [assigned_to] INT            NULL,
    [updated_on]  DATETIME       DEFAULT (getdate()) NULL,
    [state]       INT            DEFAULT ((0)) NOT NULL,
    [owner]       INT            DEFAULT ((0)) NOT NULL,
    [updated_by]  INT            DEFAULT ((100)) NOT NULL,
    PRIMARY KEY CLUSTERED ([task_id] ASC),
    CONSTRAINT [FK_task_assigned_user_id] FOREIGN KEY ([assigned_to]) REFERENCES [dbo].[user] ([user_id]),
    CONSTRAINT [FK_task_project_id] FOREIGN KEY ([project_id]) REFERENCES [dbo].[project] ([project_id]),
    CONSTRAINT [FK_task_updated_by_user_id] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[user] ([user_id]),
    CONSTRAINT [FK_task_user_id] FOREIGN KEY ([created_by]) REFERENCES [dbo].[user] ([user_id])
);























