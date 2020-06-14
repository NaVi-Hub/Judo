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
        public int Time { get; set; }
        public int id { get; set; }
        public MyButton(Activity c, int id) : base(c)
        {
            this.id = id;
            this.Time = 0;
        }
        public MyButton(Activity c) : base(c)
        {
            this.Time = 0;
        }
        public MyButton(Activity c, int id, int time) : base(c)
        {
            this.id = id;
            this.Time = time;
        }
    }
}