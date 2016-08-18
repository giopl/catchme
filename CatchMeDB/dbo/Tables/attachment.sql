CREATE TABLE [dbo].[attachment] (
    [attachment_id]  INT            IDENTITY (100, 1) NOT NULL,
    [task_id]        INT            NOT NULL,
    [user_id]        INT            NOT NULL,
    [filename]       NVARCHAR (200) NULL,
    [mimetype]       NVARCHAR (200) NULL,
    [content_length] INT            NULL,
    [created_on]     DATETIME       DEFAULT (getdate()) NULL,
    [is_disabled]    BIT            DEFAULT ((0)) NOT NULL,
    [filepath]       NVARCHAR (500) NULL,
    [comment_id]     INT            DEFAULT ((-1)) NOT NULL,
    [information_id] INT            DEFAULT ((-1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([attachment_id] ASC),
    CONSTRAINT [FK_attachment_comment_id] FOREIGN KEY ([comment_id]) REFERENCES [dbo].[comment] ([comment_id]),
    CONSTRAINT [FK_attachment_information_id] FOREIGN KEY ([information_id]) REFERENCES [dbo].[information] ([information_id]),
    CONSTRAINT [FK_attachment_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id]),
    CONSTRAINT [FK_attachment_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[user] ([user_id])
);











