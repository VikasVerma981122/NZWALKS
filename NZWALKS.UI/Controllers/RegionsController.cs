using Microsoft.AspNetCore.Mvc;
using NZWALKS.UI.Models;
using NZWALKS.UI.Models.DTO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWALKS.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7261/api/region");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
                
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();
            var HttpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7261/api/region"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(HttpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if(response is not null) 
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7261/api/region/{id.ToString()}");
        //ViewBag.Id = id;---> trial basis to check whether edit button is working fine or not!......
            if(response is not null) 
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();
            var httprequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7261/api/region/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httprequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if(response is not null) 
            {
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var HttpResponseMessage = await client.DeleteAsync($"https://localhost:7261/api/region/{request.Id}");
                HttpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

               
            }

            return View("Edit");
        }
    }

}
