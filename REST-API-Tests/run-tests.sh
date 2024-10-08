#!/bin/bash


# Ensure Logs folder exists
mkdir -p Logs

# Variables for logging
CLEAN_LOG="Logs/clean.log"
BUILD_LOG="Logs/build.log"
TEST_LOG="Logs/test.log"

# Function to handle errors
handle_error() {
    echo "❌ Error occurred during: $1"
    echo "Check the logs: $1.log"
    exit 1
}

# Cleaning the project
echo "🧹 Cleaning the project..."
dotnet clean > $CLEAN_LOG 2>&1
if [ $? -ne 0 ]; then
    handle_error "clean"
fi
echo "✅ Clean completed successfully."

# Building the project
echo "🔨 Building the project..."
dotnet build > $BUILD_LOG 2>&1
if [ $? -ne 0 ]; then
    handle_error "build"
fi
echo "✅ Build completed successfully."

# Running the tests
echo "🧪 Running the tests..."
dotnet test > $TEST_LOG 2>&1
if [ $? -ne 0 ]; then
    handle_error "test"
fi
echo "✅ Tests completed successfully."

# Successful completion
echo "🎉 All steps completed successfully!"
exit 0
