namespace biZTrack.Models
{
    public class OTPResponse
    {
        public int statusCode { get; set; }
        public object userDetails { get; set; }
        public string OTP { get; set; }
    }
}
