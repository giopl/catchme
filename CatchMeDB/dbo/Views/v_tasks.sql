
CREATE view [dbo].[v_tasks] as

select T.project_id, T.task_id, T.initiator, 

T.complexity as complexity_code, oc.name complexity,
T.[type] as type_code, ot.name [type], t.status status_code, os.name [status],
t.priority priority_code , OP.name [priority],
creator.username  created_by, creator.firstname creator_fn, creator.lastname  creator_ln, cast(t.created_on as date) created_on,
assignee.username assigned_to, assignee.firstname assignee_fn, assignee.lastname assignee_ln, cast(t.updated_on as date) updated_on,
t.due_date


 from task t

inner join optionset oc
on ( t.complexity = oc.code
and oc.option_type = 'COMPLEXITY'
)

left join optionset ot
on (t.[type] = ot.code
and ((ot.option_type)) = 'TYPE'
)

left join optionset os
on (t.[status] = os.code
and ((os.option_type)) = 'STATUS'
)

left join optionset op
on (t.[priority] = op.code
and ((op.option_type)) = 'PRIORITY'
)


inner join [user] creator
on t.created_by = creator.user_id

left join [user] assignee
on t.assigned_to = assignee.user_id

where t.state <> 1