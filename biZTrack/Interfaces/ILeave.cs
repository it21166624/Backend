using biZTrack.Models;

namespace biZTrack.Interfaces
{
    interface ILeave
    {
        Response GetLeaveBalance(string serviceNo, string year);
        Response GetNotEnteredLeave(string serviceNo, string year);
        Response GetLeaveSummary(string serviceNo, string year);
        Response GetPunctuality(string serviceNo, string year);
    }
}
