namespace MyLittleClub
{
    public class Admin1 : Person
    {
        public string sport { get; set; }
        //to add later
        //public bitmap image { get; set; }
        public Admin1( string sport, string name, string phoneNumber, string email) : base(name, phoneNumber, email)
        {
            this.sport = sport;
        }
        public Admin1() { }
    }
}