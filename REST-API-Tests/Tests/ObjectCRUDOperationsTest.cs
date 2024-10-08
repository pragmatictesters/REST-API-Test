namespace REST_API_Tests
{
    using NUnit.Framework;
    using RestSharp;
    using FluentAssertions;
    using System.Net;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class ObjectCRUDOperationsTest
    {
        private RestClient _client;
        private string _baseUrl = "https://api.restful-api.dev";
        private bool _basicTestPassed;

        [SetUp]
        public void Setup()
        {
            // Initialize the RestClient with the base URL
            _client = new RestClient(_baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the RestClient after each test
            _client?.Dispose();
        }

        [Test]
        [Order(1)]
        public void GetAllObjects_ShouldReturnValidResponse()
        {
            // Arrange
            var request = new RestRequest("/objects", Method.Get);

            // Act
            var response = _client.Execute(request);

            // Assertions 
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ContentType.Should().Be("application/json");

            // Deserialize the response content into a JArray
            var responseBody = JArray.Parse(response.Content);

            // Ensure the response body is not null and contains items
            responseBody.Should().NotBeNull();
            responseBody.Count.Should().BeGreaterThan(0);  // Ensure the list is not empty

            // Basic validation of items: id, name, data existence
            foreach (var item in responseBody)
            {
                item["id"].Should().NotBeNull();   // Check if "id" exists
                item["name"].Should().NotBeNull(); // Check if "name" exists
                item["data"].Should().NotBeNull(); // Check if "data" exists
            }

            // If everything passed, set the flag to true
            _basicTestPassed = true;
        }

        [Test]
        public void ValidatePrice_ShouldBeValidPrice()
        {
            Assume.That(_basicTestPassed, Is.True, "Basic validation must pass before running detailed data checks.");

            var request = new RestRequest("/objects", Method.Get);
            var response = _client.Execute(request);
            var responseBody = JArray.Parse(response.Content);

            foreach (var item in responseBody)
            {
                if (item["data"] != null && item["data"].Type == JTokenType.Object)
                {
                    var data = (JObject)item["data"];
                    if (data.ContainsKey("price"))
                    {
                        var price = data["price"];
                        price.Type.Should().BeOneOf(JTokenType.Float, JTokenType.Integer);
                        decimal.TryParse(price.ToString(), out _).Should().BeTrue();
                    }
                }
            }
        }

        [Test]
        public void ValidateCapacity_ShouldBeValidCapacity()
        {
            Assume.That(_basicTestPassed, Is.True, "Basic validation must pass before running detailed data checks.");

            var request = new RestRequest("/objects", Method.Get);
            var response = _client.Execute(request);
            var responseBody = JArray.Parse(response.Content);

            foreach (var item in responseBody)
            {
                if (item["data"] != null && item["data"].Type == JTokenType.Object)
                {
                    var data = (JObject)item["data"];
                    if (data.ContainsKey("Capacity") || data.ContainsKey("capacity"))
                    {
                        var capacityKey = data.ContainsKey("Capacity") ? "Capacity" : "capacity";
                        var capacity = data[capacityKey];

                        capacity.Type.Should().Be(JTokenType.String);
                        var validCapacities = new[] { "64 GB", "128 GB", "254 GB", "256 GB", "512 GB", "1 TB", "2 TB" };
                        validCapacities.Should().Contain(capacity.ToString());
                    }
                }
            }
        }

        [Test]
        public void ValidateYear_ShouldBeValidYear()
        {
            Assume.That(_basicTestPassed, Is.True, "Basic validation must pass before running detailed data checks.");

            var request = new RestRequest("/objects", Method.Get);
            var response = _client.Execute(request);
            var responseBody = JArray.Parse(response.Content);

            foreach (var item in responseBody)
            {
                if (item["data"] != null && item["data"].Type == JTokenType.Object)
                {
                    var data = (JObject)item["data"];
                    if (data.ContainsKey("year"))
                    {
                        var year = data["year"];
                        year.Type.Should().Be(JTokenType.Integer);
                        int yearValue = year.ToObject<int>();
                        yearValue.Should().BeInRange(2001, DateTime.Now.Year);
                    }
                }
            }
        }

        [Test]
        public void ValidateColor_ShouldBeValidColor()
        {
            Assume.That(_basicTestPassed, Is.True, "Basic validation must pass before running detailed data checks.");

            var request = new RestRequest("/objects", Method.Get);
            var response = _client.Execute(request);
            var responseBody = JArray.Parse(response.Content);

            foreach (var item in responseBody)
            {
                if (item["data"] != null && item["data"].Type == JTokenType.Object)
                {
                    var data = (JObject)item["data"];
                    if (data.ContainsKey("color") || data.ContainsKey("Color"))
                    {
                        var colorKey = data.ContainsKey("color") ? "color" : "Color";
                        var color = data[colorKey];

                        color.Type.Should().Be(JTokenType.String);
                        var validColors = new[] { "Cloudy White", "Blue", "Purple", "Brown", "Red", "Elderberry", "Green", "Yellow", "Black", "White" };
                        validColors.Should().Contain(color.ToString());
                    }
                }
            }
        }

        [Test]
        public void ValidateGeneration_ShouldBeValidGeneration()
        {
            Assume.That(_basicTestPassed, Is.True, "Basic validation must pass before running detailed data checks.");

            var request = new RestRequest("/objects", Method.Get);
            var response = _client.Execute(request);
            var responseBody = JArray.Parse(response.Content);

            foreach (var item in responseBody)
            {
                if (item["data"] != null && item["data"].Type == JTokenType.Object)
                {
                    var data = (JObject)item["data"];
                    if (data.ContainsKey("generation") || data.ContainsKey("Generation"))
                    {
                        var generationKey = data.ContainsKey("generation") ? "generation" : "Generation";
                        var generation = data[generationKey];

                        generation.Type.Should().Be(JTokenType.String);
                        var validGenerations = new[] { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th" };
                        validGenerations.Should().Contain(generation.ToString());
                    }
                }
            }
        }
    }
}
