@echo off

SET ProgFiles=%ProgramFiles(x86)%
if "%ProgFiles%"=="" SET ProgFiles=%ProgramFiles%

echo %ProgFiles%

if not exist "%ProgFiles%\JetBrains\Resharper\v4.1\bin" goto CopyResharper_v45

mkdir ReSharper_v4.1
cd ReSharper_v4.1
REM copy "%ProgFiles%\JetBrains\ReSharper\v4.1\Bin\JetBrains.Annotations.???" > nul
cd ..
echo Support for ReSharper 4.1 successfully copied.

:CopyResharper_v45

if not exist "%ProgFiles%\JetBrains\Resharper\v4.5\bin" goto CopyResharper_v50

mkdir ReSharper_v4.5
cd ReSharper_v4.5
REM copy "%ProgFiles%\JetBrains\ReSharper\v4.5\Bin\JetBrains.Annotations.???" > nul
cd ..
echo Support for ReSharper 4.5 successfully copied.

:CopyResharper_v50

if not exist "%ProgFiles%\JetBrains\Resharper\v5.0\bin" goto CopyResharper_v51

mkdir ReSharper_v5.0
cd ReSharper_v5.0
REM copy "%ProgFiles%\JetBrains\ReSharper\v5.0\Bin\JetBrains.Annotations.???" > nul
cd ..
echo Support for ReSharper 5.0 successfully copied.

:CopyResharper_v51

if not exist "%ProgFiles%\JetBrains\Resharper\v5.1\bin" goto CopyResharper_v60

mkdir ReSharper_v5.1
cd ReSharper_v5.1
REM copy "%ProgFiles%\JetBrains\ReSharper\v5.1\Bin\JetBrains.Annotations.???" > nul
cd ..
echo Support for ReSharper 5.1 successfully copied.

:CopyResharper_v60

if not exist "%ProgFiles%\JetBrains\Resharper\v6.0\bin" goto End

mkdir ReSharper_v6.0
cd ReSharper_v6.0

copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.ActionManagement.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.ComponentModel.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.Metadata.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.Shell.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.UI.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.Platform.ReSharper.Util.???" > nul
copy "%ProgFiles%\JetBrains\ReSharper\v6.0\Bin\JetBrains.ReSharper.Resources.???" > nul
cd ..
echo Support for ReSharper 6.0 successfully copied.

goto End

:End
