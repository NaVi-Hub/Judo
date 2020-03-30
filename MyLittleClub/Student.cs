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
    public class Student : Person
    {
        public string parentName1 { get; set; }
        public string parentName2 { get; set; }
        public string notes { get; set; }
        public string group { get; set; }
        public Student(string name, string phoneNumber, string email, string parentName1, string parentName2, string notees ,string group) : base(name, phoneNumber, email)
        {
            this.parentName1 = parentName1;
            this.parentName2 = parentName2;
            this.notes = notees;
            this.group = group;
        }
        public Student(string name, string phoneNumber, string email, string parentName1, string notees, string group) : base(name, phoneNumber, email)
        {
            this.parentName1 = parentName1;
            this.parentName2 = "";
            this.notes = notees;
            this.group = group;
        }
        public Student(string name, string phoneNumber, string email, string notees, string group) : base(name, phoneNumber, email)
        {
            this.parentName1 = "";
            this.parentName2 = "";
            this.notes = notees;
            this.group = group;
        }
        public Student() { }
    }
}