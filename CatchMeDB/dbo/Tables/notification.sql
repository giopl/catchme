CREATE TABLE [dbo].[notification] (
    [notification_id] INT             IDENTITY (100, 1) NOT NULL,
    [task_id]         INT             NOT NULL,
    [sender_id]       INT             NOT NULL,
    [sender_name]     NVARCHAR (100)  NULL,
    [recipients]      NVARCHAR (1000) NULL,
    [sent_on]         DATETIME        DEFAULT (getdate()) NULL,
    [recipients_id]   NVARCHAR (500)  NULL,
    [send_to_id]      INT             NULL,
    PRIMARY KEY CLUSTERED ([notification_id] ASC),
    CONSTRAINT [FK_notification_task_id] FOREIGN KEY ([task_id]) REFERENCES [dbo].[task] ([task_id])
);









