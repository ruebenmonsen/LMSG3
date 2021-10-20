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

            httpClient.BaseAddress = new Uri("https://localhost:44301");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;
        }

        // GET: AuthorsController
        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fullName_desc" : "";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["DateOfBirthSortParm"] = sortOrder == "DateOfBirth" ? "DateOfBirth_desc" : "DateOfBirth";
            ViewData["CountLiteraturesSortParm"] = sortOrder == "CountLiteratures" ? "CountLiteraturesSortParm_desc" : "DateOfBirth";


            var cancellation = new CancellationTokenSource();
            var authorModel = await SimpleGet();

            if (!String.IsNullOrEmpty(searchString))
            {
                authorModel = authorModel.Where(s => s.FullName.ToLower().Contains(searchString.ToLower().Trim())
                                                    || s.Age.Equals(searchString.ToLower().Trim())
                                                    || s.DateOfBirth.ToString().Contains(searchString.ToLower().Trim()));


                //Todo Add countfor each creteria result
            }


            switch (sortOrder)
            {
                case "fullName_desc":
                    authorModel = authorModel.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "Age":
                    authorModel = authorModel.OrderBy(s => s.Age).ToList();
                    break;
                case "age_desc":
                    authorModel = authorModel.OrderByDescending(s => s.Age).ToList();
                    break;
                case "DateOfBirth":
                    authorModel = authorModel.OrderBy(s => s.DateOfBirth).ToList();
                    break;
                case "dateOfBirth_desc":
                    authorModel = authorModel.OrderByDescending(s => s.DateOfBirth).ToList();
                    break;
                case "CountLiteraturesSortParm":
                    authorModel = authorModel.OrderBy(s => s.DateOfBirth).ToList();
                    break;
                case "countLiteraturesSortParm_desc":
                    authorModel = authorModel.OrderByDescending(s => s.DateOfBirth).ToList();
                    break;
                default:
                    authorModel = authorModel.OrderBy(s => s.FullName).ToList();
                    break;

            }
            // IEnumerable<SelectListItem> lieratureLevelSlectItems = await GetVehicleLevelSelectListItems();

            // return View(viewModels, IEnumerable<SelectListItem>>(viewModels, lieratureLevelSlectItems SelectItems));

            //return View(authorModel);

            // int pageSize = 10;
            //return View(await PaginatedList<LiteratureDto>.CreateAsync(authorModel.AsNoTracking(), pageNumber ?? 1, pageSize));

            return View(authorModel);
        }

        private async Task<IEnumerable<LiteratureAuthorDto>> SimpleGet()
        {

            var response = await httpClient.GetAsync("api/LiteratureAuthors?includeAllInfo=true");
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
