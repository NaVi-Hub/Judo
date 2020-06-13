using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace MyLittleClub
{
    class MyTimer : AsyncTask<Int32, Int32, Int32>
    {
        TextInputEditText Time;
        int counter;
        public MyTimer(TextInputEditText time)
        {
            this.Time = time;
            this.counter = 0;
        }
        protected override int RunInBackground(params int[] @params)
        {
            while (int.Parse(this.Time.Text) > 0)
            {
                Thread.Sleep(1000);
                counter--;
                PublishProgress(counter);
            }
            return counter;
        }
        protected override void OnProgressUpdate(params int[] values)
        {
            base.OnProgressUpdate(values);
            this.Time.Text = counter.ToString();
        }
        protected override void OnPostExecute(int result)
        {
            base.OnPostExecute(result);
        }
    }
}