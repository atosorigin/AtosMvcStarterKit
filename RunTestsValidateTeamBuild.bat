REM Use this batch file to run mstest unit tests similar to a team build test run
RUM With the batch file issues with team build unit tests can be analysed efficiently
cd UnitTest\bin\debug
"%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\mstest" /runconfig:..\..\..\LocalTestRun.testrunconfig /testcontainer:customer.project.unittest.dll
cd ..\..\..\
pause
