using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static LMSG3.Web.Controllers.LiteraturesController;

namespace LMSG3.Web.Controllers
{

    public class AuthorsController : Controller
    {
        private HttpClient httpClient;
        private const string json = "application/json";
        private readonly IHttpClientFactory httpClientFactory;
        public AuthorsController(IHttpClientFactory httpClientFactory)
        {

            httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });
            var ApiUrl = ConfigurationHelper.GetByName("ApiUrl");
            httpClient.BaseAddress = new Uri(ApiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;
        }

        // GET: AuthorsController
        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter)
        {
            

            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fullName_desc" : "";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
           // ViewData["DateOfBirthSortParm"] = sortOrder == "DateOfBirth" ? "DateOfBirth_desc" : "DateOfBirth";
            ViewData["CountLiteraturesSortParm"] = sortOrder == "CountLiteratures" ? "CountLiteraturesSortParm_desc" : "CountLiteratures";
            ViewData["LatestWorkSortParm"] = sortOrder == "LatestWork" ? "latestWork_desc" : "LatestWork";
            
            AuthorResourcesParameters authorResourceParameters = new AuthorResourcesParameters();
            authorResourceParameters.sortOrder = sortOrder;
            authorResourceParameters.searchString = searchString;
            authorResourceParameters.includeAllInfo = true;
            //if (currentFilter == null)
            //{
            //    authorResourceParameters.levelFilter = 0;

            //}
            //else
            //{
            //    authorResourceParameters.levelFilter = int.Parse(currentFilter);
            //}
            var cancellation = new CancellationTokenSource();
            var authorModel = await SimpleGet(authorResourceParameters);

           


            return View(authorModel);
        }

        private async Task<IEnumerable<LiteratureAuthorDto>> SimpleGet(AuthorResourcesParameters authorResourceParameters)
        {

            var queryString = $"includeAllInfo={authorResourceParameters.includeAllInfo}";
            queryString += $"&searchString={authorResourceParameters.searchString}";
            queryString += $"&sortOrder={authorResourceParameters.sortOrder}";
            var apiUrl = $"api/LiteratureAuthors?{queryString}";

            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var authors = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureAuthorDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return authors;

        }

        // GET: AuthorsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await httpClient.GetAsync($"api/literatureAuthors/{id}?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var author = System.Text.Json.JsonSerializer.Deserialize<LiteratureAuthorDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);


            return View(author);
        }

        // GET: AuthorsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
