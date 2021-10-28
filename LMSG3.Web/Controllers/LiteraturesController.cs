using Azure.Core;
using LMSG3.Api.ResourceParameters;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Authorization;
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
using static System.Net.Mime.MediaTypeNames;

namespace LMSG3.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
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
            var response = await  httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatures = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return literatures;

        }



        // GET: LiteraturesController1/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var imageNumber = 100;
            var response = await httpClient.GetAsync($"api/literatures/{id}?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literature = System.Text.Json.JsonSerializer.Deserialize<LiteratureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);
            //literature.CoverLImg = "https://covers.openlibrary.org/b/id/5546156-L.jpg";
            //literature.CoverLImg = "~/Thumbnails/Large/10738025-L";
            literature.CoverLImg = $"/img/Thumbnails/Large/{imageNumber}-L.jpg";

            return View(literature);
        }

        // GET: LiteraturesController1/Create
        public ActionResult Create(LiteratureDto literatureDto)
        {
            if (literatureDto.Title != null)
            {
                ViewBag.ErrorMessage = "Title already exist";
                return View(literatureDto);
            }
            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: LiteraturesController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(IFormCollection collection)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLiterature(IFormCollection form, LiteratureDto literatureDto)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Post, "api/literatures");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            //var AuthorIds = form["authorId"];
            var AuthorFirstName = form["FirstName"];
            var AuthorLastName = form["LastName"];
            var AuthorBirthDay = form["DateOfBirth"];
            var authorBirthDay1 = new DateTime();
            var authorBirthDay2 = new DateTime();
            var authorBirthDay3 = new DateTime();
            for (int d = 0; d < AuthorFirstName.Count; d++)
            {
                if (d == 0)
                {
                    var dateArray1 = AuthorBirthDay[d].Split("-");
                    var year = int.Parse(dateArray1[0]);
                    var month = int.Parse(dateArray1[1]);
                    string dayStr = dateArray1[2];
                    var dayStrArr = dayStr.Split("T");
                    var day = int.Parse(dayStrArr[0]);
                    authorBirthDay1 = new DateTime(year, month, day);
                }
                if (d == 1 && AuthorFirstName[1] != "")
                {
                    var dateArray2 = AuthorBirthDay[d].Split("-");
                    var year = int.Parse(dateArray2[0]);
                    var month = int.Parse(dateArray2[1]);
                    string dayStr = dateArray2[2];
                    var dayStrArr = dayStr.Split("T");
                    var day = int.Parse(dayStrArr[0]);
                    authorBirthDay2 = new DateTime(year, month, day);
                    //authorBirthDay2 = new DateTime(int.Parse(dateArray2[0]), int.Parse(dateArray2[1]), int.Parse(dateArray2[2]));
                }
                if (d == 2 && AuthorFirstName[2] != "")
                {
                    var dateArray3 = AuthorBirthDay[d].Split("-");
                    var year = int.Parse(dateArray3[0]);
                    var month = int.Parse(dateArray3[1]);
                    string dayStr = dateArray3[2];
                    var dayStrArr = dayStr.Split("T");
                    var day = int.Parse(dayStrArr[0]);
                    authorBirthDay3 = new DateTime(year, month, day);
                    //authorBirthDay3 = new DateTime(int.Parse(dateArray3[0]), int.Parse(dateArray3[1]), int.Parse(dateArray3[2]));
                }
              
            }
            
            List<LiteratureAuthorDto> authorsList = new List<LiteratureAuthorDto>();

            for (int i = 0; i < AuthorFirstName.Count; i++)
            {
                var author = new LiteratureAuthorDto();

                //author = new LiteratureAuthor {
                //author.Id = int.Parse(AuthorIds[i]);
                author.FirstName = AuthorFirstName[i];
                author.LastName = AuthorLastName[i];
                if (i == 0 && author.FirstName != "" && author.LastName != "")
                {
                    author.DateOfBirth = authorBirthDay1;
                    authorsList.Add(author);
                }
                if (i == 1 && author.FirstName != "" && author.LastName != "")
                {
                    author.DateOfBirth = authorBirthDay2;
                    authorsList.Add(author);
                }
                if (i == 2 && author.FirstName != "" && author.LastName != "")
                {
                    author.DateOfBirth = authorBirthDay3;
                    authorsList.Add(author);
                }

               
                //literatureDto.Authors.Add(author);

            }
            literatureDto.Authors = authorsList;
            request.Content = JsonContent.Create(literatureDto, typeof(LiteratureDto), new MediaTypeHeaderValue(json));
            var response = await httpClient.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                
                //return RedirectToAction(nameof(Create, new { literatureDto });
                return RedirectToAction(nameof(Create), literatureDto);
            }
           
           
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
        public async Task<ActionResult> Delete(int id)
        {
            var response = await httpClient.GetAsync($"api/literatures/{id}?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatureDto = System.Text.Json.JsonSerializer.Deserialize<LiteratureDto>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);


            return View(literatureDto);
            
        }

        // POST: LiteraturesController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLiterature(int id, IFormCollection collection)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/literatures/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
