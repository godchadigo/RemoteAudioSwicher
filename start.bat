@echo off

set "programName=RemoteAudioSwicher.exe"

tasklist | find /i "%programName%" > nul
if errorlevel 1 (
    echo %programName% running..
	start RemoteAudioSwicher.exe
) else (
    echo %programName% is running.. 
)

start "MyNamedCmd" cmd /k python %CD%\PythonPanel\PythonControlPanel.py
