using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vezba.Entity
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please insert a LastName.")]
        [StringLength(50,ErrorMessage ="Max lengt 50.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please insert a FirstName.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Select a Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please insert a Photo.")]
        [StringLength(50, ErrorMessage = "Max lengt 50.")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Please insert a Photo.")]
        [StringLength(500, ErrorMessage = "Max lengt 500.")]
        public string Notes { get; set; }
    }
}
