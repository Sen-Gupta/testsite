using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSite.Areas.admin.ViewModels;
using TestSite.ViewModels;

namespace TestSite.Areas.admin.Controllers
{
    public class BikesController : Controller
    {
        SqlConnection traincoreCon = new SqlConnection(ConfigurationManager.ConnectionStrings["traincore"].ConnectionString);
        // GET: Bikes
        public ActionResult Index()
        {
            return View("~/Areas/admin/Views/Bikes/Index.cshtml");
        }
        [HttpGet]
        public ActionResult List()
        {
            List<BikeModel> bl = new List<BikeModel>();
            SqlCommand Holidays = new SqlCommand("Select * from Bikes", traincoreCon);
            try
            {
                traincoreCon.Open();
                Holidays.Connection = traincoreCon;
                SqlDataReader rdr = Holidays.ExecuteReader();
                while (rdr.Read())
                {
                    BikeModel Bik = new BikeModel();
                    Bik.BikeID = (int)rdr["BikeID"];
                    Bik.BikeName = rdr["BikeName"].ToString();
                    Bik.BikeDetails = rdr["BikeDetails"].ToString();
                   

                    bl.Add(Bik);
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
                if (Holidays.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }

            return View("~/Areas/admin/Views/Bikes/Index.cshtml", bl);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<BikeModel> bik = new List<BikeModel>();
            SqlCommand Holidays = new SqlCommand("Select * from Holidays", traincoreCon);
            try
            {
                traincoreCon.Open();
                Holidays.Connection = traincoreCon;
                SqlDataReader rdr = Holidays.ExecuteReader();
                while (rdr.Read())
                {
                    BikeModel bm = new BikeModel();
                    bm.HolidayID = (int)rdr["HolidayID"];
                    bm.HolidayName = rdr["HolidayName"].ToString();

                    bik.Add(bm);
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
                if (Holidays.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }

            return View("~/Areas/admin/Views/Bikes/Create.cshtml", bik);
        }
        
        [HttpPost]
        public ActionResult Create( string HolidayName, string BikeName, string BikeDetails, HttpPostedFileBase BikeImage, string Type, string BikeSuitability)
        {
            int HolidayID = int.Parse(Request.Form["SltHoliday"]);
            //Read the FileStream Into ByteArray, equivalent to SQL Image Data Type
            byte[] buffer = new byte[BikeImage.ContentLength];
            BikeImage.InputStream.Read(buffer, 0, BikeImage.ContentLength);


            //Setup Connection

            SqlCommand sqlInsertBike = new SqlCommand();
            sqlInsertBike.CommandText = "Insert into Bikes (HolidayID,BikeName,BikeDetails,BikeImageName,BikeImage,Type,BikeSuitability) VALUES(@HolidayID,@BikeName,@BikeDetails,@BikeImageName,@BikeImage,@Type,@BikeSuitability)";
            sqlInsertBike.Parameters.Add("@HolidayID", SqlDbType.Int).Value = HolidayID;
            sqlInsertBike.Parameters.Add("@BikeName", SqlDbType.NVarChar).Value = BikeName;
            sqlInsertBike.Parameters.Add("@BikeDetails", SqlDbType.NVarChar).Value = BikeDetails;
            sqlInsertBike.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
            sqlInsertBike.Parameters.Add("@BikeSuitability", SqlDbType.NVarChar).Value = BikeSuitability;

            sqlInsertBike.Parameters.Add("@BikeImageName", SqlDbType.NVarChar).Value = BikeImage.FileName;
            sqlInsertBike.Parameters.Add("@BikeImage", SqlDbType.Image).Value = buffer;
            try
            {
                traincoreCon.Open();
                sqlInsertBike.Connection = traincoreCon;
                sqlInsertBike.ExecuteNonQuery();
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
                if (sqlInsertBike.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }

            return View("~/Areas/admin/Views/Bikes/Index.cshtml");
        }
        [HttpGet]
        public ActionResult Edit(int BikeID)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB

            SqlCommand listHolidayList = new SqlCommand("Select * from Bikes where BikeID=@BikeID", traincoreCon);
            listHolidayList.Parameters.AddWithValue("@BikeID", BikeID);
            BikeModel bm = new BikeModel();
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                SqlDataReader rdr = listHolidayList.ExecuteReader();
                while (rdr.Read())
                {

                    bm.BikeID = (int)rdr["BikeID"];
                    bm.BikeName = rdr["BikeName"].ToString();
                    bm.BikeDetails = rdr["BikeDetails"].ToString();
                    
                    
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
            return View("~/Areas/admin/Views/Bikes/Edit.cshtml", bm);
        }

        [HttpPost]
        public ActionResult Edit(BikeModel bkm)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB

            SqlCommand BikeList = new SqlCommand("UPDATE Bikes SET BikeName=@BikeName, BikeDetails=@BikeDetails where BikeID=@BikeID", traincoreCon);
            BikeList.Parameters.AddWithValue("@BikeID", bkm.BikeID);
            BikeList.Parameters.AddWithValue("@BikeName", bkm.BikeName);
            BikeList.Parameters.AddWithValue("@BikeDetails", bkm.BikeDetails);

            try
            {
                traincoreCon.Open();
                BikeList.Connection = traincoreCon;
                int result = BikeList.ExecuteNonQuery();
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
                if (BikeList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }
            return View("~/Areas/admin/Views/Bikes/Edit.cshtml");
        }
        public ActionResult Delete(int BikeID, bool? Confirm)
        {
            BikeModel bm = new BikeModel();
            if (Confirm == true)
            {
                SqlCommand cmd = new SqlCommand("Delete From Bikes where Bikeid=@BikeID");
                cmd.Parameters.Add("@BikeID", SqlDbType.Int).Value = BikeID;
                cmd.Connection = traincoreCon;
                try
                {
                    traincoreCon.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.Feedback = $"Item with {BikeID} deleted";
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

                bm.BikeID = BikeID;
            }
            return View("~/Areas/admin/Views/Bikes/Delete.cshtml", bm);
        }
    }
}