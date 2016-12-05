@ECHO OFF
REM XSLT VERSION 2013-04-22 15:24
REM BUILD PROCESS VERSION 1.1.0.1

REM ========== TOOLS ==========

SET tf_path="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\tf.exe"
SET msbuild_path="C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
SET asminfoupdater_path="D:\TFS\Tools\AsmInfoUpdater.exe"
SET consm_path="D:\TFS\Tools\ConsM.exe"
SET copyvalidator_path="D:\TFS\Tools\CopyValidator.exe"
SET iscmdbld_path="C:\Program Files (x86)\InstallShield\2014 SAB\System\IsCmdBld.exe"
SET reportlogger_path="D:\TFS\Tools\ReportLogger.exe"
SET logon_identity_path="D:\TFS\Tools\TestLogon.exe"

SET log_file=".\FCT.LLC.Common_DIR\FCT.LLC.Common.log"
%reportlogger_path% /start /i:file=%log_file% /i:lgtime="Start batch"

REM ==================== VALIDATE PARAMS ====================
REM trim quotes and spaces from the parameters and validate

%reportlogger_path% /i:file=%log_file% /i:lgtime=Parameter local_path_solution: [%1]
ECHO ----- Parameter local_path_solution: [%1] ----- 

FOR /f "useback tokens=*" %%a IN ('%1') DO SET local_path_solution=%%~a
FOR /f "tokens=* delims= " %%a IN ("%local_path_solution%") DO SET local_path_solution=%%a
IF "%local_path_solution%"=="" GOTO VALIDATION_ERROR

%reportlogger_path% /i:file=%log_file% /i:lgtime=Parameter local_path_binaries: [%2]
ECHO ----- Parameter local_path_binaries: [%2] ----- 

FOR /f "useback tokens=*" %%a IN ('%2') DO SET local_path_binaries=%%~a
FOR /f "tokens=* delims= " %%a IN ("%local_path_binaries%") DO SET local_path_binaries=%%a
IF "%local_path_binaries%"=="" GOTO VALIDATION_ERROR

GOTO VALIDATION_OK

:VALIDATION_ERROR
%consm_path% /f:msg="One of the configuration parameters is empty. Check ".\FCT.LLC.Common_DIR\FCT.LLC.Common.log""
GOTO END

:VALIDATION_OK

REM ==================== Clean Folders ====================
rmdir "%local_path_apd%" /s /q
rmdir "%local_path_binaries%" /s /q
IF EXIST .\FCT.LLC.Common_DIR\CLEAN.COPY.FCT.LLC.Common.BAT CALL .\FCT.LLC.Common_DIR\CLEAN.COPY.FCT.LLC.Common.BAT
REM ==================== END Clean Folders ====================

REM ==================== SET AssemblyInfo PATHs ====================

SET info_path_1="%local_path_solution%\FCT.LLC.Common.Utility\Properties\AssemblyInfo.cs"
SET info_path_2="%local_path_solution%\FCT.LLC.Common.Constants\Properties\AssemblyInfo.cs"
SET info_path_3="%local_path_solution%\DataContracts\Properties\AssemblyInfo.cs"
SET info_path_4="%local_path_solution%\GenericRepository\Properties\AssemblyInfo.cs"
SET info_path_5="%local_path_solution%\Logging\Properties\AssemblyInfo.cs"

REM ==================== GET TFS CHECKOUT STATUS ====================

%tf_path% status /recursive /format:detailed /user:* "%local_path_solution%\*.*" > ".\FCT.LLC.Common_DIR\CheckoutStatus.txt"
%consm_path% /c:file=".\FCT.LLC.Common_DIR\CheckoutStatus.txt"
IF %ERRORLEVEL%==1 GOTO :END

REM ==================== GET VERSION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start getting version
ECHO ----- Start getting version ----- 

%tf_path% get /recursive /version:T /force "%local_path_solution%\*.*" /noprompt
%tf_path% get /recursive /version:T /force "%local_path_binaries%\*.*" /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End getting version
ECHO ----- End getting version ----- 

REM ==================== UPDATE VERSION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start updating versions
ECHO ----- Start updating versions ----- 

%tf_path% checkout %info_path_1% /noprompt
%asminfoupdater_path% %info_path_1%

%tf_path% checkout %info_path_2% /noprompt
%asminfoupdater_path% %info_path_2%

%tf_path% checkout %info_path_3% /noprompt
%asminfoupdater_path% %info_path_3%

%tf_path% checkout %info_path_4% /noprompt
%asminfoupdater_path% %info_path_4%

%tf_path% checkout %info_path_5% /noprompt
%asminfoupdater_path% %info_path_5%

%reportlogger_path% /i:file=%log_file% /i:lgtime=End updating versions
ECHO ----- End updating versions ----- 

REM ==================== BUILD SOLUTION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start build solution
ECHO ----- Start build solution ----- 

%msbuild_path% "%local_path_solution%\FCT.LLC.Common.sln" /t:Rebuild /p:Configuration=Release /fl /flp:logfile=.\FCT.LLC.Common_DIR\FCT.LLC.Common.sln.MSBUILD.log
IF %ERRORLEVEL%==0 GOTO CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start undo on build solution error
ECHO ----- Start undo on build solution error ----- 

%tf_path% undo %info_path_1% /noprompt
%tf_path% undo %info_path_2% /noprompt
%tf_path% undo %info_path_3% /noprompt
%tf_path% undo %info_path_4% /noprompt
%tf_path% undo %info_path_5% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End undo on build solution error
ECHO ----- End undo on build solution error ----- 

%consm_path% /f:m=FCT.LLC.Common.sln
GOTO END
REM ==================== CHECKIN SOLUTION ====================

:CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=End build
ECHO ----- End build ----- 


%reportlogger_path% /i:file=%log_file% /i:lgtime=Start check in solution
ECHO ----- Start check in solution ----- 

%tf_path% checkin /override:AutoBuild  %info_path_1% /noprompt
%tf_path% checkin /override:AutoBuild  %info_path_2% /noprompt
%tf_path% checkin /override:AutoBuild  %info_path_3% /noprompt
%tf_path% checkin /override:AutoBuild  %info_path_4% /noprompt
%tf_path% checkin /override:AutoBuild  %info_path_5% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End check in solution
ECHO ----- End check in solution ----- 

%tf_path% checkout "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Utility.dll" /noprompt
xcopy "%local_path_solution%\FCT.LLC.Common.Utility\Bin\Release\FCT.LLC.Common.Utility.dll" "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Utility.dll" /Y /V
%tf_path% checkin /override:AutoBuild  "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Utility.dll" /noprompt

%tf_path% checkout "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Constants.dll" /noprompt
xcopy "%local_path_solution%\FCT.LLC.Common.Constants\Bin\Release\FCT.LLC.Common.Constants.dll" "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Constants.dll" /Y /V
%tf_path% checkin /override:AutoBuild  "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.Constants.dll" /noprompt

%tf_path% checkout "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.DataContracts.dll" /noprompt
xcopy "%local_path_solution%\DataContracts\Bin\Release\FCT.LLC.Common.DataContracts.dll" "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.DataContracts.dll" /Y /V
%tf_path% checkin /override:AutoBuild  "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Common.DataContracts.dll" /noprompt

%tf_path% checkout "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.GenericRepository.dll" /noprompt
xcopy "%local_path_solution%\GenericRepository\Bin\Release\FCT.LLC.GenericRepository.dll" "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.GenericRepository.dll" /Y /V
%tf_path% checkin /override:AutoBuild  "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.GenericRepository.dll" /noprompt

%tf_path% checkout "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Logging.dll" /noprompt
xcopy "%local_path_solution%\Logging\Bin\Release\FCT.LLC.Logging.dll" "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Logging.dll" /Y /V
%tf_path% checkin /override:AutoBuild  "%local_path_binaries%\FCT.LLC.Common\FCT.LLC.Logging.dll" /noprompt

%consm_path% /s:m=FCT.LLC.Common.sln /i:log=%log_file%

REM ==================== END ====================
:END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End batch
ECHO ----- End batch ----- 

