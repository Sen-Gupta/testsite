using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Areas.admin.ViewModels
{
    public class HolidaysModel
    {
        public int HolidayID { get; set; }
        public int HolidayListID { get; set; }
        public string HolidayName { get; set; }

        public string HolidayDetails { get; set; }

        public string HolidayImageName { get; set; }

        public byte[] HolidayImage { get; set; }
        public DateTime StartDate { get; set; }

        public decimal PricePerPerson { get; set; }
    }
}