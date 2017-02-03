del iBUpdateCustom.zip

rmdir /S /Q iBUpdateCustom
mkdir iBUpdateCustom

copy ..\TEST\BOIEIBUS.dll .\iBUpdateCustom
copy ..\TEST\BFIEIBUS.dll .\iBUpdateCustom
copy ..\TEST\BHIEIBUS.dll .\iBUpdateCustom

copy ..\TEST\BOIEIBUS.pdb .\iBUpdateCustom
copy ..\TEST\BFIEIBUS.pdb .\iBUpdateCustom
copy ..\TEST\BHIEIBUS.pdb .\iBUpdateCustom


"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "iBUpdateCustom.zip" .\iBUpdateCustom
 
rem rmdir /S /Q iBUpdateCustom
