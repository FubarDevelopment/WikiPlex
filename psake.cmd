@echo off
powershell -NoProfile -ExecutionPolicy unrestricted -Command "%~dp0\psake.ps1 -framework '4.0' %*"
