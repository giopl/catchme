
CREATE view [dbo].[v_history] as

select isNull(l.log_id,-999) log_id,  l.user_id, u.username, u.firstname, u.lastname, l.task_id, 
l.operation, l.type, l.description, l.logtime, l.old_val, 

case when [type] = 'TASK' and operation = 'CREATE' then 'New' else l.new_val end new_val,

case when [type] = 'TASK' and operation='CREATE' or type = 'STATUS' then 1 else 0 end StatusChange

,unew.firstname fname_new
,unew.lastname lname_new
from [log] l
inner join [user] u
on l.user_id = u.user_id

left join [user] unew
on unew.user_id = case when [type] in( 'ASSIGNEE','OWNER') then cast(l.new_val as int) else 0 end

union

select isnull((n.notification_id * 7)+10000,-999), n.sender_id, u.username, u.firstname, u.lastname, n.task_id, 'NOTIFY', 'MAIL', 'Notification Sent', n.sent_on, 
null,null,
0, null, null

from [notification] n
inner join [user] u
on n.sender_id = u.user_id