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

    public class GetSingleObjectTest : BaseTest
    {
        private string objectId;

        [Test]
        public async Task GetSingleObject()
        {
            Test = Extent.CreateTest("TestGetSingleObject");

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
            var response = await Client.ExecuteAsync(request);
            var responseContent = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(response.Content);
            objectId = responseContent.GetProperty("id").GetString();
            Console.WriteLine("Object id: " + objectId);
            Console.WriteLine("responseContent: " + responseContent);

            // Act

            var request1 = new RestRequest($"/objects/{objectId}", Method.Get);
            var response1 = await Client.ExecuteAsync(request1);
            var responseContent1 = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(response1.Content);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var prettyJson = System.Text.Json.JsonSerializer.Serialize(responseContent1, options);
            Console.WriteLine("Get the created single object \n" + prettyJson);

            // Assert

            responseContent1.GetProperty("id").GetString().Should().Be((string)objectId);
            responseContent1.GetProperty("name").GetString().Should().Be("Apple MacBook Pro 16");
            responseContent1.GetProperty("data").GetProperty("year").GetInt32().Should().Be(2019);
            responseContent1.GetProperty("data").GetProperty("price").GetDouble().Should().Be(1849.99);
            responseContent1.GetProperty("data").GetProperty("cpU_model").GetString().Should().Be("Intel Core i9");
            responseContent1.GetProperty("data").GetProperty("hard_disk_size").GetString().Should().Be("1 TB");

            Test.Log(Status.Pass, "Assertions passed");
        }
    }
}