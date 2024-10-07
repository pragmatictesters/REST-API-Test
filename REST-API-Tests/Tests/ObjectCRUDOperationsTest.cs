namespace REST_API_Tests;

using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

[TestFixture]
public class ObjectCRUDOperationsTest
{


    //TODO: 1 Add logging into the tests 
    //TODO: 2 Add reports into the tests 
    //TODO: 3 Check the naming conventions 


    private RestClient _client;
    private string _baseUrl = "https://api.restful-api.dev";


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
    public void TestGetListOfAllObjects()
    {

        // Arrange
        var request = new RestRequest("/objects", Method.Get);

        // Act
        var response = _client.Execute(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        response.ContentType.Should().Be("application/json");

        // Deserialize the response content into a JArray
        var responseBody = JArray.Parse(response.Content);

        // Ensure the response body is not null and contains items
        responseBody.Should().NotBeNull();
        responseBody.Count.Should().BeGreaterThan(0);  // Ensure the list is not empty

        // Iterate through each item and check for required keys
        foreach (var item in responseBody)
        {
            item["id"].Should().NotBeNull();   // Check if "id" exists
            item["name"].Should().NotBeNull(); // Check if "name" exists
            item["data"].Should().NotBeNull(); // Check if "data" exists
        }



    }





}
