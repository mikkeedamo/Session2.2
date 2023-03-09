using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Session2._2
{
    [TestClass]
    public class UnitTest1
    {

        private static RestClient restClient;
        private static readonly string BaseUrl = "https://petstore.swagger.io/v2/";
        private static readonly string endpoint = "pet";
        private static string GetURL(string endpoint) => $"{BaseUrl}{endpoint}";
        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<PetModel> cleanUpList = new List<PetModel>();


        [TestInitialize]
        public async Task TestInitialize()
        {
            restClient = new RestClient();
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var deleteRequest = new RestRequest(GetURI($"{endpoint}/{data.Id}"));
                var deleteResponse = await restClient.DeleteAsync(deleteRequest);

          
            }

        }


        [TestMethod]
        public async Task TestMethod1()
        {
            PetModel pet = new PetModel()
            {
                Category = new CategoryModel { Name = "Labrador" },
                Name = "Choco",
                PhotoUrls = new string[] { "photo " },
                Tags = new TagModel[] { new TagModel { Name = "Tags" } },
                Status = "available"
            };

            
            var payload = new RestRequest(GetURI(endpoint)).AddJsonBody(pet);
            var response = await restClient.ExecutePostAsync<PetModel>(payload);

            cleanUpList.Add(response.Data);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code is not equal");
            Assert.AreEqual(pet.Name, response.Data.Name, "Name is not equal");
            Assert.AreEqual(pet.Category.Name, response.Data.Category.Name, "Category name is not equal");
            Assert.AreEqual(pet.PhotoUrls[0], response.Data.PhotoUrls[0], "PhotoUrls is not equal");
            Assert.AreEqual(pet.Tags[0].Name, response.Data.Tags[0].Name, "Tag name is not equal");
            Assert.AreEqual(pet.Status, response.Data.Status, "Status is not equal");


        }
    }
}