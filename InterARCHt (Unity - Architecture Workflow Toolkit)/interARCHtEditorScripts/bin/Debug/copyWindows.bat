set source=C:\Users\Patrick\Documents\InterARCHtCode\interARCHt\interARCHtEditorScripts\bin\Debug\interARCHt.dll
set destination=C:\Users\Patrick\Documents\interARCHt\Assets\interARCHt
set s=C:\Users\Patrick\Documents\InterARCHtCode\interARCHt\interARCHtEditorScripts\bin\Debug\interARCHtEditorScripts.dll
set d=C:\Users\Patrick\Documents\interARCHt\Assets\interARCHt\editor

xcopy /y %source% %destination%
xcopy /y %s% %d% 