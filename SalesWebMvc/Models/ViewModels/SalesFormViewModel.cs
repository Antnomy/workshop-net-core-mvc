using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models.ViewModels
{
    public class SalesFormViewModel
    {
        public IEnumerable<Seller> Sellers { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<SalesRecord> Sales { get; set; }
        public Seller Seller { get; set; }
        public Department Department{ get; set; }
        public IGrouping<Department, SalesRecord> SalesByDepartments{ get; set; }
        public IGrouping<Seller, SalesRecord> SalesBySeller { get; set; }
        public IGrouping<DateTime, SalesRecord> SalesByDate { get; set; }
    }
}
