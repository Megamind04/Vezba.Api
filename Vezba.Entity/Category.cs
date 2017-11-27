using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezba.Entity
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please insert a CategoryName.")]
        [StringLength(30, ErrorMessage = "Max lengt 30.")]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please insert a Description.")]
        [StringLength(30, ErrorMessage = "Max lengt 150.")]
        public string Description { get; set; }
    }
}
