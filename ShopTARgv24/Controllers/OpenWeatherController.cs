using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.ServiceInterface;
using System.Threading.Tasks;

namespace ShopTARgv24.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IOpenWeatherService _service;
        public OpenWeatherController(IOpenWeatherService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Index(string city)
        {
            ViewBag.City = city ?? "";
            var model = string.IsNullOrWhiteSpace(city)
                ? null
                : await _service.GetCurrentAsync(city);

            return View(model);
        }
    }
}
