create view backlog as



select opened.fulldate, opened.sum_opened, closed.sum_closed
from
(
select t1.fulldate, t1.opened, SUM(t2.opened) as sum_opened
from (
select count(c.created_on) opened, d.fulldate
from [date] d
left join  v_tasks c 
on d.fulldate = c.created_on
where d.fulldate between (select min(created_on) from v_tasks) and (select max(updated_on) from v_tasks)
group by d.fulldate

)
 t1
inner join (
select count(c.created_on) opened, d.fulldate
from [date] d
left join  v_tasks c 
on d.fulldate = c.created_on
where d.fulldate between (select min(created_on) from v_tasks) and (select max(updated_on) from v_tasks)
group by d.fulldate

) t2 

on t1.fulldate >= t2.fulldate
group by t1.fulldate, t1.opened
--order by t1.fulldate

) opened


inner join
(
select t1.fulldate, t1.closed, SUM(t2.closed) as sum_closed
from (
select count(c.updated_on) closed, d.fulldate
from [date] d
left join  v_tasks c 
on (d.fulldate = c.updated_on
and status_code = 9)
where d.fulldate between (select min(created_on) from v_tasks) and (select max(updated_on) from v_tasks)

group by d.fulldate

)
 t1
inner join (
select count(c.updated_on) closed, d.fulldate
from [date] d
left join  v_tasks c 
on (d.fulldate = c.updated_on
and status_code = 9)
where d.fulldate between (select min(created_on) from v_tasks) and (select max(updated_on) from v_tasks)

group by d.fulldate


) t2 

on t1.fulldate >= t2.fulldate
group by t1.fulldate, t1.closed
--order by t1.fulldate

) closed
on opened.fulldate = closed.fulldate