
/* update owner where status is completed passed, closed or no issue) */
update task set assigned_to = [owner] where status in  ( 3, 6, 7, 9)



insert into log(user_id, task_id, operation, type, logtime, description)

select created_by, task_id, 'CREATE', 'TASK', created_on, 'Task created'
from task t
inner join dbo.[user] u 
on t.created_by = u.user_id


insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)
select created_by,  task_id, 
'UPDATE','STATUS',CREATED_ON,'Status changed',
o.name old, o2.name as news
 from task_hist t
inner join optionset o
on o.code = SUBSTRING(status,1,1)
and o.option_type = 'STATUS'
inner join optionset o2
on o2.code = SUBSTRING(status,3,1)
and o2.option_type = 'STATUS'
inner join [user] u 
on t.created_by = u.user_id
where [status] like '%>%'





insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)

select created_by,  task_id, 
'UPDATE','PRIORITY',CREATED_ON,'Priority changed',
o.name old, o2.name as news
 from task_hist t
inner join optionset o
on o.code = SUBSTRING(priority,1,1)
and o.option_type = 'PRIORITY'
inner join optionset o2
on o2.code = SUBSTRING(priority,3,1)
and o2.option_type = 'PRIORITY'
inner join [user] u 
on t.created_by = u.user_id
where [priority] like '%>%'

insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)

select created_by,  task_id, 
'UPDATE','COMPLEXITY',CREATED_ON,'Complexity changed',
o.name old, o2.name as news

 from task_hist t
inner join optionset o
on o.code = SUBSTRING(complexity,1,1)
and o.option_type = 'COMPLEXITY'
inner join optionset o2
on o2.code = SUBSTRING(complexity,3,1)
and o2.option_type = 'COMPLEXITY'
inner join [user] u 
on t.created_by = u.user_id
where T.[complexity] like '%>%'



insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)

select created_by,  task_id, 
'UPDATE','SEVERITY',CREATED_ON,'Severity changed',
o.name old, o2.name as news

 from task_hist t
inner join optionset o
on o.code = SUBSTRING(severity,1,1)
and o.option_type = 'SEVERITY'
inner join optionset o2
on o2.code = SUBSTRING(severity,3,1)
and o2.option_type = 'SEVERITY'
inner join [user] u 
on t.created_by = u.user_id
where T.[severity] like '%>%'



/* update owner set to creator */
/* alter table task add [owner] int not null default 0 */

update task set [owner] = created_by