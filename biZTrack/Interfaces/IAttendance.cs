using biZTrack.Models;

namespace biZTrack.Interfaces
{
    interface IAttendance
    {
        Response GetSupervisorList(string location);
        Response getEmpByPhone(string phoneNo);
        Response PostAttendance(AttendanceModel attendance);
        Response GetAttendanceCard(string serviceNo, string month);
    }
}
