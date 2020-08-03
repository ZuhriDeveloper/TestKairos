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

        public RentBook()
        {
            ID = 0;
            BookID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
        public int ID { get; set; }

        public int BookID { get; set; }
        public decimal PricePerDay { get; set; }
        public int RentLenght { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



    }
}
