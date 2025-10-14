﻿using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.ServiceInterface;
using System;

namespace ShopTARgv24.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _weatherForecastServices;
   
        public WeatherController
            (
            IWeatherForecastServices weatherForecastServices
            )
        {
            _weatherForecastServices = weatherForecastServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchCity()
        { 
        return View();
        }
    }
}
