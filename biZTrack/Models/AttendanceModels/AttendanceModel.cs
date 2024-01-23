namespace biZTrack.Models
{
    public class AttendanceModel
    {
        public string serviceNo { get; set; }
        public int InOut { get; set; }
        public string dateTime { get; set; }
        public string attendanceList { get; set; }
        public int IwNo { get; set; }
        public string location { get; set; }
        public string geoLocation { get; set; }
    }
}
