using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Task = Android.Gms.Tasks.Task;

namespace MyLittleClub
{
    class QueryListener : Java.Lang.Object, IOnCompleteListener
    {
        Action<Task> OnQueryComplete;
        public QueryListener(Action<Task> action)
        {
            OnQueryComplete = action;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            OnQueryComplete(task);
        }
    }
}