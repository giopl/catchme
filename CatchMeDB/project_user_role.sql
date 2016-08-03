insert into project_user_role (user_id, project_id, role)
select user_id, project_id, 0 
from project_user