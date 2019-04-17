:CheckOS
IF EXIST "%PROGRAMFILES(X86)%" (GOTO 64BIT) ELSE (GOTO 32BIT)

:64BIT
call "%ProgramFiles(x86)%\2C\2C.bat" esm install Intation.TicketSystem.WebApplication "%cd%\run.bat"
call "%ProgramFiles(x86)%\2C\2C.bat" esm set Intation.TicketSystem.WebApplication ObjectName ".\VSTS" "VSTS"
call "%ProgramFiles(x86)%\2C\2C.bat" esm start Intation.TicketSystem.WebApplication
GOTO END

:32BIT
call "%ProgramFiles%\2C\2C.bat" esm install Intation.TicketSystem.WebApplication "%cd%\run.bat"
call "%ProgramFiles(x86)%\2C\2C.bat" esm set Intation.TicketSystem.WebApplication ObjectName ".\VSTS" "VSTS"
call "%ProgramFiles%\2C\2C.bat" esm start Intation.TicketSystem.WebApplication
GOTO END

:END 






