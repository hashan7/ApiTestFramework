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
    public class DeleteObjectTest : BaseTest
    {
        private string objectId;

        [Test]
        public async Task DeleteObject()
        {
            Test = Extent.CreateTest("TestDeleteObject");

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
            Console.WriteLine("ResponseContent: " + responseContent);

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

            var requestPut = new RestRequest($"/objects/{objectId}", Method.Put);
            requestPut.AddJsonBody(requestBodyPut);
            var responsePut = await Client.ExecuteAsync(requestPut);
            var jsonResponseContent = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responsePut.Content);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var prettyJson = System.Text.Json.JsonSerializer.Serialize(jsonResponseContent, options);
            Console.WriteLine("Get the updated object: \n" + prettyJson);
            
            // Act

            var deleteRequest = new RestRequest($"/objects/{objectId}", Method.Delete);
            var deleteResponse = await Client.ExecuteAsync(deleteRequest);
            var jsonResponseOfDeleteContent = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(deleteResponse.Content);
            var optionsDelete = new JsonSerializerOptions { WriteIndented = true };
            var prettyDeleteJson = System.Text.Json.JsonSerializer.Serialize(jsonResponseOfDeleteContent, optionsDelete);
            

            // Assert
            
            Console.WriteLine("DeleteResponse.Content " + deleteResponse.Content);
            Console.WriteLine("DeleteResponse.Content json " + prettyDeleteJson);

            responsePut.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responsePut.ContentType.Should().Be("application/json");
            deleteResponse.Content.Should().Contain("message");
            jsonResponseOfDeleteContent.GetProperty("message").GetString().Should().Be($"Object with id = {objectId} has been deleted.");
            
            Test.Log(Status.Pass, "Assertions passed");
        }
    }
}