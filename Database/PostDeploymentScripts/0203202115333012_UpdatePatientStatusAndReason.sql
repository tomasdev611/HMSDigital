DECLARE  @scriptName_0203202115333012 varchar(max) ='0203202115333012_UpdatePatientStatusAndReason.sql';
DECLARE @scriptRunCount_0203202115333012 int = 0;
SET @scriptRunCount_0203202115333012  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_0203202115333012)
IF @scriptRunCount_0203202115333012 = 0
BEGIN
    
    INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_0203202115333012);
    /* Enter your script here */
	UPDATE patient.PatientDetails SET StatusReasonId = StatusId WHERE StatusId = 1 OR StatusId = 2;
    UPDATE patient.PatientDetails SET StatusId = 4 WHERE StatusId = 1 OR StatusId = 2;
    
END
