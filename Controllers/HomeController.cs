using HttpAPIConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpAPIConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index2()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(HttpMethod.Get,"/api/v2/Faculty/GetAllFaculty/");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            //start
            //var responseString = await response.Content.ReadAsStringAsync();
            //List<Faculty> fac = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Faculty>>(responseString);
            //end

           

            var responseStream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            List<Faculty> fac2 = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Faculty>>(responseStream, options);

            return View(fac2);
        }
        //Edit
        public async Task<IActionResult> Edit(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v2/Faculty/GetFaculty/"+id);

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Faculty fac = Newtonsoft.Json.JsonConvert.DeserializeObject<Faculty>(responseString);
            return View(fac);


        }
        //EditSave
        public async Task<IActionResult> EditSave([FromForm]  Faculty faculty)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v2/Faculty/updateFaculty/" + faculty.Facultyid);

            // serialize it
            var serializedImageForUpdate = System.Text.Json.JsonSerializer.Serialize(faculty);
            request.Content = new StringContent(serializedImageForUpdate,System.Text.Encoding.Unicode,"application/json");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);



            response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();
            //List<Faculty> fac = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Faculty>>(responseString);


            return RedirectToAction("Index2");


        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
