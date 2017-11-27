using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Vezba.Entity
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please insert a Customer Name.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please insert a Contact Name.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Please insert a Address.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please insert a City.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please insert a Postal Code.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please insert a Country.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string Country { get; set; }
    }
}
