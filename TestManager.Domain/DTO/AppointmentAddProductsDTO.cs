using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO
{
    public class AppointmentAddProductsDTO
    {
        public int TransactionId { get; set; }
        public List<AddProduct> Products { get; set; } = new();

        public  bool JoinProducts { get; set; }
    }

    public class AddProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? AccessionNumber { get; set; }
    }
}
