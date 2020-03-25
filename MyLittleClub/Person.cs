using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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