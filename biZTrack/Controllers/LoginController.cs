using biZTrack.DataAccess;
using biZTrack.Interfaces;
using biZTrack.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttendMeBackEnd.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private ILogin ILogin = null;

        [HttpGet]
        public async Task<ActionResult> LoginByPhone(string phoneNo)
        {
            OTPResponse Oresponse = new OTPResponse();
            ILogin = new DALogin();
            Response res = ILogin.LoginByPhone(phoneNo);
            if (res.statusCode == 200)
            {
                Oresponse.userDetails = res.resultSet;
                using (var client = new HttpClient())
                {

                    try
                    {
                        client.BaseAddress = null;
                        client.DefaultRequestHeaders.Accept.Clear();
                        StringContent httpContent = null;
                        HttpResponseMessage response = await client.PostAsync(client.BaseAddress, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            string kk = await response.Content.ReadAsStringAsync();
                            kk = kk.Replace("\"", "");

                            Oresponse.statusCode = 200;
                            Oresponse.OTP = kk;

                        }

                        else
                        {
                            Oresponse.statusCode = 401;
                        }
                    }
                    catch (Exception e)
                    {
                        Oresponse.statusCode = 500;
                    }

                }
            }
            else 
                Oresponse.statusCode = 404;
            
            return Json(Oresponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserImg(string serviceNo) {
            try
            {
                var image = System.IO.File.OpenRead(@"\\cdplc-apps\CDPLC_PROD\PHOTOS\dtsemp_photo\" + serviceNo);
                return File(image, "image/jpeg");
            }
            catch(Exception ex) {
                return Json(ex.Message);
            }
        }
    }
}