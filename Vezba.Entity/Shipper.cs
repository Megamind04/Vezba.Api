using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezba.Entity
{
    public class Shipper
    {
        public int ShipperID { get; set; }

        [Required(ErrorMessage = "Please insert a Shipper Name.")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        [DisplayName("Shipper Name")]
        public string ShipperName { get; set; }

        [Required(ErrorMessage = "Please insert a Phone.")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        public string Phone { get; set; }
    }
}
