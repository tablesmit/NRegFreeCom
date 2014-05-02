mkdir nuget\lib\
:: NET 4.0
mkdir nuget\lib\net40\
copy build\Release\NRegFreeCom.dll nuget\lib\net40\
copy build\Release\NRegFreeCom.pdb nuget\lib\net40\
copy build\Release\NRegFreeCom.xml nuget\lib\net40\
copy build\Release\NRegFreeCom.dll.manifest nuget\lib\net40\


:: NET 3.5
mkdir nuget\lib\NET35\
copy build\NET35\Release\NRegFreeCom.dll nuget\lib\NET35\
copy build\NET35\Release\NRegFreeCom.pdb nuget\lib\NET35\
copy build\NET35\Release\NRegFreeCom.xml nuget\lib\NET35\
copy build\NET35\Release\NRegFreeCom.dll.manifest nuget\lib\NET35\

