DECLARE  @scriptName_@datePrefix@ varchar(max) ='@name@';

DECLARE @scriptRunCount_@datePrefix@ int = 0;

SET @scriptRunCount_@datePrefix@  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_@datePrefix@)

IF @scriptRunCount_@datePrefix@ = 0
BEGIN
    
    INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_@datePrefix@);

    /* Enter your script here */
	/*TODO : INCLUDE THIS FILE TO DATABASE.CSPROJ*/
    
END