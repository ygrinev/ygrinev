@ECHO OFF
REM XSLT VERSION 2012-08-13 11:00

REM set a full path to a solution folder
SET local_path_solution="D:\TFS\LLC-EF Lite\LLC\Source Code\FCT.LLC.BusinessService"

REM set a full path to a common binaries folder
SET local_path_binaries="D:\TFS\LLC-EF Lite\LLC\Binaries"

REM set a full path to a package folder (a parent folder of 'IS Project', 'Release', and 'Resource')
SET local_path_apd="D:\TFS\APD\Packaging\LLC\FCT.LLC.BusinessService\Version 2.0-LLCLite"

CALL .\FCT.LLC.BusinessService_DIR\MAIN.FCT.LLC.BusinessService.BAT %local_path_solution% %local_path_binaries% %local_path_apd%