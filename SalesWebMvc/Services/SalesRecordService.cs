using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;
        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from s in _context.SalesRecord select s;
            if (minDate.HasValue)
            {
                result = result.Where(s => s.Data >= minDate);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(s => s.Data <= maxDate);
            }
            return await result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Data)
                .ToListAsync();
        }
        public async Task<List<SalesRecord>> FindByFilterAsync(DateTime? minDate, DateTime? maxDate, Seller seller, Department department)
        {
            var result = (from s in _context.SalesRecord select s)
              .Include(s => s.Seller)
              .Include(s => s.Seller.Department).Select(s => s); 

            if (minDate.HasValue)
            {
                result = result.Where(s => s.Data >= minDate);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(s => s.Data <= maxDate);
            }
            if(seller.Id != 0)
            {
                result = result.Where(s => s.Seller.Id == seller.Id);
            }
            if (department.Id != 0)
            {
                result = result.Where(s => s.Seller.Department.Id == department.Id);
            }
            return await result              
                .OrderByDescending(s => s.Data)
                .ToListAsync();
        }
        public List<IGrouping<Department, SalesRecord>> FindByDateGrouping(DateTime? minDate, DateTime? maxDate)
        {
            var result = from s in _context.SalesRecord select s;
            if (minDate.HasValue)
            {
                result = result.Where(s => s.Data >= minDate);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(s => s.Data <= maxDate);
            }
            return result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Seller.Department)
                .GroupBy(s => s.Seller.Department)
                .ToList();               
        }
    }
}
