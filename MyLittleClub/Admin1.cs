namespace MyLittleClub
{
    public class Admin1 : Person
    {
        public int age { get; set; } // stam
        public string sport { get; set; }
        //to add later
        //public bitmap image { get; set; }
        public Admin1(int age, string sport, string name, string phoneNumber, string email) : base(name, phoneNumber, email)
        {
            this.age = age;
            this.sport = sport;
        }
        public Admin1() { }
    }
}