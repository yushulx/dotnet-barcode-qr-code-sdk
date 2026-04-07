@echo off
setlocal

set MSBUILD="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\bin\MSBuild.exe"
set IISEXPRESS="C:\Program Files\IIS Express\iisexpress.exe"
set PORT=62873

echo [1/3] Restoring NuGet packages...
nuget restore MvcBarcodeQRCodeFramework.csproj -PackagesDirectory packages
if errorlevel 1 (
    echo ERROR: NuGet restore failed.
    pause & exit /b 1
)

echo [2/3] Building project...
%MSBUILD% MvcBarcodeQRCodeFramework.csproj /p:Configuration=Debug /t:Build /nologo /v:minimal
if errorlevel 1 (
    echo ERROR: Build failed.
    pause & exit /b 1
)

echo [3/3] Starting IIS Express on http://localhost:%PORT%/
taskkill /f /im iisexpress.exe >nul 2>&1
set APPPATH=%~dp0
set APPPATH=%APPPATH:~0,-1%
%IISEXPRESS% /path:"%APPPATH%" /port:%PORT%

endlocal
