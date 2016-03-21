CREATE TABLE [dbo].[task_user] (
    [user_id] INT NOT NULL,
    [task_id] INT NOT NULL,
    [role]    INT NULL,
    PRIMARY KEY CLUSTERED ([user_id] ASC, [task_id] ASC),
    CONSTRAINT [FK_task_user_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id]),
    CONSTRAINT [FK_task_user_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[user] ([user_id])
);

