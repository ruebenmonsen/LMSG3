using LMSG3.Api;
using LMSG3.Core.Models.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public class LiteratureSelectService : ILiteratureSelectService
    {
        private HttpClient httpClient;
        private const string json = "application/json";
        private readonly IHttpClientFactory httpClientFactory;

        public LiteratureSelectService(IHttpClientFactory httpClientFactory)
        {
            httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });

            httpClient.BaseAddress = new Uri("https://localhost:44301");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<SelectListItem>> GetLiteratureLeves()
        {

            var response = await httpClient.GetAsync("api/LiteratureLevels");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var levels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureLevelDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

             return levels.Select(g => new SelectListItem
             {
                 Text = g.Name.ToString(),
                 Value = g.Id.ToString()
             }); 
            
                     
                      
        }
        
        public async Task<IEnumerable<SelectListItem>> GetLiteraturesTypes()
        {

            var response = await httpClient.GetAsync("api/LiteratureTypes");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var levels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureLevelDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return levels.Select(g => new SelectListItem
            {
                Text = g.Name.ToString(),
                Value = g.Id.ToString()
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetLiteraturesSubjects() 
        {

            var response = await httpClient.GetAsync("api/LiteratureSubjects");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var levels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<LiteratureLevelDto>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Newtonsoft json
            // var literatures = JsonConvert.DeserializeObject<IEnumerable<LiteratureDto>>(content);

            return levels.Select(g => new SelectListItem
            {
                Text = g.Name.ToString(),
                Value = g.Id.ToString()
            });
        }

        
    }
}
