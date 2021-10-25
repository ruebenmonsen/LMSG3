using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static LMSG3.Web.Services.LiteraturesController;

namespace LMSG3.Web.Controllers
{
    public partial class LiteraturesController : Controller
    {
        private HttpClient httpClient;
        private const string json = "application/json";
        private readonly IHttpClientFactory httpClientFactory;

        // GET: LiteraturesController1
        public LiteraturesController(IHttpClientFactory httpClientFactory)
        {
            //var client = httpClientFactory.CreateClient();

            httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });
            var ApiUrl = ConfigurationHelper.GetByName("ApiUrl");
            httpClient.BaseAddress = new Uri(ApiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;

        }


        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter)
        {

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ReleaseDateSortParm"] = sortOrder == "ReleaseDate" ? "releaseDate_desc" : "ReleaseDate";
            ViewData["SubjectSortParm"] = sortOrder == "Subject" ? "subject_desc" : "Subject";
            ViewData["LevelSortParm"] = sortOrder == "Level" ? "level_desc" : "Level";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";

            LiteraturesResourceParameters literaturesResourceParameters = new LiteraturesResourceParameters();
            literaturesResourceParameters.sortOrder = sortOrder;
            literaturesResourceParameters.searchString = searchString;
            literaturesResourceParameters.includeAllInfo = true;
            if (currentFilter == null)
            {
                literaturesResourceParameters.levelFilter = 0;

            }
            else
            {
                literaturesResourceParameters.levelFilter = int.Parse(currentFilter);
            }
            
            

            var cancellation = new CancellationTokenSource();
            
            var litratureModel = await SimpleGet(literaturesResourceParameters);

            
            return View(litratureModel);
        }

        private async Task<IEnumerable<LiteratureDto>> SimpleGet(LiteraturesResourceParameters literaturesResourceParameters)
        {
            var queryString = $"includeAllInfo={literaturesResourceParameters.includeAllInfo}";
            queryString += $"&levelFilter={literaturesResourceParameters.levelFilter}";
            queryString += $"&searchString={literaturesResourceParameters.searchString}";
            queryString += $"&sortOrder={literaturesResourceParameters.sortOrder}";
            var apiUrl = $"api/literatures?{queryString}";
            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatures = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return literatures;

        }



        private async Task<IEnumerable<LiteratureDto>> GetWithStream()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/literatures?includeAllInfos=false");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            IEnumerable<LiteratureDto> literatureDtos;


            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                response.EnsureSuccessStatusCode();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new Newtonsoft.Json.JsonSerializer();
                        literatureDtos = serializer.Deserialize<IEnumerable<LiteratureDto>>(jsonReader);
                    }
                }
            }

            return literatureDtos;

        }

        // GET: LiteraturesController1/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await httpClient.GetAsync($"api/literatures/{id}?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literature = System.Text.Json.JsonSerializer.Deserialize<LiteratureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);


            return View(literature);
        }

        // GET: LiteraturesController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiteraturesController1/Create
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

        public async Task<ActionResult> CreateLiterature(LiteratureDto literatureDto)
        {
            var said = "wow";
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44301/api/literatures");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            //var authors = new LiteratureAuthor
            //{
            //    FirstName = "Create from client",
            //    LastName = "Bouazza",
            //    DateOfBirth = new DateTime(1940, 12, 31)
            //};

            request.Content = JsonContent.Create(literatureDto, typeof(LiteratureDto), new MediaTypeHeaderValue(json));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var codeEvents = System.Text.Json.JsonSerializer.Deserialize<Literature>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //return codeEvents;
            return RedirectToAction(nameof(Index));


        }

        // GET: LiteraturesController1/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await httpClient.GetAsync($"api/literatures/{id}?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatureDto = System.Text.Json.JsonSerializer.Deserialize<LiteratureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);


            return View(literatureDto);
            
        }

        public async Task<ActionResult> EditLiterature(IFormCollection form, Literature literatureDto)
        {
            
            foreach (string key in form.Keys)
            {
                var Key = form[key];

            }
            var AuthorIds = form["authorId"];
            var AuthorFirstName = form["FirstName"];
            var AuthorLastName = form["LastName"];
            var AuthorBirthDay = form["DateOfBirth"];


            List<LiteratureAuthor> authorsList = new List<LiteratureAuthor>();

            for (int i = 0; i < AuthorIds.Count; i++)
            {
                var author = new LiteratureAuthor();
               
                //author = new LiteratureAuthor {
                //author.Id = int.Parse(AuthorIds[i]);
                author.FirstName = AuthorFirstName[i];
                author.LastName = AuthorLastName[i];
                author.DateOfBirth = new DateTime(1995, 12, 31);
                authorsList.Add(author);
                //literatureDto.Authors.Add(author);

            }
            literatureDto.Authors = authorsList;
            var literarure = System.Text.Json.JsonSerializer.Serialize(literatureDto);
            
            var requestContent = new StringContent(literarure, Encoding.UTF8, "application/json");
            // var uri = Path.Combine("companies", "fc12c11e-33a3-45e2-f11e-08d8bdb38ded");
            
             var response = await httpClient.PutAsync($"api/literatures/{literatureDto.Id}", requestContent);
            response.EnsureSuccessStatusCode();
            //UpdateAuthor(id, authorId, FirstName,LastName, DateOfBirth);
            return RedirectToAction(nameof(Index));


        }

        private void UpdateAuthor(int id, int authorId, string firstName, string lastName, DateTime dateOfBirth)
        {
            var authorLiterature = new LiteratureAuthor();
            authorLiterature.Id = authorId;
            authorLiterature.FirstName = firstName;
            authorLiterature.LastName = lastName;
            authorLiterature.DateOfBirth = dateOfBirth;
           
            var author = System.Text.Json.JsonSerializer.Serialize(authorLiterature);
            var requestContent = new StringContent(author, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync($"api/literatureAuthors/{authorId}", requestContent);
            //response.EnsureSuccessStatusCode();
        }

        // POST: LiteraturesController1/Edit/5
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

        // GET: LiteraturesController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiteraturesController1/Delete/5
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
