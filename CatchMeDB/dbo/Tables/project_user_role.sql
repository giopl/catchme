CREATE TABLE [dbo].[project_user_role] (
    [user_id]    INT NOT NULL,
    [project_id] INT NOT NULL,
    [role]       INT DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([user_id] ASC, [project_id] ASC),
    CONSTRAINT [FK_project_user_role_project_id] FOREIGN KEY ([project_id]) REFERENCES [dbo].[project] ([project_id]),
    CONSTRAINT [FK_project_user_role_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[user] ([user_id])
);



