@ECHO OFF
REM XSLT VERSION 2012-08-13 11:00

REM set a full path to a solution folder
SET local_path_solution="D:\TFS\EasyFund\EPS\SC\FCT.EPS.Agents"

REM set a full path to a common binaries folder
SET local_path_binaries="D:\TFS\EasyFund\EPS\Binaries"

REM set a full path to a package folder (a parent folder of 'IS Project', 'Release', and 'Resource')
SET local_path_apd="D:\TFS\APD\Packaging\EPS\FCT.EPS.ReceiveAgents\Latest"

CALL .\FCT.EPS.WSP.ReceiveAgents_DIR\MAIN.FCT.EPS.WSP.ReceiveAgents.BAT %local_path_solution% %local_path_binaries% %local_path_apd%