@echo off
echo abc | powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\3rdParty\psake\psake.ps1 BuildScript.ps1 -framework '3.5' %*"
