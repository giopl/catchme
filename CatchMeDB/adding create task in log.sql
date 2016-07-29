
insert into log(user_id, task_id, operation, type, logtime, description)

select created_by, task_id, 'CREATE', 'TASK', created_on, 'Task created by user ' + u.username 
from task t
inner join dbo.[user] u 
on t.created_by = u.user_id


insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)
select created_by,  task_id, 
'UPDATE','STATUS',CREATED_ON,'Status changed by user ' + u.username,
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
where status like '%>%'




insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)

select created_by,  task_id, 
'UPDATE','PRIORITY',CREATED_ON,'Priority changed by user ' + u.username,
o.name old, o2.name as news
 from task_hist t
inner join optionset o
on o.code = SUBSTRING(status,1,1)
and o.option_type = 'PRIORITY'
inner join optionset o2
on o2.code = SUBSTRING(status,3,1)
and o2.option_type = 'PRIORITY'
inner join [user] u 
on t.created_by = u.user_id
where status like '%>%'

insert into log(user_id, task_id, operation, type, logtime, description, old_val, new_val)

select created_by,  task_id, 
'UPDATE','TYPE',CREATED_ON,'Type changed by user ' + u.username,
o.name old, o2.name as news
 from task_hist t
inner join optionset o
on o.code = SUBSTRING(status,1,1)
and o.option_type = 'TYPE'
inner join optionset o2
on o2.code = SUBSTRING(status,3,1)
and o2.option_type = 'TYPE'
inner join [user] u 
on t.created_by = u.user_id
where status like '%>%'


/* update owner set to creator */
/* alter table task add [owner] int not null default 0 */

update task set [owner] = created_by