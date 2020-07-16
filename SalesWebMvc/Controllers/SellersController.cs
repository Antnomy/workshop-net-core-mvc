using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exeptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            return View(_sellerService.FindAll());
        }
        public IActionResult Create()
        {
            var viewModel = new SellerFormViewModel { Departments = _departmentService.FindAll() };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new SellerFormViewModel { Departments = _departmentService.FindAll() };
                return View(viewModel);
            }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
       
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }
            var seller= _sellerService.FindById(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not found!" });
            }
            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            _sellerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }
            var seller = _sellerService.FindById(id);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not found!" });
            }
            return View(seller);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }           
            var seller = _sellerService.FindById(id);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Found!" });
            }

            var viewModel = new SellerFormViewModel {Seller = seller, Departments = _departmentService.FindAll() };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new SellerFormViewModel { Departments = _departmentService.FindAll() };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Missmatch!" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { e.Message });
            }           
        }
        public IActionResult Error(string message)
        {
            ErrorViewModel errorView = new ErrorViewModel() { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            return View(errorView);
        }
    }
}
