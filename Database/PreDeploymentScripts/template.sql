DECLARE  @scriptName_@datePrefix@ varchar(max) ='@name@';

DECLARE @scriptRunCount_@datePrefix@ int = 0;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PrePostDeploymentScriptRuns')
BEGIN
    SET @scriptRunCount_@datePrefix@  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_@datePrefix@)
	
	IF @scriptRunCount_@datePrefix@ = 0
	BEGIN
		INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_@datePrefix@);
	END
END

IF @scriptRunCount_@datePrefix@ = 0
BEGIN

    /* Enter your script here */
	/*TODO : INCLUDE THIS FILE TO DATABASE.CSPROJ*/
    
END