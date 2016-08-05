
CREATE view [dbo].[v_visit] as
select 
isNull(l.user_id,-999) user_id,

u.firstname, u.lastname, max(l.logtime) lastlogin, max(operation) operation , max([type]) [type]
from log l
inner join [user] u
on l.user_id = u.user_id

inner join (
select user_id, max(logtime) logtime
from log l
group by user_id
) maxlogin

on maxlogin.user_id = l.user_id
and maxlogin.logtime = l.logtime
where l.user_id > 0
group by l.user_id, u.firstname, u.lastname