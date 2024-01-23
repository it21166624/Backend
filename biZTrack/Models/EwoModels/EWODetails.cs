using System;

namespace biZTrack.Models
{
    public class EWODetails
    {
        public string ewo_no { get; set; }
        public string authorize_person { get; set; }
        public string authorize_by { get; set; }
        public string ewo_status { get; set; }
        public string WD_status { get; set; }
        public string status_bckcolor { get; set; }
        public string status_txtcolor { get; set; }
        public double estimated_amount { get; set; }
        public double billed_amount { get; set; }
        public string evaluation_by { get; set; }
        public string recieved_by { get; set; }
        public string issued_by { get; set; }
        public string doc_type { get; set; }
        public string remarks { get; set; }
        public string serial_no { get; set; }
        public string recieved_date { get; set; }
        public string issued_date { get; set; }
    }

    public class EWOTrackingDetails
    {
        public string issued_by { get; set; }
        public string recieved_by { get; set; }
        public string recieved_by_name { get; set; }
        public string serial_no { get; set; }
        public DateTime recieved_date { get; set; }
        public DateTime issued_date { get; set; }
    }
}
