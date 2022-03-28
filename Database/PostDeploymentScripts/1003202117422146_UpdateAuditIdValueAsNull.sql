DECLARE  @scriptName_1003202117422146 varchar(max) ='1003202117422146_UpdateAuditIdValueAsNull.sql';
DECLARE @scriptRunCount_1003202117422146 int = 0;
SET @scriptRunCount_1003202117422146  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_1003202117422146)
IF @scriptRunCount_1003202117422146 = 0
BEGIN
    
    INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_1003202117422146);
    /* Enter your script here */
	UPDATE patient.PatientNoteAuditLog SET AuditId = Null;
    UPDATE patient.PatientDetailAuditLog SET AuditId = Null;
    UPDATE core.HospiceAuditLog SET AuditId = Null;
    UPDATE core.HospiceLocationAuditLog SET AuditId = Null;
    UPDATE core.InventoryAuditLog SET AuditId = Null;
    UPDATE core.ItemAuditLog SET AuditId = Null;
    UPDATE core.OrderHeaderAuditLog SET AuditId = Null;
    UPDATE core.OrderLineItemAuditLog SET AuditId = Null;
    UPDATE core.PatientInventoryAuditLog SET AuditId = Null;
    UPDATE core.SiteAuditLog SET AuditId = Null;
    UPDATE core.UserAuditLog SET AuditId = Null;
    
END
