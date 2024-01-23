using biZTrack.DataAccess;
using biZTrack.Interfaces;
using biZTrack.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace AttendMeBackEnd.Controllers
{
    public class AttendanceController : Controller
    {
        IAttendance IAttendance = null;

        [HttpGet]
        public ActionResult GetSupervisorList(string serviceNo)
        {
            IAttendance = new DAAttendance();
            return Json(IAttendance.GetSupervisorList(serviceNo), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Login()
        {
            string PhoneNo = Request.Headers["PhoneNo"].Replace("+940", "+94");
            IAttendance = new DAAttendance();
            return Json(IAttendance.getEmpByPhone(PhoneNo), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PostAttendance(AttendanceModel attendance)
        {
            IAttendance = new DAAttendance();
            return Json(IAttendance.PostAttendance(attendance), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PostArray(AttendanceModel attendance)
        {

            IAttendance = new DAAttendance();
            AttendanceModel[] Attendance = JsonConvert.DeserializeObject<AttendanceModel[]>(attendance.attendanceList);
            foreach (AttendanceModel item in Attendance)
            {
                IAttendance.PostAttendance(item);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAttendanceCard(string serviceNo, string month)
        {
            IAttendance = new DAAttendance();
            return Json(IAttendance.GetAttendanceCard(serviceNo, month), JsonRequestBehavior.AllowGet);
        }
    }
}