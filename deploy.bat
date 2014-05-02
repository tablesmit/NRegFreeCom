:: Builds,  copies, versions and packages release libs
call nuget.bat
call build.bat
:: TODO: call updated Nuspec according version of Version Info
call copyfiles.bat
call pack.bat
::TODO: validate and upload to Nuget