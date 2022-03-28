@echo off
set scriptType=%1
set fileName=%2
set fileDirectory=
IF NOT DEFINED scriptType (
	echo "scriptType(PreDeploy/PostDeploy) is required"
	pause
	exit /b
)


IF %scriptType%==PreDeploy set fileDirectory=PreDeploymentScripts\
IF %scriptType%==PostDeploy set fileDirectory=PostDeploymentScripts\

IF NOT DEFINED fileDirectory (
	echo "scriptType(%scriptType%) is not valid. Valid values: PreDeploy/PostDeploy"
	pause
	exit /b
)

IF NOT DEFINED fileName (
	echo "fileName is required"
	pause
	exit /b
)

set datePrefix=%date%%time%
set datePrefix=%datePrefix:/=%
set datePrefix=%datePrefix:-=%
set datePrefix=%datePrefix::=%
set datePrefix=%datePrefix: =%
set datePrefix=%datePrefix:.=%

set templateFileName=template.sql
set scriptFileName=%datePrefix%_%fileName%.sql


copy "%fileDirectory%%templateFileName%" "%fileDirectory%%scriptFileName%"

echo.>>%fileDirectory%script.%scriptType%ment.sql
echo :r .\%scriptFileName%>>%fileDirectory%script.%scriptType%ment.sql


set "textfile=%scriptFileName%"
set "newfile=%scriptFileName%_temp"


set "search=@name@"
set "replace=%scriptFileName%"
CALL :Replace

set "search=@datePrefix@"
set "replace=%datePrefix%"
CALL :Replace


pause






:Replace

(for /f "delims=" %%i in (%fileDirectory%%textfile%) do (
    set "line=%%i"
    setlocal enabledelayedexpansion
    set "line=!line:%search%=%replace%!"
    echo(!line!
    endlocal
))>"%fileDirectory%%newfile%"

del %fileDirectory%%textfile%
rename %fileDirectory%%newfile%  %textfile%
EXIT /B 0
