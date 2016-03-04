set configuration=%1
if '%configuration%'=='' set configuration=Release
set folder=artifacts\nuget\%configuration%
if '%folder%'=='' set folder=Release
for /f %%G IN ('dir /b %folder%\*.symbols.nupkg') do nuget push "%folder%\%%G"
pause