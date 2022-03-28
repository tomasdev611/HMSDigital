
/* Feature Flags Declaration */

DECLARE @F_CrispInAppChat int = 1;
DECLARE @F_FhirPatientFeature int = 2;
DECLARE @F_DriverOptimizationFeature int = 3;
DECLARE @F_ReportPortal int = 4;

DECLARE @FeaturesList TABLE (Id int, Name NVARCHAR(255), IsEnabled BIT)
INSERT INTO @FeaturesList VALUES
			   (@F_CrispInAppChat, 'CrispInAppChat', 0),
			   (@F_FhirPatientFeature, 'FhirPatientFeature', 0),
			   (@F_DriverOptimizationFeature, 'DriverOptimizationFeature', 0),
			   (@F_ReportPortal, 'ReportPortal', 0)

/* seed feature flags */
INSERT INTO [core].[Features]  
SELECT a.Id, a.Name, a.IsEnabled FROM @FeaturesList a 
LEFT JOIN [core].[Features] b 
ON a.Id = b.Id
WHERE b.Id IS NULL