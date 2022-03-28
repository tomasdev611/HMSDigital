#!/bin/bash

/opt/sqlpackage/sqlpackage /a:Publish /tsn:. /tdn:HMSDigital /tu:sa /tp:$PASSWORD /sf:/var/opt/mssql/data/Database.dacpac
