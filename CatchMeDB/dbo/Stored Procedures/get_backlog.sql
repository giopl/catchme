-- =============================================
-- Author:		Giovanni L'Etourdi
-- Create date: 01-Jul-2016
-- Description:	returns the backlog i.e opened vs closed per date for a project
-- =============================================
CREATE PROCEDURE get_backlog 
	-- Add the parameters for the stored procedure here
	@project_id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	

select opened.fulldate, opened.sum_opened, closed.sum_closed
from
(
select t1.fulldate, t1.opened, SUM(t2.opened) as sum_opened
from (
select count(c.created_on) opened, d.fulldate
from [date] d
left join  
(
select created_on

from v_tasks 
where project_id = @project_id
) c

on d.fulldate = c.created_on
--and project_id = @project_id
where d.fulldate between (select min(created_on) from v_tasks where project_id = @project_id) and (select max(updated_on) from v_tasks where project_id = @project_id)
group by d.fulldate

)
 t1
inner join (
select count(c.created_on) opened, d.fulldate
from [date] d
left join  
(
select created_on

from v_tasks 
where project_id = @project_id
) c
on d.fulldate = c.created_on
--and project_id = @project_id
where d.fulldate between (select min(created_on) from v_tasks where project_id = @project_id) and (select max(updated_on) from v_tasks where project_id = @project_id)
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

left join  
(
select updated_on, status_code
from v_tasks 
where project_id = @project_id
) c


 
on (d.fulldate = c.updated_on
and status_code = 9 
--and project_id = @project_id
)
where d.fulldate between (select min(created_on) from v_tasks where project_id = @project_id) and (select max(updated_on) from v_tasks where project_id = @project_id)

group by d.fulldate

)
 t1
inner join (
select count(c.updated_on) closed, d.fulldate
from [date] d

left join  (
select updated_on, status_code

from v_tasks 
where project_id = @project_id
) c 
on (d.fulldate = c.updated_on
and status_code = 9 
--and project_id = @project_id
)
where d.fulldate between (select min(created_on) from v_tasks where project_id = @project_id) and (select max(updated_on) from v_tasks where project_id = @project_id)

group by d.fulldate


) t2 

on t1.fulldate >= t2.fulldate
group by t1.fulldate, t1.closed
--order by t1.fulldate

) closed
on opened.fulldate = closed.fulldate


END