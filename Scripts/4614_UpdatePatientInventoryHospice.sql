UPDATE pi 
SET HospiceId = pd.HospiceId,
	HospiceLocationId = pd.HospiceLocationId
FROM CORE.PatientInventory pi
	JOIN patient.PatientDetails pd ON pi.PatientUUID = pd.UniqueId
WHERE pi.HospiceId IS NULL