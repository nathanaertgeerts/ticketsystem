:CheckOS
IF EXIST "%PROGRAMFILES(X86)%" (GOTO 64BIT) ELSE (GOTO 32BIT)

:64BIT
call "%ProgramFiles(x86)%\2C\2C.bat" esm stop Intation.TicketSystem.WebApplication
call "%ProgramFiles(x86)%\2C\2C.bat" esm remove Intation.TicketSystem.WebApplication confirm
GOTO END

:32BIT
call "%ProgramFiles%\2C\2C.bat" esm stop Intation.TicketSystem.WebApplication
call "%ProgramFiles%\2C\2C.bat" esm remove Intation.TicketSystem.WebApplication confirm
GOTO END

:END






