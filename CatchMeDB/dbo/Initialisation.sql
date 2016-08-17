SET IDENTITY_INSERT  [dbo].[user] ON;

GO

insert into [user] ( user_id, username, firstname, lastname, role, is_active) values
(0,'Unassigned','unassigned','unassigned',1,1);
GO

SET IDENTITY_INSERT  [dbo].[user] OFF;

GO

SET IDENTITY_INSERT  [dbo].[project] ON;
GO

insert into project(project_id, name, description, is_active) values (-1,'system','system dummy record used for FKs', 1);
GO

SET IDENTITY_INSERT  [dbo].[project] OFF;

GO



SET IDENTITY_INSERT  [dbo].[task] ON;
GO

insert into task(task_id, project_id, title, description, state, owner, updated_by) values (-1,-1,'system','system dummy record used for FKs', 0,0,0);
GO

SET IDENTITY_INSERT  [dbo].[task] OFF;

GO


SET IDENTITY_INSERT  [dbo].[comment] ON;
GO

insert into comment(comment_id, task_id, user_id, title, description, is_disabled) values (-1,-1,0,'system','system dummy record used for FKs', 0);
GO

SET IDENTITY_INSERT  [dbo].[comment] OFF;



SET IDENTITY_INSERT  [dbo].[information] ON;

INSERT INTO [dbo].[information]
           (information_id,
		   [project_id]
           ,[title]
           ,[description]
           
           ,[created_by]
           
           ,[updated_by]
           )
     VALUES
           (-1,-1
           ,'system'
           ,'system dummy record used for FKs'
           ,0
           ,0)
GO




SET IDENTITY_INSERT  [dbo].[information] OFF;

GO


--CLEANUP OF LOGS TO USE DUMMY TASK FOR LOGIN
UPDATE LOG set task_id = -1 where operation = 'LOGIN'