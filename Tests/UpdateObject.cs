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
    public class UpdateObjectTest : BaseTest
    {
        private string objectId;

        [Test]
        public async Task UpdateObject()
        {
            Test = Extent.CreateTest("TestUpdateObject");

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

            var requestBodyPut = new
            {
                name = "Apple MacBook Pro 16",
                data = new
                {
                    year = 2019,
                    price = 2049.99,
                    cpU_model = "Intel Core i9",
                    hard_disk_size = "1 TB",
                    color = "silver"
                }
            };
            
            // Act

            var requestPut = new RestRequest($"/objects/{objectId}", Method.Put);
            requestPut.AddJsonBody(requestBodyPut);
            var responsePut = await Client.ExecuteAsync(requestPut);
            var jsonResponseContent = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responsePut.Content);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var prettyJson = System.Text.Json.JsonSerializer.Serialize(jsonResponseContent, options);
            Console.WriteLine("Get the updated object: \n" + prettyJson);

            // Assert

            responsePut.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responsePut.ContentType.Should().Be("application/json");
            jsonResponseContent.GetProperty("id").GetString().Should().Be((string)objectId);
            jsonResponseContent.GetProperty("name").GetString().Should().Be("Apple MacBook Pro 16");  
            jsonResponseContent.GetProperty("data").GetProperty("year").GetInt32().Should().Be(2019);  
            jsonResponseContent.GetProperty("data").GetProperty("price").GetDouble().Should().Be(2049.99); // price updated
            jsonResponseContent.GetProperty("data").GetProperty("cpU_model").GetString().Should().Be("Intel Core i9");  
            jsonResponseContent.GetProperty("data").GetProperty("hard_disk_size").GetString().Should().Be("1 TB");  
            jsonResponseContent.GetProperty("data").GetProperty("color").GetString().Should().Be("silver"); // color updated
            responsePut.Content.Should().Contain("color");
            responsePut.Content.Should().Contain("updatedAt");

            Test.Log(Status.Pass, "Assertions passed");
        }
    }
}