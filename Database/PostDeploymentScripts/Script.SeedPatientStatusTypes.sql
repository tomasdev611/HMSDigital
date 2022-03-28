DECLARE @PatientStatusTypesList TABLE (Id int, Name VARCHAR(MAX), Color Varchar(50))
INSERT INTO @PatientStatusTypesList VALUES
			   (1, 'Discharged', null),
			   (2, 'Deceased', null),
			   (3, 'Active', 'green'),
			   (4, 'Inactive', 'red'),
			   (5, 'Not needed', null),
			   (6, 'Blank', 'grey'),
			   (7, 'Pending Active', 'yellow'),
			   (8, 'Pending', 'yellow'),
			   (9, 'Duplicate', null)

UPDATE b SET b.Name = a.Name , b.Color = a.Color
from @PatientStatusTypesList a 
	left join [patient].[PatientStatusTypes]   b 
	on a.Id = b.Id
	where b.Name <> a.Name or ((b.Color is null and a.Color is not null) or b.Color <> a.Color);

INSERT INTO [patient].[PatientStatusTypes]              
Select a.Id, a.Name, a.Color from @PatientStatusTypesList a 
	left join [patient].[PatientStatusTypes]   b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null