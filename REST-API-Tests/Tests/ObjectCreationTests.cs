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
    }
}