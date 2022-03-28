DECLARE @scriptName_08132021192614687 VARCHAR(MAX) = '08132021192614687_UpdatePatientStatusReason.sql';
DECLARE @scriptRunCount_08132021192614687 INT = 0;
SET @scriptRunCount_08132021192614687 = (
	SELECT COUNT(*)
	FROM [core].[PrePostDeploymentScriptRuns]
	WHERE ScriptName = @scriptName_08132021192614687
)
IF @scriptRunCount_08132021192614687 = 0
BEGIN

	DECLARE @PickupReason TABLE (
		  PickupReason VARCHAR(50)
		, PatientUuid UNIQUEIDENTIFIER
		, OrderDate DATETIME
		, OrderHeaderId INT
	)

	DECLARE @PickupReasonByPatient TABLE (
		  PickupReason VARCHAR(50)
		, PickupReasonId INT
		, PatientUuid UNIQUEIDENTIFIER
		, OrderDate DATETIME
	)

	INSERT INTO @PickupReason
	SELECT CASE OH.PickupReason
			   WHEN 'NotNeeded' THEN 'Not Needed'
			   ELSE OH.PickupReason
		   END
		 , OH.PatientUuid
		 , OH.OrderDateTime
		 , OH.Id
	FROM core.OrderHeaders OH
		JOIN patient.PatientDetails PD ON OH.PatientUuid = PD.UniqueId
	WHERE OH.PickupReason IS NOT NULL
		AND OH.PickupReason <> ''
		AND PD.StatusReasonId IS NULL
		AND OH.StatusId <> 17
		AND OH.OrderTypeId = 14

	INSERT INTO @PickupReasonByPatient
	SELECT PR.PickupReason
		 , PST.Id AS PickupReasonId
		 , PatientUuid
		 , OrderDate
	FROM @PickupReason PR
		JOIN patient.PatientStatusTypes PST ON PR.PickupReason = PST.Name
	WHERE PR.OrderHeaderId = (
			SELECT TOP 1 PST2.OrderHeaderId
			FROM @PickupReason PST2
			WHERE PST2.PatientUuid = PR.PatientUuid
			ORDER BY PST2.OrderDate DESC
		)

	MERGE patient.PatientDetails AS TARGET USING @PickupReasonByPatient AS SOURCE
	ON TARGET.UniqueId = SOURCE.PatientUuid
	WHEN MATCHED
		THEN UPDATE
			SET TARGET.StatusReasonId = SOURCE.PickupReasonId
			  , TARGET.StatusChangedDate = SOURCE.OrderDate;

END