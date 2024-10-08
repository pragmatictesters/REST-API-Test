

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
dotnet test --filter "AddObject_ShouldReturnValidResponse" Tes
```

## Test Structure

The test suite is organized into the following key tests:

- **[Object Creation](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Verifies that objects can be created using the API's `POST` method.
- **[Object Retrieval](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Ensures that objects can be retrieved by their ID using the `GET` method.
- **[Object Update](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Tests the update functionality using the API's `PUT` method.
- **[Object Deletion](./REST-API-Tests/Tests/ObjectCRUDTests.cs)**: Confirms that objects can be deleted and verifies the appropriate error message when trying to access a deleted object.
- **[List All Objects](./REST-API-Tests/Tests/ObjectListingAndValidationTests.cs)**: Verifies that all objects can be listed using the API's `GET` method and ensures each key in the response contains valid data (e.g., checking for valid `id`, `name`, and `data` fields).


## Rate Limit Considerations

Please note that the API has a **request limit of 100 requests per day**. If you exceed this limit, you will receive an error message stating that you have reached the daily limit. The limit resets every 24 hours. You can mock the API for some of the tests or reduce the number of test runs to avoid hitting the rate limit.

## Setting up GitHub Actions (Optional)

To automatically run the tests on each push, you can set up a CI/CD pipeline with GitHub Actions. Here’s an example workflow configuration:

```yaml
name: .NET Test

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
    - run: dotnet restore
    - run: dotnet test
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Issues

If you encounter any issues, feel free to create an issue on GitHub.
