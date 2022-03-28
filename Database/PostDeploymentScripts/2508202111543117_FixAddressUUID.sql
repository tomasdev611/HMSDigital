DECLARE  @scriptName_2508202111543117 varchar(max) ='2508202111543117_FixAddressUUID.sql';
DECLARE @scriptRunCount_2508202111543117 int = 0;
SET @scriptRunCount_2508202111543117  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_2508202111543117)
IF @scriptRunCount_2508202111543117 = 0
BEGIN
    
    INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_2508202111543117);
   
    UPDATE patient.Addresses SET AddressUUID = NEWID() WHERE AddressUUID IS NULL OR AddressUUID='00000000-0000-0000-0000-000000000000'

    UPDATE core.Addresses SET AddressUUID = NEWID() WHERE AddressUUID IS NULL OR AddressUUID='00000000-0000-0000-0000-000000000000'
    
END
