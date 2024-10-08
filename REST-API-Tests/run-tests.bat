@echo off

:: Create Logs folder if it doesn't exist
if not exist Logs (
    mkdir Logs
)

:: Variables for logging
set CLEAN_LOG=Logs\clean.log
set BUILD_LOG=Logs\build.log
set TEST_LOG=Logs\test.log

:: Function to handle errors
:handle_error
echo ❌ Error occurred during: %1
echo Check the logs: %1.log
exit /b 1

:: Cleaning the project
echo 🧹 Cleaning the project...
dotnet clean > %CLEAN_LOG% 2>&1
if %ERRORLEVEL% neq 0 (
    call :handle_error clean
)
echo ✅ Clean completed successfully.

:: Building the project
echo 🔨 Building the project...
dotnet build > %BUILD_LOG% 2>&1
if %ERRORLEVEL% neq 0 (
    call :handle_error build
)
echo ✅ Build completed successfully.

:: Running the tests
echo 🧪 Running the tests...
dotnet test > %TEST_LOG% 2>&1
if %ERRORLEVEL% neq 0 (
    call :handle_error test
)
echo ✅ Tests completed successfully.

:: Successful completion
echo 🎉 All steps completed successfully!
exit /b 0