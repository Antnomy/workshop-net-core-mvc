using System.Collections.Generic;


namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public IEnumerable< Department> Departments { get; set; }

    }
}
