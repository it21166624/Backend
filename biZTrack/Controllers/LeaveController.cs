using biZTrack.DataAccess;
using biZTrack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biZTrack.Controllers
{
    public class LeaveController : Controller
    {
        ILeave ILeave = null;
        [HttpGet]
        public ActionResult GetLeaveBalance(string serviceNo, string year)
        {
            ILeave = new DALeave();
            return Json(ILeave.GetLeaveBalance(serviceNo, year), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNotEnteredLeave(string serviceNo, string year)
        {
            ILeave = new DALeave();
            return Json(ILeave.GetNotEnteredLeave(serviceNo, year), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLeaveSummary(string serviceNo, string year)
        {
            ILeave = new DALeave();
            return Json(ILeave.GetLeaveSummary(serviceNo, year), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPunctuality(string serviceNo, string year)
        {
            ILeave = new DALeave();
            return Json(ILeave.GetPunctuality(serviceNo, year), JsonRequestBehavior.AllowGet);
        }
    }
}