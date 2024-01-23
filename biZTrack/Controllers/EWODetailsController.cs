using biZTrack.DataAccess;
using biZTrack.Interfaces;
using biZTrack.Models;
using System.Web.Mvc;

namespace AttendMeBackEnd.Controllers
{
    public class EWODetailsController : Controller
    {
        IEWODetails IEWODetails = null;

        //[Authentication]
        public ActionResult Index()
        {
            return Json("!!BiZTrack!!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecieveEWODetails(EWODetails EWODetails)
        {
            IEWODetails = new DAEWODetails();
            return Json(IEWODetails.RecieveEWODetails(EWODetails), JsonRequestBehavior.AllowGet);
        }

        //[Authentication]
        public ActionResult GetEWODetails(string EWONo)
        {
            IEWODetails = new DAEWODetails();
            return Json(IEWODetails.GetEWODetails(EWONo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendEWODetails(EWODetails EWODetails)
        {
            IEWODetails = new DAEWODetails();
            return Json(IEWODetails.SendEWODetails(EWODetails), JsonRequestBehavior.AllowGet);
        }
        
    }
}