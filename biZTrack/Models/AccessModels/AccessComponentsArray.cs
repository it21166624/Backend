using System.Collections.Generic;

namespace biZTrack.Models
{
    public class AccessComponentsArray
    {
        public string code { get; set; }
        public string description { get; set; }
        public List<AccessHeadComponents> subComponents { get; set; }
    }
}
