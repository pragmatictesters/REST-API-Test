using REST_API_Tests.Helpers; // Import the helper
using Allure.Net.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;


namespace REST_API_Tests


{
    [TestFixture]
    [AllureNUnit]
    public class ObjectCreationTests
    {
        private RestClient _client;
        private string _baseUrl;
        private string _createdObjectId;
        private readonly ILogger<ObjectCreationTests> _logger;


        public ObjectCreationTests()
        {
            // Initialize logger for this class
            _logger = LoggerHelper.CreateLogger<ObjectCreationTests>();

        }


        [SetUp]
        public void Setup()
        {
            var config = ConfigurationHelper.GetConfiguration();
            _baseUrl = config["ApiSettings:BaseUrl"];
            _client = new RestClient(_baseUrl);
            _logger.LogInformation("Initialized ObjectCRUDOperationsTest with BaseURL: {BaseUrl}", _baseUrl);


        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _logger.LogInformation("Tearing down the test AddObject_ShouldReturnValidResponse.");
            LoggerHelper.Shutdown();  // Ensure NLog flushes logs
        }

        [Test, Description("TC-OBJ-001: Verify adding a new object and validate the response")]
        [AllureSuite("ObjectCreationTests")]
        [AllureFeature("TC-OBJ-001: Verify adding a new object and validate the response")]
        [AllureSeverity(SeverityLevel.normal)]  // Use SeverityLevel from Allure.Net.Commons
        [Order(1)]
        public void AddObject_ShouldReturnValidResponse()
        {

            _logger.LogInformation("Starting test: AddObject_ShouldReturnValidResponse");

            // Arrange: Use Dictionary for the 'data' part to handle string keys with spaces
            var request = new RestRequest("/objects", Method.Post);
            var data = new Dictionary<string, object>
            {
                { "year", 2019 },
                { "price", 1849.99 },
                { "CPU model", "Intel Core i9" },
                { "Hard disk size", "1 TB" }
            };

            var requestBody = new
            {
                name = "Apple MacBook Pro 16",
                data = data
            };

            request.AddJsonBody(requestBody);

            // Act
            var response = _client.Execute(request);

            // Assert basic response properties
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ContentType.Should().Be("application/json");

            // Parse the response content into a JObject
            var responseBody = JObject.Parse(response.Content);

            // Print the response content to the console for debugging
            Console.WriteLine(response.Content);

            // Assert response body fields
            responseBody.Should().NotBeNull();  // Ensure the response body is not null
            responseBody["id"].Should().NotBeNull(); // Ensure "id" exists

            _createdObjectId = responseBody["id"].ToString(); // Store the created object's ID
            _createdObjectId.Should().NotBeNullOrEmpty();     // Ensure the ID is not null or empty


            // Check if "name" exists and validate its value
            responseBody["name"].Should().NotBeNull();
            responseBody["name"].ToString().Should().Be("Apple MacBook Pro 16");

            // Check the "data" object and its fields
            var responseData = responseBody["data"];
            responseData.Should().NotBeNull();  // Ensure the "data" field is present

            if (responseData != null)
            {

                // Validate individual fields in the "data" object
                responseData["year"].Should().NotBeNull();
                responseData["year"].ToObject<int>().Should().Be(2019);  // Check "year"

                responseData["price"].Should().NotBeNull();
                responseData["price"].ToObject<decimal>().Should().Be(1849.99M);  // Check "price"

                responseData["CPU model"].Should().NotBeNull();
                responseData["CPU model"].ToString().Should().Be("Intel Core i9");  // Check "CPU model"

                responseData["Hard disk size"].Should().NotBeNull();
                responseData["Hard disk size"].ToString().Should().Be("1 TB");  // Check "Hard disk size"
            }

            // Check if "createdAt" exists and validate its value
            responseBody["createdAt"].Should().NotBeNull();
            var createdAt = responseBody["createdAt"].ToObject<DateTime>();
            //FIXME: Need to check the time agaist the server time. Provide a better vaidation
            //createdAt.Should().BeBefore(DateTime.Now);  // Ensure createdAt is before the current time

            _logger.LogInformation("Test AddObject_ShouldReturnValidResponse completed successfully.");

        }


        [Test, Description("TC-OBJ-002: Verify retrieving the object details created in the previous test")]
        [Order(2)]
        public void GetObjectById_ShouldReturnCorrectObject()
        {
            _logger.LogInformation("Starting test: GetObjectById_ShouldReturnCorrectObject");

            // Arrange: Use the ID captured in the previous step to get the object
            Assume.That(_createdObjectId, Is.Not.Null.Or.Empty, "No object ID available from creation step.");

            var request = new RestRequest($"/objects/{_createdObjectId}", Method.Get);

            // Act: Execute the GET request to retrieve the object by ID
            var response = _client.Execute(request);

            // Assert: Validate that the object is retrieved successfully
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ContentType.Should().Be("application/json");

            // Parse the response content
            var responseBody = JObject.Parse(response.Content);

            // Assert that the object matches what was created
            responseBody["id"].ToString().Should().Be(_createdObjectId); // Check the ID
            responseBody["name"].ToString().Should().Be("Apple MacBook Pro 16");

            var data = responseBody["data"];
            data["year"].ToObject<int>().Should().Be(2019);            // Validate "year"
            data["price"].ToObject<decimal>().Should().Be(1849.99M);   // Validate "price"
            data["CPU model"].ToString().Should().Be("Intel Core i9"); // Validate "CPU model"
            data["Hard disk size"].ToString().Should().Be("1 TB");     // Validate "Hard disk size"
            _logger.LogInformation("Completed test: GetObjectById_ShouldReturnCorrectObject");

        }

        [Test, Description("TC-OBJ-003: Verify updating an existing object and validate the response")]
        [Order(3)]
        public void UpdateObject_ShouldReturnUpdatedResponse()
        {
            _logger.LogInformation("Starting test: UpdateObject_ShouldReturnUpdatedResponse");

            // Arrange: Use the ID from the previous test to update the object
            Assume.That(_createdObjectId, Is.Not.Null.Or.Empty, "No object ID available from creation step.");

            var request = new RestRequest($"/objects/{_createdObjectId}", Method.Put);
            var updatedData = new Dictionary<string, object>
            {
                { "year", 2019 },
                { "price", 2049.99 },
                { "CPU model", "Intel Core i9" },
                { "Hard disk size", "1 TB" },
                { "color", "silver" } // Adding the "color" field
            };

            var updateRequestBody = new
            {
                name = "Apple MacBook Pro 16",
                data = updatedData
            };

            request.AddJsonBody(updateRequestBody);

            // Act: Execute the PUT request to update the object
            var response = _client.Execute(request);

            // Assert: Validate that the object is updated successfully
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ContentType.Should().Be("application/json");

            // Parse the response content
            var responseBody = JObject.Parse(response.Content);

            // Assert that the "id" remains the same
            responseBody["id"].ToString().Should().Be(_createdObjectId);

            // Check if "name" exists and validate its value
            responseBody["name"].Should().NotBeNull();
            responseBody["name"].ToString().Should().Be("Apple MacBook Pro 16");

            // Check the "data" object and its updated fields
            var responseData = responseBody["data"];
            responseData.Should().NotBeNull();  // Ensure the "data" field is present

            if (responseData != null)
            {
                // Validate the updated fields in the "data" object
                responseData["year"].Should().NotBeNull();
                responseData["year"].ToObject<int>().Should().Be(2019);  // Check "year"

                responseData["price"].Should().NotBeNull();
                responseData["price"].ToObject<decimal>().Should().Be(2049.99M);  // Check "price"

                responseData["CPU model"].Should().NotBeNull();
                responseData["CPU model"].ToString().Should().Be("Intel Core i9");  // Check "CPU model"

                responseData["Hard disk size"].Should().NotBeNull();
                responseData["Hard disk size"].ToString().Should().Be("1 TB");  // Check "Hard disk size"

                responseData["color"].Should().NotBeNull();
                responseData["color"].ToString().Should().Be("silver");  // Check the new "color" field
            }

            // Check if "updatedAt" exists and validate its value
            responseBody["updatedAt"].Should().NotBeNull();
            var updatedAt = responseBody["updatedAt"].ToObject<DateTime>();
            updatedAt.Should().BeAfter(DateTime.Now.AddSeconds(-60));  // Ensure updatedAt is recent (within the last 60 seconds)
            _logger.LogInformation("Completed test: UpdateObject_ShouldReturnUpdatedResponse");

        }


        [Test, Description("TC-OBJ-004-1: Verify deleting the created object and validate the deletion response")]
        [Order(4)]
        public void DeleteObject_ShouldReturnValidResponse()
        {
            _logger.LogInformation("Starting test: DeleteObject_ShouldReturnValidResponse");

            // Arrange: Use the ID from the previous test to delete the object
            Assume.That(_createdObjectId, Is.Not.Null.Or.Empty, "No object ID available from creation step.");

            var deleteRequest = new RestRequest($"/objects/{_createdObjectId}", Method.Delete);

            // Act: Execute the DELETE request to remove the object
            var deleteResponse = _client.Execute(deleteRequest);

            // Assert: Validate that the object is deleted successfully
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResponse.ContentType.Should().Be("application/json");

            // Parse the delete response content
            var deleteResponseBody = JObject.Parse(deleteResponse.Content);
            deleteResponseBody["message"].ToString().Should().Be($"Object with id = {_createdObjectId} has been deleted.");
            _logger.LogInformation("Completed test: DeleteObject_ShouldReturnValidResponse");

        }

        [Test, Description("TC-OBJ-004-2: Verify that attempting to retrieve a deleted object returns a NotFound error")]
        [Ignore("Skipping due to known issue with typo in the API response ('Oject' instead of 'Object')")]
        [Order(5)]
        public void GetDeletedObject_ShouldReturnNotFound()
        {
            _logger.LogInformation("Starting test: GetDeletedObject_ShouldReturnNotFound");

            // Arrange: Use the ID of the deleted object to attempt retrieval
            Assume.That(_createdObjectId, Is.Not.Null.Or.Empty, "No object ID available from previous deletion.");

            var getRequest = new RestRequest($"/objects/{_createdObjectId}", Method.Get);

            // Act: Execute the GET request to retrieve the deleted object
            var getResponse = _client.Execute(getRequest);

            // Assert: Validate that the object no longer exists and the appropriate error is returned
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);  // Expecting 404 Not Found
            var errorResponseBody = JObject.Parse(getResponse.Content);
            errorResponseBody["error"].ToString().Should().Contain($"Object with id={_createdObjectId} was not found.");
            _logger.LogInformation("Completed test: GetDeletedObject_ShouldReturnNotFound");

        }


    }
}