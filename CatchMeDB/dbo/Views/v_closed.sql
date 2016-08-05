
CREATE view [dbo].[v_closed] as 

select  isnull(l.user_id,-999) user_id, isnull(t.project_id,-999) project_id, count(1) [count]
from log l

inner join
(
select task_id, max(logtime) maxlogtime
from log
where operation = 'UPDATE' AND [type] = 'STATUS'
AND old_val = 'Completed'
group by task_id
) A
on l.task_id = A.task_id
and l.logtime = A.maxlogtime

inner join task t
on l.task_id = t.task_id
and t.status = 9
and t.state <> 1

where l.operation = 'UPDATE' AND l.[type] = 'STATUS'
AND old_val = 'Completed'
and new_val = 'Passed'

group by l.user_id, t.project_id