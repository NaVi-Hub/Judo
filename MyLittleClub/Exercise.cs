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
    public class Exercise
    {
        public string name { get; set; }
        public double duration { get; set; }
        public double order { get; set; }
        public string explenatiotn { get; set; }
        public Exercise (string name, double duration, double order, string explenation)
        {
            this.name = name;
            this.duration = duration;
            this.order = order;
            this.explenatiotn = explenation;
        }
        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=netframework-4.8  
    }
}