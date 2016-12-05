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

SET log_file=".\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.log"
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

%reportlogger_path% /i:file=%log_file% /i:lgtime=Parameter local_path_apd: [%3]
ECHO ----- Parameter local_path_apd: [%3] ----- 

FOR /f "useback tokens=*" %%a IN ('%3') DO SET local_path_apd=%%~a
FOR /f "tokens=* delims= " %%a IN ("%local_path_apd%") DO SET local_path_apd=%%a
IF "%local_path_apd%"=="" GOTO VALIDATION_ERROR

GOTO VALIDATION_OK

:VALIDATION_ERROR
%consm_path% /f:msg="One of the configuration parameters is empty. Check ".\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.log""
GOTO END

:VALIDATION_OK

REM ==================== Clean Folders ====================
rmdir "%local_path_apd%" /s /q
rmdir "%local_path_binaries%" /s /q
IF EXIST .\FCT.EPS.NotificationMonitor_DIR\CLEAN.COPY.FCT.EPS.NotificationMonitor.BAT CALL .\FCT.EPS.NotificationMonitor_DIR\CLEAN.COPY.FCT.EPS.NotificationMonitor.BAT
REM ==================== END Clean Folders ====================

REM ==================== SET AssemblyInfo PATHs ====================

SET info_path_1="%local_path_solution%\FCT.EPS.NotificationMonitor\Properties\AssemblyInfo.cs"
SET info_path_2="%local_path_solution%\FCT.EPS.NotificationMonitor.Resources\Properties\AssemblyInfo.cs"

REM ==================== GET TFS CHECKOUT STATUS ====================

%tf_path% status /recursive /format:detailed /user:* "%local_path_solution%\*.*" > ".\FCT.EPS.NotificationMonitor_DIR\CheckoutStatus.txt"
%consm_path% /c:file=".\FCT.EPS.NotificationMonitor_DIR\CheckoutStatus.txt"
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

%reportlogger_path% /i:file=%log_file% /i:lgtime=End updating versions
ECHO ----- End updating versions ----- 

REM ==================== BUILD SOLUTION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start build solution
ECHO ----- Start build solution ----- 

%msbuild_path% "%local_path_solution%\FCT.EPS.NotificationMonitor.sln" /t:Rebuild /p:Configuration=Release /fl /flp:logfile=.\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.sln.MSBUILD.log
IF %ERRORLEVEL%==0 GOTO CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start undo on build solution error
ECHO ----- Start undo on build solution error ----- 

%tf_path% undo %info_path_1% /noprompt
%tf_path% undo %info_path_2% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End undo on build solution error
ECHO ----- End undo on build solution error ----- 

%consm_path% /f:m=FCT.EPS.NotificationMonitor.sln
GOTO END
REM ==================== CHECKIN SOLUTION ====================

:CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=End build
ECHO ----- End build ----- 


%reportlogger_path% /i:file=%log_file% /i:lgtime=Start check in solution
ECHO ----- Start check in solution ----- 

%tf_path% checkin %info_path_1% /noprompt
%tf_path% checkin %info_path_2% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End check in solution
ECHO ----- End check in solution ----- 

%consm_path% /s:m=FCT.EPS.NotificationMonitor.sln /i:log=%log_file%

REM ==================== START APD ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start getting latest version APD
ECHO ----- Start getting latest version APD ----- 

%tf_path% get /recursive /version:T /force "%local_path_apd%\*.*" /noprompt
%tf_path%  checkout /recursive "%local_path_apd%" /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End getting latest version APD
ECHO ----- End getting latest version APD ----- 

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copy validator
ECHO ----- Start copy validator ----- 

%copyvalidator_path% /p:$(local_path_solution)="%local_path_solution%" /p:$(local_path_apd)="%local_path_apd%" /i:file=.\FCT.EPS.NotificationMonitor_DIR\STN.FCT.EPS.NotificationMonitor.XML /i:batch=.\FCT.EPS.NotificationMonitor_DIR\COPY.FCT.EPS.NotificationMonitor.BAT
IF %ERRORLEVEL%==0 GOTO COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Copy validator failed
ECHO ----- Copy validator failed ----- 
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copy validator
ECHO ----- End copy validator ----- 

:COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying solution build results
ECHO ----- Start copying solution build results ----- 

call .\FCT.EPS.NotificationMonitor_DIR\COPY.FCT.EPS.NotificationMonitor.BAT

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying solution build results
ECHO ----- End copying solution build results ----- 

REM ==================== BUILD PACKAGE 1====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start building package
ECHO ----- Start building package 1 ----- 

%iscmdbld_path% -p "%local_path_apd%\IS Project\FCT.EPS.NotificationMonitor.Send.ism" > .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Send.ISM.log

IF %ERRORLEVEL%==0 GOTO COPY_PACKAGE1

%reportlogger_path% /i:file=%log_file% /i:lgtime=InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Send.ISM.log
ECHO ----- InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Send.ISM.log ----- 

%tf_path% undo /recursive "%local_path_apd%" /noprompt
%consm_path% /f:msg="InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Send.ISM.log"
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End building package
ECHO ----- End building package 1 ----- 

REM ==================== COPY PACKAGE 1====================
:COPY_PACKAGE1

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying package
ECHO ----- Start copying package 1 ----- 

xcopy "%local_path_apd%\IS Project\FCT.EPS.NotificationMonitor.Send\Product Configuration 1\Release 1\DiskImages\DISK1\FCT.EPS.NotificationMonitor.Send.msi" "%local_path_apd%\Release\FCT.EPS.NotificationMonitor.Send.msi" /Y /V
%logon_identity_path% "%local_path_solution%"
REM Check in later 
REM %tf_path%  checkin /recursive "%local_path_apd%" /noprompt /comment:@".\comment.txt"
%consm_path% /s:msg="InstallShield project has been built."

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying package
ECHO ----- End copying package 1 ----- 

REM ==================== BUILD PACKAGE 2====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start building package
ECHO ----- Start building package 2 ----- 

%iscmdbld_path% -p "%local_path_apd%\IS Project\FCT.EPS.NotificationMonitor.Receive.ism" > .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Receive.ISM.log

IF %ERRORLEVEL%==0 GOTO COPY_PACKAGE2

%reportlogger_path% /i:file=%log_file% /i:lgtime=InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Receive.ISM.log
ECHO ----- InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Receive.ISM.log ----- 

%tf_path% undo /recursive "%local_path_apd%" /noprompt
%consm_path% /f:msg="InstallShield failed. Check .\FCT.EPS.NotificationMonitor_DIR\FCT.EPS.NotificationMonitor.Receive.ISM.log"
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End building package
ECHO ----- End building package 2 ----- 

REM ==================== COPY PACKAGE 2====================
:COPY_PACKAGE2

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying package
ECHO ----- Start copying package 2 ----- 

xcopy "%local_path_apd%\IS Project\FCT.EPS.NotificationMonitor.Receive\Product Configuration 1\Release 1\DiskImages\DISK1\FCT.EPS.NotificationMonitor.Receive.msi" "%local_path_apd%\Release\FCT.EPS.NotificationMonitor.Receive.msi" /Y /V
%logon_identity_path% "%local_path_solution%"
%tf_path%  checkin /recursive "%local_path_apd%" /noprompt /comment:@".\comment.txt"
%consm_path% /s:msg="InstallShield project has been built."

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying package
ECHO ----- End copying package 2 ----- 

REM ==================== END ====================
:END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End batch
ECHO ----- End batch ----- 

