@echo off

echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\lib\psake\psake.ps1 BuildScript.ps1 -framework '3.5' net35"
echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\lib\psake\psake.ps1 BuildScript.ps1 -framework '4.0' %*"