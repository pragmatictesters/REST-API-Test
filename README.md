

# REST API Test Suite

This project contains automated tests for verifying the functionality of the REST API at [restful-api.dev](https://restful-api.dev). The tests cover CRUD operations (Create, Read, Update, and Delete) for objects in the API.

## Prerequisites

Before running the tests, make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (Please use the latest version)
- [Git](https://git-scm.com/downloads)

## Clone the Repository

To get started, clone this repository to your local machine:

```bash
git clone https://github.com/pragmatictesters/REST-API-Test.git
cd REST-API-Test
```

## Restore Dependencies

Once inside the project directory, restore the necessary dependencies using the `dotnet` CLI:

```bash
dotnet restore
```

This will download and install all the required packages listed in the `.csproj` file.

## Running the Tests

After restoring dependencies, you can run the test suite using:

```bash
dotnet test
```

This will run all the tests and provide output in the console showing the test results, including any failures.

### Running Specific Tests

If you want to run a specific test or group of tests, you can filter by test name:

```bash
dotnet test --filter "TestName"
```

For example, to run only the object creation test, you can use:

```bash
dotnet test --filter "AddObject_ShouldReturnValidResponse"
```

## Test Scenarios

1. **Get a list of all objects**  
   Test to ensure the API can retrieve a list of all objects using a GET request.
   
2. **Add an object using POST**  
   Test to ensure a new object can be added to the API using a POST request.
   
3. **Get a single object using the added ID**  
   Test to retrieve the object added in Step 2 using its unique ID with a GET request.
   
4. **Update the object using PUT**  
   Test to update the object created in Step 2 using a PUT request.
   
5. **Delete the object using DELETE**  
   Test to delete the object created in Step 2 using a DELETE request.

## Test Structure

The test suite is organized into the following key tests:

- **[List All Objects](./REST-API-Tests/Tests/ObjectListingAndValidationTests.cs)**: Verifies that all objects can be listed using the API's `GET` method and ensures each key in the response contains valid data (e.g., checking for valid `id`, `name`, and `data` fields).
- **[Object Creation](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Verifies that objects can be created using the API's `POST` method.
- **[Object Retrieval](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Ensures that objects can be retrieved by their ID using the `GET` method.
- **[Object Update](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Tests the update functionality using the API's `PUT` method.
- **[Object Deletion](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Confirms that objects can be deleted and verifies the appropriate error message when trying to access a deleted object.


### List of Dependencies

The following libraries are used in this project to facilitate API testing, logging, and assertions:

- **[RestSharp](https://restsharp.dev/)**  
  A simple HTTP client for .NET. RestSharp is used to make HTTP requests (GET, POST, PUT, DELETE) to the API and handle responses efficiently.

- **[NUnit](https://nunit.org/)**  
  A popular unit-testing framework for .NET. NUnit provides a rich set of features for writing and running tests, ensuring the correctness of the API operations.

- **[NLog](https://nlog-project.org/)**  
  A flexible and free logging platform for .NET. NLog is used to add logging at different levels (info, error, etc.) for better test traceability and debugging.

- **[FluentAssertions](https://fluentassertions.com/)**  
  A set of extension methods that enhance the readability of test assertions. FluentAssertions makes it easier to write clear and descriptive tests, improving code maintainability.

Each of these libraries helps improve the quality, maintainability, and readability of the tests while providing powerful capabilities for handling API requests, logging, and assertions.


## Rate Limit Considerations 
### ⚠️ Warning

Please note that the API has a **request limit of 100 requests per day**. If you exceed this limit, you will receive an error message stating that you have reached the daily limit. The limit resets every 24 hours. You can mock the API for some of the tests or reduce the number of test runs to avoid hitting the rate limit.

## Setting up GitHub Actions for CI/CD Integration

To automatically run the tests on each push or pull request, you can set up a CI/CD pipeline with **GitHub Actions**. This ensures that your test suite runs every time changes are made to the codebase, providing immediate feedback on any issues.

## GitHub Actions Workflow Configuration

The following is an example **GitHub Actions** workflow configuration for running the tests with logging and test results tracking.

```yaml
name: .NET Test Suite

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v2

    - name: Set up .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '6.0.x'

    - name: Install dependencies
      run: dotnet restore

    - name: Run tests with logging
      run: dotnet test --logger "trx;LogFileName=TestResults.trx"

    - name: Upload Test Results
      uses: actions/upload-artifact@v2
      with:
        name: TestResults
        path: TestResults.trx
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Issues

If you encounter any issues, feel free to create an issue on GitHub.
