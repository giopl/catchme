CREATE TABLE [dbo].[task_category] (
    [task_id]     INT NOT NULL,
    [category_id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([task_id] ASC, [category_id] ASC),
    CONSTRAINT [FK_task_category_id] FOREIGN KEY ([category_id]) REFERENCES [dbo].[category] ([category_id]),
    CONSTRAINT [FK_task_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id])
);

