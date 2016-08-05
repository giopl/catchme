CREATE view [dbo].[v_status] as

select 
isnull(t.project_id,-999) project_id,
isnull(t.assigned_to,-999) user_id, u.firstname, u.lastname,
sum(case [status] when 0 then 1 else 0 end) newtask,
sum(case [status] when 1 then 1 else 0 end) [action], 

sum(case [status] when 2 then 1 else 0 end) investigation, 

sum(case [status] when 3 then 1 else 0 end) completed, 

sum(case [status] when 4 then 1 else 0 end) onhold, 
sum(case [status] when 5 then 1 else 0 end) problem, 
sum(case [status] when 6 then 1 else 0 end) no_issue, 
sum(case [status] when 7 then 1 else 0 end) passed, 
sum(case [status] when 8 then 1 else 0 end) failed,
sum(case when status < 9 then 1 else 0 end) total_open,
max(coalesce(C.[count],0)) closed,
sum(case when status < 9 then 1 else 0 end) + max(coalesce(C.[count],0)) total
 

from task t

inner join [user] u
on t.assigned_to = u.user_id

left join 
(

select  user_id, t.project_id, count(1) [count]
from log l

inner join
(
select task_id, max(logtime) maxlogtime
from log
where operation = 'UPDATE' AND [type] = 'STATUS'
AND (new_val = 'Completed' or new_val='No Issue')
group by task_id
) A
on l.task_id = A.task_id
and l.logtime = A.maxlogtime

inner join task t
on l.task_id = t.task_id
and t.status = 9
and t.state <> 1

where l.operation = 'UPDATE' AND l.[type] = 'STATUS'
AND (new_val = 'Completed' or new_val = 'No Issue')


group by l.user_id, t.project_id
) C
on C.user_id = t.assigned_to 
and c.project_id = t.project_id
where t.state <>1
group by t.assigned_to, t.project_id, u.firstname, u.lastname