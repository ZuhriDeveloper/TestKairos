using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosTest.Helpers;

namespace KairosTest.Models
{
    public class RentBook
    {

        public int ID { get; set; }

        [RequiredGreaterThanZero(ErrorMessage ="Please choose book")]
        [Display(Name = "Book")]
        public int BookID { get; set; }

        public virtual Book Book { get; set; }


        [RequiredGreaterThanZero(ErrorMessage = "Please choose book")]
        [Display(Name = "Price Per Day")]
        public decimal PricePerDay { get; set; }

        public string PricePerDayDisplay { get; set; }
        public int RentLenght { get; set; }

        [Display(Name = "Date Start")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Date End")]
        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string Total { get; set; }



    }
}
