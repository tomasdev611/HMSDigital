DECLARE  @scriptName_1808202115304871 varchar(max) ='1808202115304871_UpdateInventoryStatus.sql';
DECLARE @scriptRunCount_1808202115304871 int = 0;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PrePostDeploymentScriptRuns')
BEGIN
    SET @scriptRunCount_1808202115304871  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_1808202115304871)
	
	IF @scriptRunCount_1808202115304871 = 0
	BEGIN
		INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_1808202115304871);
	END
END
IF @scriptRunCount_1808202115304871 = 0
BEGIN
    
    UPDATE [core].[Inventory] SET StatusId = 1 WHERE StatusId = 5
    
END
