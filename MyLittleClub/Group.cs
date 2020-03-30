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
using Java.Sql;

namespace MyLittleClub
{
    public class Group
    {

        public string age { get; set; }
        public string geoupLevel { get; set; }
        public bool competetive { get; set; }
        public string Location { get; set; }
        private bool GroupFull;
        //Location loc;
        public string date { get; set; }
        public string time { get; set; }


        private int numOfstudents = 0;
        public Training CurrentTraining { get; set; }
        public List<Training> Trainings { get; set; }

        //Constructor if all the parameters are given
        public Group(string age, string level, bool competetive, string loc, Student[] students, string date, string time /*,Location loc, DateTime date*/)
        {
            this.age = age;
            this.geoupLevel = level;
            this.competetive = competetive;
            this.Location = loc;
            this.date = date;
            this.time = time;
            /*this.loc = loc;*/
        }

        //Contsrtuctor for an emty group whlie only the age and is given
        public Group(string age, string lvl, bool comp, string location)
        {
            this.age = age;
            this.geoupLevel = lvl;
            this.competetive = comp;
            this.Location = location;
        }
        public Group(string age, string lvl, bool comp, string location, string date, string time)
        {
            this.age = age;
            this.geoupLevel = lvl;
            this.competetive = comp;
            this.Location = location;
            this.date = date;
            this.time = time;
        }

    }
    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.calendar?view=netframework-4.8 calendar
}