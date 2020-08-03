using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosTest.Helpers;

namespace KairosTest.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Display(Name = "Book Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book title must be filled")]
        public string BookTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Author title must be filled")]
        public string Author { get; set; }

        [Display(Name = "Book Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book type must be filled")]
        public string BookType { get; set; }

        [Display(Name = "Rent Price per Day")]
        [RequiredPositiveValue(ErrorMessage = "Value must be greater than zero")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal RentPrice { get; set; }
    }
}
