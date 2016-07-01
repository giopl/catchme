SET IDENTITY_INSERT  [dbo].[user] ON;

GO

insert into [user] ( user_id, username, firstname, lastname, role, is_active) values
(0,'Unassigned','unassigned','unassigned',1,1);
GO

SET IDENTITY_INSERT  [dbo].[user] OFF;

GO
