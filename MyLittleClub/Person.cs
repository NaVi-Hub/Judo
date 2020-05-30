namespace MyLittleClub
{
    public class Person
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }

        public Person(string name, string phoneNumber, string email)
        {
            this.name = name;
            this.phoneNumber = phoneNumber;
            this.email = email;
        }
        public Person()
        {
        }

    }

}