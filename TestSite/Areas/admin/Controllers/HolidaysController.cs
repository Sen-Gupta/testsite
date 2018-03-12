using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSite.ViewModels;

namespace TestSite.Areas.admin.Controllers
{
    public class HolidaysController : Controller
    {
        SqlConnection traincoreCon = new SqlConnection(ConfigurationManager.ConnectionStrings["traincore"].ConnectionString);
        // GET: Holidays
        public ActionResult Index()
        {          
            return View("~/Areas/admin/Views/Holidays/Index.cshtml");
        }
        [HttpGet]
        public ActionResult List()
        {
            List<HolidaysModel> hl = new List<HolidaysModel>();
            SqlCommand listHolidayList = new SqlCommand("Select * from Holidays", traincoreCon);
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                SqlDataReader rdr = listHolidayList.ExecuteReader();
                while (rdr.Read())
                {
                    HolidaysModel hm = new HolidaysModel();
                    hm.HolidayID = (int)rdr["HolidayID"];
                    hm.HolidayListID = (int)rdr["HolidayListID"];
                    hm.HolidayName = rdr["HolidayName"].ToString();
                    hm.HolidayDetails = rdr["HolidayDetails"].ToString();
                    hm.HolidayImageName = rdr["HolidayDetails"].ToString();
                    hm.StartDate = (DateTime)rdr["StartDate"];
                    hm.PricePerPerson = (decimal)rdr["PricePerPerson"];
                    if (rdr["HolidayImage"] != DBNull.Value)
                    {
                        hm.HolidayImage = (byte[])rdr["HolidayImage"];
                    }

                    hl.Add(hm);
                }
                ViewBag.Feedback = "Save Successfully";

            }
            catch (SqlException ex)
            {
                ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred";
            }
            catch (Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if (listHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }

            return View("~/Areas/admin/Views/Holidays/Index.cshtml", hl);
        }


        //Need Get to receive empty form 
        [HttpGet]
        public ActionResult Create(HolidayListsModel hlm)
        {
            List<HolidayListsModel> hl = new List<HolidayListsModel>();
            SqlCommand listHolidayList = new SqlCommand("Select HolidayListID,HolidayListName from HolidayLists", traincoreCon);
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                SqlDataReader rdr = listHolidayList.ExecuteReader();
                while (rdr.Read())
                {
                    HolidayListsModel hm = new HolidayListsModel();
                    hm.HolidayListID = (int)rdr["HolidayListID"];
                    hm.HolidayListName = rdr["HolidayListName"].ToString();                              
                    hl.Add(hm);
                }             
            }
            catch (SqlException ex)
            {
                ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred";
            }
            catch (Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if (listHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }
            return View("~/Areas/admin/Views/Holidays/Create.cshtml",hl);
        }
        [HttpPost]
        public ActionResult Create( string HolidayName, string HolidayDetails,DateTime? StartDate,decimal? PricePerPerson, HttpPostedFileBase HolidayImage)
        {
            //Read the FileStream Into ByteArray, equivalent to SQL Image Data Type
            byte[] buffer = new byte[HolidayImage.ContentLength];
            HolidayImage.InputStream.Read(buffer, 0, HolidayImage.ContentLength);

            int HolidayListID = int.Parse(Request.Form["holidaylist"].ToString());
            //Setup Connection

            SqlCommand sqlInsertHolidayList = new SqlCommand();
            sqlInsertHolidayList.CommandText = "Insert into Holidays (HolidayListID,HolidayName,HolidayDetails,HolidayImageName,HolidayImage,StartDate,PricePerPerson) VALUES(@HolidayListID,@HolidayName,@HolidayDetails,@HolidayImageName,@HolidayImage,@StartDate,@PricePerPerson)";
            sqlInsertHolidayList.Parameters.Add("@HolidayListID", SqlDbType.Int).Value = HolidayListID;
            sqlInsertHolidayList.Parameters.Add("@HolidayName", SqlDbType.NVarChar, 256).Value = HolidayName;
            sqlInsertHolidayList.Parameters.Add("@HolidayDetails", SqlDbType.NVarChar, 256).Value = HolidayDetails;
            sqlInsertHolidayList.Parameters.Add("@HolidayImageName", SqlDbType.NVarChar, 250).Value = HolidayImage.FileName;
            sqlInsertHolidayList.Parameters.Add("@HolidayImage", SqlDbType.Image).Value = buffer;
            sqlInsertHolidayList.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
            sqlInsertHolidayList.Parameters.Add("@PricePerPerson", SqlDbType.Money).Value = PricePerPerson;
            try
            {
                traincoreCon.Open();
                sqlInsertHolidayList.Connection = traincoreCon;
                sqlInsertHolidayList.ExecuteNonQuery();
                ViewBag.Feedback = "Save Successfully";

            }
            catch (SqlException ex)
            {
                ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred";
            }
            catch (Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if (sqlInsertHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }

            return View("~/Areas/admin/Views/Holidays/Create.cshtml");
        }
        [HttpGet]
        public ActionResult Edit(int HolidayID)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB

            SqlCommand listHolidayList = new SqlCommand("Select * from Holidays where HolidayID=@HolidayID", traincoreCon);
            listHolidayList.Parameters.AddWithValue("@HolidayID", HolidayID);
            HolidaysModel hm = new HolidaysModel();
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                SqlDataReader rdr = listHolidayList.ExecuteReader();
                while (rdr.Read())
                {

                    hm.HolidayID= (int)rdr["HolidayID"];
                    hm.HolidayName = rdr["HolidayName"].ToString();
                    hm.HolidayDetails = rdr["HolidayDetails"].ToString();
                    hm.HolidayImageName = rdr["HolidayImageName"].ToString();
                    if (rdr["HolidayImage"] != DBNull.Value)
                    {
                        hm.HolidayImage = (byte[])rdr["HolidayImage"];
                    }
                }

            }
            catch (SqlException ex)
            {
                ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred";
            }
            catch (Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if (listHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }
            return View("~/Areas/admin/Views/Holidays/Edit.cshtml", hm);
        }

        [HttpPost]
        public ActionResult Edit(HolidaysModel hlm)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB

            SqlCommand listHolidayList = new SqlCommand("UPDATE Holidays SET HolidayName=@HolidayName, StartDate=@StartDate, HolidayDetails=@HolidayDetails,PricePerPerson=@PricePerPerson where HolidayID=@HolidayID", traincoreCon);
            listHolidayList.Parameters.AddWithValue("@HolidayID", hlm.HolidayID);
            listHolidayList.Parameters.AddWithValue("@HolidayName", hlm.HolidayName);
            listHolidayList.Parameters.AddWithValue("@HolidayDetails", hlm.HolidayDetails);
            listHolidayList.Parameters.AddWithValue("@StartDate", hlm.StartDate);
            listHolidayList.Parameters.AddWithValue("@PricePerPerson", hlm.PricePerPerson);
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                int result = listHolidayList.ExecuteNonQuery();
                ViewBag.Feedback = "The record updated successfully";

            }
            catch (SqlException ex)
            {
                ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred";
            }
            catch (Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if (listHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }
            return View("~/Areas/admin/Views/Holidays/Edit.cshtml");
        }


        public ActionResult Delete(int HolidayID, bool? Confirm)
        {
            HolidaysModel hm = new HolidaysModel();
            if (Confirm == true)
            {
                SqlCommand cmd = new SqlCommand("Delete From Holidays where HolidayID=@HolidayID");
                cmd.Parameters.Add("@HolidayID", SqlDbType.Int).Value = HolidayID;
                cmd.Connection = traincoreCon;
                try
                {
                    traincoreCon.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.Feedback = $"Item with ID {HolidayID} deleted";
                }
                catch (SqlException ex)
                { ViewBag.Feedback = $"{ex.ErrorCode}, {ex.Message} occurred"; }
                catch (Exception ex)
                { ViewBag.Feedback = $"{ex.Message} occurred"; }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        traincoreCon.Close();

                    }
                }
            }
            else
            {

                hm.HolidayID = HolidayID;
            }
            return View("~/Areas/admin/Views/Holidays/Delete.cshtml", hm);
        }
    }
}