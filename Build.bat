@ECHO off

IF NOT "%1%"=="" GOTO BUILD
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe WikiPlex.msbuild /p:Configuration=Debug /t:FullBuild
GOTO END

:BUILD
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe WikiPlex.msbuild /p:Configuration=Debug /t:%1%

:END