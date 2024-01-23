using biZTrack.Models;

namespace biZTrack.Interfaces
{
    interface IAccess
    {
        Response GetAccessHeadComponent(string user_id);
        Response GetAccessSubComponent(string encryptKey, string head_comp_id);
        Response GetAccessComponentList();
    }
}
