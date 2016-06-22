CREATE TABLE [dbo].[comment] (
    [comment_id]  INT            IDENTITY (100, 1) NOT NULL,
    [task_id]     INT            NOT NULL,
    [user_id]     INT            NOT NULL,
    [title]       NVARCHAR (500) NULL,
    [description] NVARCHAR (MAX) NULL,
    [created_on]  DATETIME       DEFAULT (getdate()) NULL,
    [is_disabled] BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([comment_id] ASC),
    CONSTRAINT [FK_comment_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id]),
    CONSTRAINT [FK_comment_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[user] ([user_id])
);







