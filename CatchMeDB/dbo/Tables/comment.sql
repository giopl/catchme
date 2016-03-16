CREATE TABLE [dbo].[comment] (
    [comment_id]  INT            IDENTITY (100, 1) NOT NULL,
    [task_id]     INT            NOT NULL,
    [username]    NVARCHAR (30)  NOT NULL,
    [title]       NVARCHAR (60)  NULL,
    [description] NVARCHAR (MAX) NULL,
    [created_on]  DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([comment_id] ASC),
    CONSTRAINT [FK_comment_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id]),
    CONSTRAINT [FK_comment_username] FOREIGN KEY ([username]) REFERENCES [dbo].[user] ([username])
);

