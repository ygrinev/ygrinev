@ECHO OFF
REM XSLT VERSION 2013-04-22 15:24
REM BUILD PROCESS VERSION 1.1.0.1

REM ========== TOOLS ==========

SET tf_path="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\tf.exe"
SET msbuild_path="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"
SET asminfoupdater_path="D:\TFS\Tools\AsmInfoUpdater.exe"
SET consm_path="D:\TFS\Tools\ConsM.exe"
SET copyvalidator_path="D:\TFS\Tools\CopyValidator.exe"
SET iscmdbld_path="C:\Program Files (x86)\InstallShield\2014 SAB\System\IsCmdBld.exe"
SET reportlogger_path="D:\TFS\Tools\ReportLogger.exe"
SET logon_identity_path="D:\TFS\Tools\TestLogon.exe"

SET log_file=".\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.log"
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
%consm_path% /f:msg="One of the configuration parameters is empty. Check ".\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.log""
GOTO END

:VALIDATION_OK

REM ==================== SET AssemblyInfo PATHs ====================

SET info_path_1="%local_path_solution%\Services\Properties\AssemblyInfo.cs"
SET info_path_2="%local_path_solution%\Entities\Properties\AssemblyInfo.cs"
SET info_path_3="%local_path_solution%\DataAccess\Properties\AssemblyInfo.cs"
SET info_path_4="%local_path_solution%\BusinessLogic\Properties\AssemblyInfo.cs"

SET info_path_9="%local_path_solution%\UnitTests\Properties\AssemblyInfo.cs"

REM ==================== GET TFS CHECKOUT STATUS ====================

%tf_path% status /recursive /format:detailed /user:* "%local_path_solution%\*.*" > ".\FCT.LLC.BusinessService_DIR\CheckoutStatus.txt"
%consm_path% /c:file=".\FCT.LLC.BusinessService_DIR\CheckoutStatus.txt"
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
 

%tf_path% checkout %info_path_9% /noprompt
%asminfoupdater_path% %info_path_9%

%reportlogger_path% /i:file=%log_file% /i:lgtime=End updating versions
ECHO ----- End updating versions ----- 

REM ==================== BUILD SOLUTION ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start build solution
ECHO ----- Start build solution ----- 

%msbuild_path% "%local_path_solution%\FCT.LLC.BusinessService.sln" /t:Rebuild /p:Configuration=Release /fl /flp:logfile=.\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.sln.MSBUILD.log
IF %ERRORLEVEL%==0 GOTO CHECKIN_SOLUTION

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start undo on build solution error
ECHO ----- Start undo on build solution error ----- 

%tf_path% undo %info_path_1% /noprompt
%tf_path% undo %info_path_2% /noprompt
%tf_path% undo %info_path_3% /noprompt
%tf_path% undo %info_path_4% /noprompt

%tf_path% undo %info_path_9% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End undo on build solution error
ECHO ----- End undo on build solution error ----- 

%consm_path% /f:m=FCT.LLC.BusinessService.sln
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

%tf_path% checkin /override:AutoBuild  %info_path_9% /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End check in solution
ECHO ----- End check in solution ----- 

%consm_path% /s:m=FCT.LLC.BusinessService.sln /i:log=%log_file%

REM ==================== START APD ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start getting latest version APD
ECHO ----- Start getting latest version APD ----- 

%tf_path% get /recursive /version:T /force "%local_path_apd%\*.*" /noprompt
%tf_path%  checkout /recursive "%local_path_apd%" /noprompt

%reportlogger_path% /i:file=%log_file% /i:lgtime=End getting latest version APD
ECHO ----- End getting latest version APD ----- 

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copy validator
ECHO ----- Start copy validator ----- 
 
 ECHO ----- Parameter local_path_apd: [%3] ----- 
  ECHO ----- Parameter local_path_apd: [%4] ----- 
 ECHO ----- Parameter local_path_apd: [%5] ----- 


%copyvalidator_path% /p:$(local_path_solution)="%local_path_solution%" /p:$(local_path_apd)="%local_path_apd%" /i:file=.\FCT.LLC.BusinessService_DIR\STN.FCT.LLC.BusinessService.XML /i:batch=.\FCT.LLC.BusinessService_DIR\COPY.FCT.LLC.BusinessService.BAT
IF %ERRORLEVEL%==0 GOTO COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Copy validator failed
ECHO ----- Copy validator failed ----- 
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copy validator
ECHO ----- End copy validator ----- 

:COPY_BUILD_RESULTS

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying solution build results
ECHO ----- Start copying solution build results ----- 

call .\FCT.LLC.BusinessService_DIR\COPY.FCT.LLC.BusinessService.BAT

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying solution build results
ECHO ----- End copying solution build results ----- 

REM ==================== BUILD PACKAGE ====================

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start building package
ECHO ----- Start building package ----- 

%iscmdbld_path% -p "%local_path_apd%\IS Project\FCT.LLC.BusinessService.ism" > .\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.ISM.log

IF %ERRORLEVEL%==0 GOTO COPY_PACKAGE

%reportlogger_path% /i:file=%log_file% /i:lgtime=InstallShield failed. Check .\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.ISM.log
ECHO ----- InstallShield failed. Check .\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.ISM.log ----- 

%tf_path% undo /recursive "%local_path_apd%" /noprompt
%consm_path% /f:msg="InstallShield failed. Check .\FCT.LLC.BusinessService_DIR\FCT.LLC.BusinessService.ISM.log"
GOTO END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End building package
ECHO ----- End building package ----- 

REM ==================== COPY PACKAGE ====================
:COPY_PACKAGE

%reportlogger_path% /i:file=%log_file% /i:lgtime=Start copying package
ECHO ----- Start copying package ----- 

xcopy "%local_path_apd%\IS Project\FCT.LLC.BusinessService\Product Configuration 1\Release 1\DiskImages\DISK1\FCT.LLC.BusinessService.msi" "%local_path_apd%\Release\FCT.LLC.BusinessService.msi" /Y /V
%logon_identity_path% "%local_path_solution%"
%tf_path%  checkin /override:AutoBuild  /recursive "%local_path_apd%" /noprompt /comment:@".\comment.txt"
%consm_path% /s:msg="InstallShield project has been built."

%reportlogger_path% /i:file=%log_file% /i:lgtime=End copying package
ECHO ----- End copying package ----- 

REM ==================== END ====================
:END

%reportlogger_path% /i:file=%log_file% /i:lgtime=End batch
ECHO ----- End batch ----- 

