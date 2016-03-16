CREATE TABLE [dbo].[employee] (
    [user_id]        VARCHAR (30)  NOT NULL,
    [empl_num]       CHAR (10)     NOT NULL,
    [title_code]     CHAR (3)      NULL,
    [fam_name]       VARCHAR (50)  NULL,
    [given_name]     VARCHAR (100) NULL,
    [common_name]    VARCHAR (50)  NULL,
    [gender]         CHAR (1)      NULL,
    [position_code]  INT           NULL,
    [position_title] VARCHAR (100) NULL,
    [bu_code]        VARCHAR (12)  NULL,
    [bu_name]        VARCHAR (100) NULL,
    [tel_num]        VARCHAR (50)  NULL,
    [is_active]      CHAR (1)      NULL,
    [team]           VARCHAR (100) NULL
);

