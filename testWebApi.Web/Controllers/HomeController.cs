using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using testWebApi.Web.Models;

namespace testWebApi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task <IActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7038");
            HttpResponseMessage response =await client.GetAsync("api/employee");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(results);
            }


            return View(employees);
        }


        public async Task<IActionResult> Detail(int id)
        {
            Employee employees = await GetmployeeById(id);

            return View(employees);
        }

        private static async Task<Employee> GetmployeeById(int id)
        {
            Employee employees = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7038");
            HttpResponseMessage response = await client.GetAsync($"api/employee/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<Employee>(results);
            }

            return employees;
        }

        public async Task<IActionResult> Delete(int id)
        {
            Employee employees = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7038");
            HttpResponseMessage response = await client.DeleteAsync($"api/employee/{id}");
            if (response.IsSuccessStatusCode)
            {
              return RedirectToAction("Index") ;
            }


            return View();
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }     
        
        [HttpPost]

        public async Task<IActionResult> Create(Employee employee)
        {
            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7038");
            var response = await client.PostAsJsonAsync("api/employee",employee);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]


        public async Task<IActionResult> Edit(int id)
        {
           Employee employee = await GetmployeeById(id);
            return View(employee);
        }
             
        
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7038");
            var response = await client.PutAsJsonAsync($"api/employee/{employee.Id}", employee);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
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