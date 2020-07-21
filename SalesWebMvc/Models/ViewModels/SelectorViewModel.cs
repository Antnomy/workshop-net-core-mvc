using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    public class SelectorViewModel
    {
        public IEnumerable<Seller> Sellers { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public Seller Seller { get; set; }
        public Department Department{ get; set; }
    }
}
