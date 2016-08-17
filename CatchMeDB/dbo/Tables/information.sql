CREATE TABLE [dbo].[information] (
    [information_id] INT            IDENTITY (100, 1) NOT NULL,
    [project_id]     INT            NOT NULL,
    [title]          NVARCHAR (500) NULL,
    [description]    NVARCHAR (MAX) NULL,
    [importance]     INT            NULL,
    [created_by]     INT            NULL,
    [created_on]     DATETIME       DEFAULT (getdate()) NULL,
    [updated_by]     INT            DEFAULT ((100)) NOT NULL,
    [updated_on]     DATETIME       DEFAULT (getdate()) NULL,
    [state]          INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([information_id] ASC),
    CONSTRAINT [FK_information_project_id] FOREIGN KEY ([project_id]) REFERENCES [dbo].[project] ([project_id]),
    CONSTRAINT [FK_information_updated_by_user_id] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[user] ([user_id]),
    CONSTRAINT [FK_information_user_id] FOREIGN KEY ([created_by]) REFERENCES [dbo].[user] ([user_id])
);

