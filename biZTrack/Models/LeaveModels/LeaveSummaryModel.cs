using System;

namespace biZTrack.Models
{
    public class LeaveSummaryModel
    {
        public string leaveform_no { get; set; }
        public string service_no { get; set; }
        public string date { get; set; }
        public string day { get; set; }
        public string reason { get; set; }
        public string leave_type { get; set; }
        public string no_days { get; set; }
        public string approved_date { get; set; }
    }
}
