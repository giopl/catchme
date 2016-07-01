--use CatchMeDB

CREATE view v_figures as 

select 

project_id,
count(1) total,

sum(case when status_code < 9 and getDAte() <= due_Date then 1 else 0 end) [open],
sum(case when status_code < 9 and getDAte() > due_Date then 1 else 0 end) [overdue],


sum(case when updated_on <= due_Date and status_code  = 9 then 1 else 0 end) closed_ontime,

sum(case when updated_on > due_Date and status_code  = 9 then 1 else 0 end) closed_late

from v_tasks
group by project_id