using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;


namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SellerService sellerService, DepartmentService departmentService, SalesRecordService salesRecordService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _salesRecordService = salesRecordService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime today = DateTime.Today;
            DateTime yestarday = new DateTime(today.Year, today.Month, today.Day - 1);
            DateTime dateMinSale = await _salesRecordService.MinDateSaleAsync();
            ViewData["DefaultDataMax"] = today.ToString("yyyy-MM-dd");
            ViewData["DefaultDataMin"] = dateMinSale.ToString("yyyy-MM-dd");

            SelectorViewModel viewModel = new SelectorViewModel() { Departments = await _departmentService.FindAllAsync(), Sellers = await _sellerService.FindAllAsync() };
            return View(viewModel);
        }
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {           
            ViewData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }
        public async Task<IActionResult> SearchByFilter(DateTime? minDate, DateTime? maxDate, Seller seller, Department department)
        {
            ViewData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByFilterAsync(minDate, maxDate, seller, department);
            SelectorViewModel viewModel = new SelectorViewModel()
            { Departments = await _departmentService.FindAllAsync(), Sellers = await _sellerService.FindAllAsync(),Sales = result };
         
            return View(viewModel);
        }
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate, int grouping)
        {
            ViewData["MinDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["MaxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await  _salesRecordService.FindByDateGroupingAsync(minDate, maxDate, grouping);
            return View(result);
        }
    }
}
