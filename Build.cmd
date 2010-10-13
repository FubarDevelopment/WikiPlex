@echo off

if (%1)==(ci) goto Build35
if (%1)==(CI) goto Build35
goto Build40
  
:Build35
echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\lib\psake\psake.ps1 BuildScript.ps1 -framework '3.5' CI35"

:Build40
echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\lib\psake\psake.ps1 BuildScript.ps1 -framework '4.0' %*"

:EXIT