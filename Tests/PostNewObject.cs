using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Serilog;
using AventStack.ExtentReports;
using RazorEngine.Compilation.ImpromptuInterface;
using Newtonsoft.Json;
using System.Text.Json;

namespace ApiTestingProject.Tests
{
    public class PostNewObjectTest : BaseTest
    {
        [Test]
        public async Task PostNewObject()
        {
            Test = Extent.CreateTest("TestPostNewObject");

            // Arrange
            
            var request = new RestRequest("/objects", Method.Post);
            var requestBody = new
            {
                name = "Apple MacBook Pro 16",
                data = new
                {
                    year = 2019,
                    price = 1849.99,
                    CPU_model = "Intel Core i9",
                    Hard_disk_size = "1 TB"
                }
            };
            request.AddJsonBody(requestBody);

            // Act

            Log.Information("Sending POST request to /objects");
            var response = await Client.ExecuteAsync(request);

            // Assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var responseContent = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(response.Content);
            Console.WriteLine(" responseContent: " + responseContent);
            Console.WriteLine("responseContent.id: " + responseContent.GetProperty("id").GetString());
            
            response.IsSuccessStatusCode.Should().BeTrue();
            response.ContentType.Should().Be("application/json");
            responseContent.GetProperty("name").GetString().Should().Be("Apple MacBook Pro 16");
            responseContent.GetProperty("data").GetProperty("year").GetInt32().Should().Be(2019);
            responseContent.GetProperty("data").GetProperty("price").GetDouble().Should().Be(1849.99);
            responseContent.GetProperty("data").GetProperty("cpU_model").GetString().Should().Be("Intel Core i9"); // Response given as "CPU_model" instead of "cpU_model" in the response content  
            responseContent.GetProperty("data").GetProperty("hard_disk_size").GetString().Should().Be("1 TB"); // Response given as "Hard_disk_size" instead of "hard_disk_size" in the response content

            Test.Log(Status.Pass, "Assertions passed");
        }
    }
}