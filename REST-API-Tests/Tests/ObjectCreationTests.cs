using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Generic;

namespace REST_API_Tests
{
    [TestFixture]
    public class ObjectCreationTests
    {
        private RestClient _client;
        private string _baseUrl = "https://api.restful-api.dev";
        private string _createdObjectId;


        [SetUp]
        public void Setup()
        {
            _client = new RestClient(_baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public void AddAppleMacBookPro16_ShouldReturnValidResponse()
        {
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
            createdAt.Should().BeBefore(DateTime.Now);  // Ensure createdAt is before the current time
        }


        [Test]
        public void GetObjectById_ShouldReturnCorrectObject()
        {
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
        }
    }
}