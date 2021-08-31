@echo off
if exist dangeric0.bin del dangeric0.bin
if exist dangerick.bin del dangerick.bin
if exist File0.lst del File0.lst
if exist File0.exp del File0.exp
if exist File1.lst del File1.lst
if exist dangerick.rom del dangerick.rom

rem Define ESCchar to use in ANSI escape sequences
rem https://stackoverflow.com/questions/2048509/how-to-echo-with-different-colors-in-the-windows-command-line
for /F "delims=#" %%E in ('"prompt #$E# & for %%E in (1) do rem"') do set "ESCchar=%%E"

@echo on
tools\tasm -85 -b -i File0.asm dangeric0.bin
@if errorlevel 1 goto Failed
@echo off

dir /-c dangeric0.bin|findstr /R /C:"dangeric0.bin"

@echo on
tools\tasm -85 -b -i File1.asm dangerick.bin
@if errorlevel 1 goto Failed
@echo off

dir /-c dangerick.bin|findstr /R /C:"dangerick.bin"


copy /b dangeric0.bin+dangerick.bin dangerick.rom >nul
@if errorlevel 1 goto Failed

dir /-c dangerick.rom|findstr /R /C:"dangerick.rom"

echo %ESCchar%[92mSUCCESS%ESCchar%[0m
exit

:Failed
@echo off
echo %ESCchar%[91mFAILED%ESCchar%[0m
exit /b
