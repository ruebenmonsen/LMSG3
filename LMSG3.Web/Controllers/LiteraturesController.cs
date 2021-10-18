using LMSG3.Core.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            httpClient.BaseAddress = new Uri("https://localhost:44301");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;
           
        }

        
        public async Task<ActionResult> Index()
        {
            var cancellation = new CancellationTokenSource();
            var res = await SimpleGet();
            //var res = await GetWithStream();
            //return View(Json(res));
            return View(res);


        }

        private async Task<IEnumerable<LiteratureDto>> SimpleGet()
        {

            var response = await httpClient.GetAsync("api/literatures?includeAllInfos=false");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var literatures = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
           // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return  literatures;

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
        public ActionResult Details(int id)
        {
            return View();
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
