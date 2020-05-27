using Android.Net.Wifi.Aware;

namespace MyLittleClub
{
    public class Admin1 : Person
    {
        public string sport { get; set; }

        public string ProfilePic { get; set; }
        //to add later
        //public bitmap image { get; set; }
        public Admin1( string sport, string name, string phoneNumber, string email, string Profile) : base(name, phoneNumber, email)
        {
            this.sport = sport;
            this.ProfilePic = Profile;
        }
        public Admin1() { }
    }
}