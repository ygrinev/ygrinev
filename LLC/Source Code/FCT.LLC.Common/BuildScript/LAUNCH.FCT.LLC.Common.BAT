@ECHO OFF
REM XSLT VERSION 2012-08-13 11:00

REM set a full path to a solution folder
SET local_path_solution="D:\TFS\PS Release\Application Code\LLC\20160127\Source Code\FCT.LLC.Common"

REM set a full path to a common binaries folder
SET local_path_binaries="D:\TFS\PS Release\Application Code\LLC\20160127\Binaries"

REM set a full path to a package folder (a parent folder of 'IS Project', 'Release', and 'Resource')
SET local_path_apd=""

CALL .\FCT.LLC.Common_DIR\MAIN.FCT.LLC.Common.BAT %local_path_solution% %local_path_binaries%