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
    class MyButton : Button
    {
        public int id { get; set; }
        public MyButton(Activity c, int id) : base(c)
        {
            this.id = id;
        }
    }
}