@echo off
setlocal

cd "%~dp0"

dotnet tool update docfx --tool-path packages

rem packages\docfx init -q -o Documentation
packages\docfx Documentation\docfx.json

