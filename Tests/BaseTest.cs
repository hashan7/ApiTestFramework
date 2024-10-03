using RestSharp;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Text.Json;
using Assert = NUnit.Framework.Assert;
using FluentAssertions;

namespace ApiTestingProject.Tests
{
    
    [TestFixture]
    public class BaseTest
    {
        protected RestClient Client;

        protected static AventStack.ExtentReports.ExtentReports Extent;
        protected static ExtentTest Test;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var htmlReporter = new ExtentHtmlReporter("extent.html");
            Extent = new AventStack.ExtentReports.ExtentReports();
            Extent.AttachReporter(htmlReporter);

            Client = new RestClient("https://api.restful-api.dev");

            
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                Extent.Flush();
            }
            catch (TypeLoadException ex)
            {
                Console.WriteLine("Error during TearDown: " + ex.Message);
            }
        }

    }
}