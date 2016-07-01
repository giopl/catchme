
CREATE PROCEDURE [dbo].[PopulateDates]
	-- Add the parameters for the stored procedure here
@StartDate		DATETIME = '2016-01-01',
@NumberOfDays	INT = 3000
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

 
	INSERT
	INTO	dbo.date
			(fulldate)
	SELECT	DATEADD(DAY, RowNum, @StartDate) AS DateValue
	FROM	(SELECT	TOP (@NumberOfDays)
					p1.PARAMETER_ID,
					ROW_NUMBER() OVER (ORDER BY p1.PARAMETER_ID) - 1 AS RowNum
			 FROM	sys.all_parameters p1
			 CROSS	JOIN sys.all_parameters p2) NumberTable
END