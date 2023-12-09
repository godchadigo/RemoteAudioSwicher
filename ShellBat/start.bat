@echo off

set "programName=NAudio.exe"

tasklist | find /i "%programName%" > nul
if errorlevel 1 (
    echo %programName% running..
	start NAudio.exe
) else (
    echo %programName% is running.. 
)

start "MyNamedCmd" cmd /k python %CD%\PythonPanel\PythonControlPanel.py
