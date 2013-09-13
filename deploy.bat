:: Builds,  copies, versions and packages release libs

call build.bat
:: TODO: call updated Nuget according version of Version Info
call copyfiles.bat
call pack.bat