del iBUpdate.zip
rmdir /S /Q iBUpdate
mkdir iBUpdate

copy ..\TEST\BNIEIBUS.dll .\iBUpdate
copy ..\TEST\BEIEIBUS.dll .\iBUpdate
copy ..\TEST\BDIEIBUS.dll .\iBUpdate

copy ..\TEST\BNIEIBUS.pdb .\iBUpdate
copy ..\TEST\BEIEIBUS.pdb .\iBUpdate
copy ..\TEST\BDIEIBUS.pdb .\iBUpdate

copy ..\TEST\ApexNet*.dll .\iBUpdate
copy ..\TEST\ApexNet*.pbd .\iBUpdate

copy ..\TEST\RestSharpApex*.* .\iBUpdate
copy ..\TEST\Ionic*.* .\iBUpdate
copy ..\TEST\AMHelper*.* .\iBUpdate
copy ..\TEST\NewtonsoftApex* .\iBUpdate

copy ..\TEST\iBUpdater.* .\iBUpdate

"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "iBUpdate.zip" .\iBUpdate
 

rmdir /S /Q iBUpdate

del ConnettoreBusiness.zip
rmdir /S /Q ConnettoreBusiness
mkdir ConnettoreBusiness

copy ..\TEST\BNIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BNIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\BEIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BEIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\BDIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BDIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\ApexNet*.dll .\ConnettoreBusiness
copy ..\TEST\ApexNet*.pbd .\ConnettoreBusiness

copy ..\TEST\RestSharpApex*.* .\ConnettoreBusiness
copy ..\TEST\Ionic*.* .\ConnettoreBusiness
copy ..\TEST\AMHelper*.* .\ConnettoreBusiness
copy ..\TEST\NewtonsoftApex* .\ConnettoreBusiness

rem ci sono differenze solo a partire da qui
copy ..\TEST\iBUpdater.* .\ConnettoreBusiness

cd .\ConnettoreBusiness
"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "ConnettoreBusiness.zip" *.*
move .\ConnettoreBusiness.zip ..
cd ..

rmdir /S /Q ConnettoreBusiness
