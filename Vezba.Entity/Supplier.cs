using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezba.Entity
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Please insert a Supplier Name.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Please insert a Contact Name.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }


        [Required(ErrorMessage = "Please insert a Address.")]
        [StringLength(150, ErrorMessage = "Max lengt 150.")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please insert a City.")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please insert a Postal Code.")]
        [StringLength(15, ErrorMessage = "Max lengt 15.")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please insert a Country.")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        [DisplayName("Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please insert a Phone.")]
        [StringLength(15, ErrorMessage = "Max lengt 15.")]
        [DisplayName("Phone")]
        public string Phone { get; set; }
    }
}
