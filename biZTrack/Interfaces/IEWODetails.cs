using biZTrack.Models;

namespace biZTrack.Interfaces
{
    interface IEWODetails
    {
        Response RecieveEWODetails(EWODetails EWODetails);
        Response GetEWODetails(string EWONo);
        Response SendEWODetails(EWODetails EWODetails);
    }
}
