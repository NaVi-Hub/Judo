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
    public class Admin1 : Person
    {
        public int age { get; set; } // stam
        public string sport { get; set; }
        public bool LogIn { get; set; }
        //to add later
        //public bitmap image { get; set; }
        public Admin1(int age, string sport, string name, string phoneNumber, string email, bool l) : base(name, phoneNumber, email)
        {
            this.age = age;
            this.sport = sport;
            this.LogIn = l;
        }
        public Admin1() { }
    }
}