using LMSG3.Core.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    public class LiteraturesController : Controller
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


        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DescriptionSortParm"] = sortOrder == "FullName" ? "description_desc" : "Description";
            ViewData["ReleaseDateSortParm"] = sortOrder == "ReleaseDate" ? "releaseDate_desc" : "ReleaseDate";
            ViewData["SubjectSortParm"] = sortOrder == "Subject" ? "subject_desc" : "Subject";
            ViewData["LevelSortParm"] = sortOrder == "Level" ? "level_desc" : "Level";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";

            var cancellation = new CancellationTokenSource();
            var litratureModel = await SimpleGet();

            if (!String.IsNullOrEmpty(searchString))
            {
                litratureModel = litratureModel.Where(s => s.Title.ToLower().Contains(searchString.ToLower().Trim())
                                                    || s.SubjectName.ToLower().Contains(searchString.ToLower().Trim())
                                                    || s.Description.ToLower().Contains(searchString.ToLower().Trim())
                                                    || s.ReleaseDate.ToString().Contains(searchString.ToLower().Trim())
                                                    || s.LevelName.ToLower().Contains(searchString.ToLower().Trim())
                                                    //|| s.LiteraLevelId.Equals(currentFilter)
                                                    || s.LiteraTypeName.ToLower().Contains(searchString.ToLower().Trim()));


                //Todo Add countfor each creteria result
            }
            // var authors = litratureModel.Where(d => d.Authors.FirstOrDefault);
            //  var authors = from s in litratureModel.Where(a => a.Authors != null) select s.Authors;

            if (!String.IsNullOrEmpty(currentFilter))
            {
                litratureModel = litratureModel.Where(s => s.LiteraLevelId.Equals(Int16.Parse(currentFilter)));

            }
            switch (sortOrder)
            {
                case "title_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Description":
                    litratureModel = litratureModel.OrderBy(s => s.Description).ToList();
                    break;
                case "description_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.Description).ToList();
                    break;
                case "ReleaseDate":
                    litratureModel = litratureModel.OrderBy(s => s.ReleaseDate).ToList();
                    break;
                case "releaseDate_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.ReleaseDate).ToList();
                    break;
                case "Subject":
                    litratureModel = litratureModel.OrderBy(s => s.SubjectName).ToList();
                    break;
                case "subject_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.SubjectName).ToList();
                    break;
                case "Level":
                    litratureModel = litratureModel.OrderBy(s => s.LevelName).ToList();
                    break;
                case "level_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.LevelName).ToList();
                    break;
                case "Type":
                    litratureModel = litratureModel.OrderBy(s => s.LiteraTypeName).ToList();
                    break;
                case "type_desc":
                    litratureModel = litratureModel.OrderByDescending(s => s.LiteraTypeName).ToList();
                    break;
                default:
                    litratureModel = litratureModel.OrderBy(s => s.Title).ToList();
                    break;
            }
            // IEnumerable<SelectListItem> lieratureLevelSlectItems = await GetVehicleLevelSelectListItems();

            // return View(viewModels, IEnumerable<SelectListItem>>(viewModels, lieratureLevelSlectItems SelectItems));

            //return View(litratureModel);

            // int pageSize = 10;
            //return View(await PaginatedList<LiteratureDto>.CreateAsync(litratureModel.AsNoTracking(), pageNumber ?? 1, pageSize));

            return View(litratureModel);
        }

        private async Task<IEnumerable<LiteratureDto>> SimpleGet()
        {
            
            var response = await httpClient.GetAsync("api/literatures?includeAllInfo=true");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatures = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return literatures;

        }

        public static class ConfigurationHelper
        {
            public static string GetByName(string configKeyName)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                IConfigurationSection section = config.GetSection(configKeyName);

                return section.Value;
            }
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

        // GET: LiteraturesController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
