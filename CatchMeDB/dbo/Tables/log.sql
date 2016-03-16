CREATE TABLE [dbo].[log] (
    [log_id]      INT             IDENTITY (100, 1) NOT NULL,
    [username]    NVARCHAR (30)   NOT NULL,
    [task_id]     INT             NOT NULL,
    [operation]   NVARCHAR (60)   NULL,
    [type]        NVARCHAR (60)   NULL,
    [logtime]     DATETIME        DEFAULT (getdate()) NULL,
    [description] NVARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([log_id] ASC),
    CONSTRAINT [FK_log_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id]),
    CONSTRAINT [FK_log_username] FOREIGN KEY ([username]) REFERENCES [dbo].[user] ([username])
);

