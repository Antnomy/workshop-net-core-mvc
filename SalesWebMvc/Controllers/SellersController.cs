﻿using System;
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

        public async Task<IActionResult> Index()
        {
            return View(await _sellerService.FindAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = new SellerFormViewModel { Departments = await _departmentService.FindAllAsync() };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new SellerFormViewModel { Departments = await _departmentService.FindAllAsync() };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }
            var seller= await _sellerService.FindByIdAsync(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not found!" });
            }
            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new {e.Message });
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }
            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not found!" });
            }
            return View(seller);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Provided!" });
            }           
            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Not Found!" });
            }

            var viewModel = new SellerFormViewModel {Seller = seller, Departments = await _departmentService.FindAllAsync() };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new SellerFormViewModel { Departments = await _departmentService.FindAllAsync() };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Missmatch!" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
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
