[![Build status](https://dev.azure.com/hospice-source/HMSDigital/_apis/build/status/HMSDigital-API-CI)](https://dev.azure.com/hospice-source/HMSDigital/_build/latest?definitionId=1)

# HMSDigital 

## Steps to update database:

1. Open **Database** project
2. Make changes to database schema using SSDT tools
3. Create database **HMSDigital** if not present.
4. Update local schema by running the "Database" project
5. Open command prompt
6. To update model and database context, run following command:
    #### Core.Data  
    ```bash
    dotnet ef dbcontext scaffold "<Connection String for local database HMSDigital>" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir ./ --startup-project Core.API --project Core.Data --schema core --no-pluralize --no-onconfiguring -f 
    ```
    
    #### for Patient.Data
    ```bash
    dotnet ef dbcontext scaffold "<Connection String for local database Patients>" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir ./ --startup-project Patient.API --project Patient.Data --context HMSPatientContext --schema patient --no-pluralize --no-onconfiguring -f
    ```
    
    #### for Notification.Data
    ```bash
    dotnet ef dbcontext scaffold "<Connection String for local database HMSDigital>" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir ./ --startup-project Notification.API --project Notification.Data --context HMSNotificationContext --schema core --no-pluralize --no-onconfiguring -f
    ```

7. Remove connection string from Generated DataContext file

