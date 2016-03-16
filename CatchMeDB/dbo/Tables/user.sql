CREATE TABLE [dbo].[user] (
    [username]       NVARCHAR (30) NOT NULL,
    [firstname]      NVARCHAR (60) NULL,
    [lastname]       NVARCHAR (60) NULL,
    [job_title]      NVARCHAR (60) NULL,
    [team]           NVARCHAR (60) NULL,
    [role]           INT           DEFAULT ((0)) NOT NULL,
    [num_logins]     INT           NULL,
    [is_active]      BIT           DEFAULT ((1)) NOT NULL,
    [email]          NVARCHAR (60) NULL,
    [active_project] INT           NULL,
    PRIMARY KEY CLUSTERED ([username] ASC)
);

