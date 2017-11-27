using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezba.Entity
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please insert a Product Name.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        public int SupplierID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required(ErrorMessage ="Please insert a Unit")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Please insert a Price")]
        public decimal Price { get; set; }
    }
}
