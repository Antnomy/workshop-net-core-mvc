using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exeptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }
        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }
        public async Task<Seller> FindByIdAsync(int? id)
        {
            return await _context.Seller.Include(m => m.Department).FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            //var obj = FindByIdAsync(id);
            var obj = await _context.Seller.FindAsync(id);

            try
            {
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new IntegrityException("Can't Delete Seller because he/she has Sales");
            }
        }
        public async Task UpdateAsync(Seller seller)
        {
            if (!await _context.Seller.AnyAsync(s => s.Id == seller.Id))
            {
                throw new NotFoundException("Id Not Found");
            }
            try
            {
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException("A error ocured" + e.Message);
            }
        }
    }
}
