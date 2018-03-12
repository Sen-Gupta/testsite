using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Areas.admin.ViewModels
{
    public class BikeModel
    {
       public int BikeID { get; set; }
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public string BikeName { get; set; }
        public string BikeDetails { get; set; }
        public string BikeImageName { get; set; }
        public Byte[] BikeImage { get; set; }
        public string Type { get; set; }
        public string BikeSuitability { get; set; }
    }
}