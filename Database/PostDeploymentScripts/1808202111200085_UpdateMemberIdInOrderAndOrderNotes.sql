DECLARE @scriptName_1808202111200085 varchar(max) ='1808202111200085_UpdateMemberIdInOrderAndOrderNotes.sql'
DECLARE @scriptRunCount_1808202111200085 int = 0
SET @scriptRunCount_1808202111200085  = (SELECT COUNT(*) FROM [core].[PrePostDeploymentScriptRuns] WHERE ScriptName = @scriptName_1808202111200085)
IF @scriptRunCount_1808202111200085 = 0
BEGIN
    
    INSERT INTO [core].[PrePostDeploymentScriptRuns] VALUES (@scriptName_1808202111200085);
   
	/* Update memberId in orderHeaders table */
	UPDATE o SET o.HospiceMemberId = m.Id 
	FROM [core].[OrderHeaders] o 
	INNER JOIN [core].[HospiceMember] m
	ON o.NetSuiteCustomerContactId = m.NetSuiteContactId
	WHERE o.NetSuiteCustomerContactId IS NOT NULL
	  AND o.HospiceMemberId IS NULL;

	UPDATE o SET o.HospiceMemberId = m.HospiceMemberId 
	FROM [core].[OrderHeaders] o 
	INNER JOIN [core].[HospiceLocationMembers] m
	ON o.NetSuiteCustomerContactId = m.NetSuiteContactId
	WHERE o.NetSuiteCustomerContactId IS NOT NULL
	  AND o.HospiceMemberId IS NULL;

	/* Update memberId in orderNotes table */
	UPDATE o SET o.HospiceMemberId = m.Id  
	FROM [core].[OrderNotes] o 
	INNER JOIN [core].[HospiceMember] m
	ON o.NetSuiteContactId = m.NetSuiteContactId
	WHERE o.NetSuiteContactId IS NOT NULL
	  AND o.HospiceMemberId IS NULL;

	UPDATE o SET o.HospiceMemberId = m.HospiceMemberId 
	FROM [core].[OrderNotes] o 
	INNER JOIN [core].[HospiceLocationMembers] m
	ON o.NetSuiteContactId = m.NetSuiteContactId
	WHERE o.NetSuiteContactId IS NOT NULL
	  AND o.HospiceMemberId IS NULL;
    
END
