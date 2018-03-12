using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TestSite.Areas.admin.ViewModels;

namespace TestSite.Areas.admin.Controllers
{
    [RouteArea("admin")]
    public class HolidayListsController : Controller
    {
        SqlConnection traincoreCon = new SqlConnection(ConfigurationManager.ConnectionStrings["traincore"].ConnectionString);
        // GET: HolidayLists
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Areas/admin/Views/HolidayLists/Index.cshtml");
        }

        [HttpGet]
        public ActionResult List()
        {
            List<HolidayListsModel> hl = new List<HolidayListsModel>();
            SqlCommand listHolidayList = new SqlCommand("Select * from HolidayLists", traincoreCon);
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
                    hm.HolidayListDetails = rdr["HolidayListDetails"].ToString();
                    hm.HolidayListImageName = rdr["HolidayListImageName"].ToString();
                    if(rdr["HolidayListImage"] != DBNull.Value)
                    {
                        hm.HolidayListImage = (byte[])rdr["HolidayListImage"];
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
           
            return View("~/Areas/admin/Views/HolidayLists/Index.cshtml",hl);
        }


        //Need Get to receive empty form 
        [HttpGet]
        public ActionResult Create()
        {

            return View("~/Areas/admin/Views/HolidayLists/Create.cshtml");
        }
        [HttpPost]
        public ActionResult Create(string HolidayListName, string HolidayListDetails, HttpPostedFileBase HolidayListImage)
        {
            //Read the FileStream Into ByteArray, equivalent to SQL Image Data Type
            byte[] buffer = new byte[HolidayListImage.ContentLength];
            HolidayListImage.InputStream.Read(buffer, 0, HolidayListImage.ContentLength);


            //Setup Connection
            
            SqlCommand sqlInsertHolidayList = new SqlCommand();
            sqlInsertHolidayList.CommandText = "Insert into HolidayLists (HolidayListName,HolidayListDetails,HolidayListImageName,HolidayListImage) VALUES(@HolidayListName,@HolidayListDetails,@HolidayListImageName,@HolidayListImage)";
            sqlInsertHolidayList.Parameters.Add("@HolidayListName", SqlDbType.NVarChar, 256).Value = HolidayListName;
            sqlInsertHolidayList.Parameters.Add("@HolidayListDetails", SqlDbType.NVarChar, 1024).Value = HolidayListDetails;

            sqlInsertHolidayList.Parameters.Add("@HolidayListImageName",SqlDbType.NVarChar,256).Value = HolidayListImage.FileName;
            sqlInsertHolidayList.Parameters.Add("@HolidayListImage", SqlDbType.Image).Value = buffer;
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
            catch(Exception ex)
            {
                ViewBag.Feedback = $"{ex.Message} occurred";
            }
            finally
            {
                if(sqlInsertHolidayList.Connection.State == ConnectionState.Open)
                {
                    traincoreCon.Close();
                }
            }
          
            return View("~/Areas/admin/Views/HolidayLists/Create.cshtml");
        }
        [HttpGet]
        public ActionResult Edit(int HolidayListID)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB
         
            SqlCommand listHolidayList = new SqlCommand("Select * from HolidayLists where HolidayListID=@HolidayListID", traincoreCon);
            listHolidayList.Parameters.AddWithValue("@HolidayListID", HolidayListID);
            HolidayListsModel hm = new HolidayListsModel();
            try
            {
                traincoreCon.Open();
                listHolidayList.Connection = traincoreCon;
                SqlDataReader rdr = listHolidayList.ExecuteReader();
                while (rdr.Read())
                {
               
                    hm.HolidayListID = (int)rdr["HolidayListID"];
                    hm.HolidayListName = rdr["HolidayListName"].ToString();
                    hm.HolidayListDetails = rdr["HolidayListDetails"].ToString();
                    hm.HolidayListImageName = rdr["HolidayListImageName"].ToString();
                    if (rdr["HolidayListImage"] != DBNull.Value)
                    {
                        hm.HolidayListImage = (byte[])rdr["HolidayListImage"];
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
            return View("~/Areas/admin/Views/HolidayLists/Edit.cshtml", hm);
        }

        [HttpPost]
        public ActionResult Edit(HolidayListsModel hlm)
        {
            //Retrieve the Holiday, we are supposed to Edit and Send it on the same create Page
            //We will use SQL Reader to get the Holiday List from DB

            SqlCommand listHolidayList = new SqlCommand("UPDATE HolidayLists SET HolidayListName=@HolidayListName, HolidayListDetails=@HolidayListDetails where HolidayListID=@HolidayListID", traincoreCon);
            listHolidayList.Parameters.AddWithValue("@HolidayListID", hlm.HolidayListID);
            listHolidayList.Parameters.AddWithValue("@HolidayListName", hlm.HolidayListName);
            listHolidayList.Parameters.AddWithValue("@HolidayListDetails", hlm.HolidayListDetails);
       
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
            return View("~/Areas/admin/Views/HolidayLists/Edit.cshtml");
        }


        public ActionResult Delete(int HolidayListID ,bool? Confirm )
        {
            HolidayListsModel hm = new HolidayListsModel();
            if (Confirm==true)
            {
                SqlCommand cmd = new SqlCommand("Delete From HolidayLists where holidaylistid=@HolidayListID");
                cmd.Parameters.Add("@HolidayListID", SqlDbType.Int).Value = HolidayListID;
                cmd.Connection = traincoreCon;
                try
                {
                    traincoreCon.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.Feedback = $"Item with {HolidayListID} deleted";
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
               
                hm.HolidayListID = HolidayListID;
            }
            return View("~/Areas/admin/Views/HolidayLists/Delete.cshtml", hm);
        }

    }
}