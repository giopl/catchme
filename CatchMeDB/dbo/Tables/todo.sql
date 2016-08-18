CREATE TABLE [dbo].[todo] (
    [todo_id]     INT            IDENTITY (100, 1) NOT NULL,
    [user_id]     INT            NOT NULL,
    [title]       INT            NOT NULL,
    [description] NVARCHAR (MAX) NULL,
    [created_on]  DATETIME       DEFAULT (getdate()) NULL,
    [updated_on]  DATETIME       DEFAULT (getdate()) NULL,
    [status]      INT            NULL,
    [priority]    INT            NULL
);

