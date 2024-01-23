using biZTrack.Models;

namespace biZTrack.Interfaces
{
    interface ILogin
    {
        Response LoginByPhone(string phoneNo);
    }
}
