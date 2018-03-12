using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Areas.admin.ViewModels
{
    public class HolidayListsModel
    {
        public int HolidayListID { get; set; }
        public string HolidayListName { get; set; }

        public string HolidayListDetails { get; set; }

        public string HolidayListImageName { get; set; }

        public byte[] HolidayListImage { get; set; }
    }
}