

 MERGE [task] AS tgt
     USING (
		select task_id, max(created_on) updated_on
		 from task_hist
		group by task_id
		) AS src (task_id, updated_on)  
     ON tgt.task_id = src.task_id
    WHEN MATCHED 
	        THEN UPDATE SET tgt.updated_on  = src.updated_on;
    