using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }
        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            DateTime yestarday = new DateTime(today.Year, today.Month, today.Day - 1);
            ViewData["DefaultDataMax"] = today.ToString("yyyy-MM-dd");
            ViewData["DefaultDataMin"] = yestarday.ToString("yyyy-MM-dd");
            return View();
        }
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            
            ViewData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }
        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}
