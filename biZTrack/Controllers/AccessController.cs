using biZTrack.DataAccess;
using biZTrack.Interfaces;
using biZTrack.Middleware;
using System.Web.Mvc;

namespace AttendMeBackEnd.Controllers
{
    public class AccessController : Controller
    {
        private IAccess IAccess = null;

        [HttpGet]
        [Authentication]
        public ActionResult GetAccessHeadComponent()
        {
            IAccess = new DAAccess();
            string authKey = Request.Headers["MySecretToken"];
            return Json(IAccess.GetAccessHeadComponent(authKey), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authentication]
        public ActionResult GetAccessSubComponent(string headCompId)
        {
            IAccess = new DAAccess();
            string authKey = Request.Headers["MySecretToken"];
            return Json(IAccess.GetAccessSubComponent(authKey, headCompId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authentication]
        public ActionResult GetAccessComponentList()
        {
            IAccess = new DAAccess();
            return Json(IAccess.GetAccessComponentList(), JsonRequestBehavior.AllowGet);
        }
    }
}