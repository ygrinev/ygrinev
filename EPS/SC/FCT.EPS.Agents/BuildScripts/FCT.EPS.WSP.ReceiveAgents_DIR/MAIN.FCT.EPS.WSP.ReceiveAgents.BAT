@ECHO OFF
REM XSLT VERSION 2013-04-22 15:24
REM BUILD PROCESS VERSION 1.1.0.1

REM ========== TOOLS ==========

SET tf_path="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\tf.exe"
SET msbuild_path="C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
SET asminfoupdater_path="D:\TFS\Tools\AsmInfoUpdater.exe"
SET consm_path="D:\TFS\Tools\ConsM.exe"
SET copyvalidator_path="D:\TFS\Tools\CopyValidator.exe"
SET iscmdbld_path="c:\Program Files (x86)\InstallShield\2014 SAB\System\IsCmdBld.exe"
SET reportlogger_path="D:\TFS\Tools\ReportLogger.exe"
SET logon_identity_path="D:\TFS\Tools\TestLogon.exe"

SET log_file=".\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.log"
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
%consm_path% /f:msg="One of the configuration parameters is empty. Check ".\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.log""
GOTO END

:VALIDATION_OK

REM ==================== Clean Folders ====================
rmdir "%local_path_apd%" /s /q
rmdir "%local_path_binaries%" /s /q
IF EXIST .\FCT.EPS.WSP.ReceiveAgents_DIR\CLEAN.COPY.FCT.EPS.WSP.ReceiveAgents.BAT CALL .\FCT.EPS.WSP.ReceiveAgents_DIR\CLEAN.COPY.FCT.EPS.WSP.ReceiveAgents.BAT
REM ==================== END Clean Folders ====================

REM ==================== SET AssemblyInfo PATHs ====================

SET info_path_1="%local_path_solution%\FCT.EPS.WSP.DataAccess\Properties\AssemblyInfo.cs"
SET info_path_2="%local_path_solution%\FCT.EPS.WSP.DataAccessTests\Properties\AssemblyInfo.cs"
SET info_path_3="%local_path_solution%\FCT.EPS.WSP.ExternalResources\Properties\AssemblyInfo.cs"
SET info_path_4="%local_path_solution%\FCT.EPS.WSP.GCSA.BusinessLogic\Properties\AssemblyInfo.cs"
SET info_path_5="%local_path_solution%\FCT.EPS.WSP.GCSA.Implementation\Properties\AssemblyInfo.cs"
SET info_path_6="%local_path_solution%\FCT.EPS.WSP.Resources\Properties\AssemblyInfo.cs"
SET info_path_7="%local_path_solution%\FCT.EPS.WSP.GetChequeStateTests\Properties\AssemblyInfo.cs"
SET info_path_8="%local_path_solution%\FCT.EPS.WSP.GCSA.Resources\Properties\AssemblyInfo.cs"
SET info_path_9="%local_path_solution%\FCT.EPS.WSP.GCSA.BusinessLogicTests\Properties\AssemblyInfo.cs"
SET info_path_10="%local_path_solution%\FCT.EPS.WSP.GFSA.BusinessLogic\Properties\AssemblyInfo.cs"
SET info_path_11="%local_path_solution%\FCT.EPS.WSP.GFSA.Implementation\Properties\AssemblyInfo.cs"
SET info_path_12="%local_path_solution%\FCT.EPS.WSP.GFSA.Resources\Properties\AssemblyInfo.cs"
SET info_path_13="%local_path_solution%\FCT.EPS.WSP.GFSA.BusinessLogicTests\Properties\AssemblyInfo.cs"

REM ==================== GET TFS CHECKOUT STATUS ====================

%tf_path% status /recursive /format:detailed /user:* "%local_path_solution%\*.*" > ".\FCT.EPS.WSP.ReceiveAgents_DIR\CheckoutStatus.txt"
%consm_path% /c:file=".\FCT.EPS.WSP.ReceiveAgents_DIR\CheckoutStatus.txt"
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

%tf_path% checkout %info_path_6% /noprompt
%asminfoupdater_path% %info_path_6%

%tf_path% checkout %info_path_7% /noprompt
%asminfoupdater_path% %info_path_7%

%tf_path% checkout %info_path_8% /noprompt
%asminfoupdater_path% %info_path_8%

%tf_path% checkout %info_path_9% /noprompt
%asminfoupdater_path% %info_path_9%

%tf_path% checkout %info_path_10% /noprompt
%asminfoupdater_path% %info_path_10%

%tf_path% checkout %info_path_11% /noprompt
%asminfoupdater_path% %info_path_11%

%tf_path% checkout %info_path_12% /noprompt
%asminfoupdater_path% %info_path_12%

%tf_path% checkout %info_path_13% /noprompt
%asminfoupdater_path% %info_path_13%

%reportlogger_path% /i:file=%log_file% /i:lgtime=End updating versions
ECHO ----- End updating versions ----- 

REM ==================== BUILD SOLUTION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start build solution
ECHO ----- Start build solution ----- 

%msbuild_path% "%local_path_solution%\FCT.EPS.WSP.ReceiveAgents.sln" /t:Rebuild /p:Configuration=Release /fl /flp:logfile=.\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.sln.MSBUILD.log
IF %ERRORLEVEL%==0 GOTO CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start undo on build solution error
ECHO ----- Start undo on build solution error ----- 

%tf_path% undo %info_path_1% /noprompt
%tf_path% undo %info_path_2% /noprompt
%tf_path% undo %info_path_3% /noprompt
%tf_path% undo %info_path_4% /noprompt
%tf_path% undo %info_path_5% /noprompt
%tf_path% undo %info_path_6% /noprompt
%tf_path% undo %info_path_7% /noprompt
%tf_path% undo %info_path_8% /noprompt
%tf_path% undo %info_path_9% /noprompt
%tf_path% undo %info_path_10% /noprompt
%tf_path% undo %info_path_11% /noprompt
%tf_path% undo %info_path_12% /noprompt
%tf_path% undo %info_path_13% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End undo on build solution error
ECHO ----- End undo on build solution error ----- 

%consm_path% /f:m=FCT.EPS.WSP.ReceiveAgents.sln
GOTO END
REM ==================== CHECKIN SOLUTION ====================

:CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=End build
ECHO ----- End build ----- 


%reportlogger_path% /i:file=%log_file% /i:lgtime=Start check in solution
ECHO ----- Start check in solution ----- 

%tf_path% checkin %info_path_1% /noprompt
%tf_path% checkin %info_path_2% /noprompt
%tf_path% checkin %info_path_3% /noprompt
%tf_path% checkin %info_path_4% /noprompt
%tf_path% checkin %info_path_5% /noprompt
%tf_path% checkin %info_path_6% /noprompt
%tf_path% checkin %info_path_7% /noprompt
%tf_path% checkin %info_path_8% /noprompt
%tf_path% checkin %info_path_9% /noprompt
%tf_path% checkin %info_path_10% /noprompt
%tf_path% checkin %info_path_11% /noprompt
%tf_path% checkin %info_path_12% /noprompt
%tf_path% checkin %info_path_13% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End check in solution
ECHO ----- End check in solution ----- 

%consm_path% /s:m=FCT.EPS.WSP.ReceiveAgents.sln /i:log=%log_file%

REM ==================== START APD ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start getting latest version APD
ECHO ----- Start getting latest version APD ----- 

%tf_path% get /recursive /version:T /force "%local_path_apd%\*.*" /noprompt
%tf_path%  checkout /recursive "%local_path_apd%" /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End getting latest version APD
ECHO ----- End getting latest version APD ----- 

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copy validator
ECHO ----- Start copy validator ----- 

%copyvalidator_path% /p:$(local_path_solution)="%local_path_solution%" /p:$(local_path_apd)="%local_path_apd%" /i:file=.\FCT.EPS.WSP.ReceiveAgents_DIR\STN.FCT.EPS.WSP.ReceiveAgents.XML /i:batch=.\FCT.EPS.WSP.ReceiveAgents_DIR\COPY.FCT.EPS.WSP.ReceiveAgents.BAT
IF %ERRORLEVEL%==0 GOTO COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Copy validator failed
ECHO ----- Copy validator failed ----- 
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copy validator
ECHO ----- End copy validator ----- 

:COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying solution build results
ECHO ----- Start copying solution build results ----- 

call .\FCT.EPS.WSP.ReceiveAgents_DIR\COPY.FCT.EPS.WSP.ReceiveAgents.BAT

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying solution build results
ECHO ----- End copying solution build results ----- 

REM ==================== BUILD PACKAGE ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start building package
ECHO ----- Start building package ----- 

%iscmdbld_path% -p "%local_path_apd%\IS Project\FCT.EPS.WSP.ReceiveAgents.ism" > .\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.ISM.log

IF %ERRORLEVEL%==0 GOTO COPY_PACKAGE

%reportlogger_path% /i:file=%log_file% /i:lgtime=InstallShield failed. Check .\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.ISM.log
ECHO ----- InstallShield failed. Check .\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.ISM.log ----- 

%tf_path% undo /recursive "%local_path_apd%" /noprompt
%consm_path% /f:msg="InstallShield failed. Check .\FCT.EPS.WSP.ReceiveAgents_DIR\FCT.EPS.WSP.ReceiveAgents.ISM.log"
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End building package
ECHO ----- End building package ----- 

REM ==================== COPY PACKAGE ====================
:COPY_PACKAGE

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying package
ECHO ----- Start copying package ----- 

xcopy "%local_path_apd%\IS Project\FCT.EPS.WSP.ReceiveAgents\Product Configuration 1\Release 1\DiskImages\DISK1\FCT.EPS.WSP.ReceiveAgents.msi" "%local_path_apd%\Release\FCT.EPS.WSP.ReceiveAgents.msi" /Y /V
%logon_identity_path% "%local_path_solution%"
%tf_path%  checkin /recursive "%local_path_apd%" /noprompt /comment:@".\comment.txt"
%consm_path% /s:msg="InstallShield project has been built."

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying package
ECHO ----- End copying package ----- 

REM ==================== END ====================
:END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End batch
ECHO ----- End batch ----- 

