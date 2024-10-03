using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Serilog;
using AventStack.ExtentReports;
using RazorEngine.Compilation.ImpromptuInterface;

namespace ApiTestingProject.Tests
{
    public class GetAllObjectsTests : BaseTest
    {
        [Test]
        
        public async Task GetObjects()
        {
            Test = Extent.CreateTest("TestGetObjects");

            // Arrange

            var request = new RestRequest("/objects", Method.Get);

            // Act
            
            Log.Information("Sending GET request to /objects");
            var response = await Client.ExecuteAsync(request);
            var jsonArray = Newtonsoft.Json.Linq.JArray.Parse(response.Content);

            // Assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content.Should().Contain("id");
            response.Content.Should().Contain("name");
            response.Content.Should().Contain("data");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.ContentType.Should().Be("application/json");
            jsonArray.Count.Should().BeGreaterThan(0);

            Test.Log(Status.Pass, "Assertions passed");
        }
    }
}