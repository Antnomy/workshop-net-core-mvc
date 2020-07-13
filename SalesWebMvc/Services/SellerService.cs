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
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }
        public Seller FindById(int? id)
        {
            return _context.Seller.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
        }
        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            //var obj = FindById(id);
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
        public void Update(Seller seller)
        {
            if (!_context.Seller.Any(s => s.Id == seller.Id))
            {
                throw new NotFoundException("Id Not Found");
            }
            try
            {
                _context.Seller.Update(seller);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException("A error ocured" + e.Message);
            }
        }
    }
}
