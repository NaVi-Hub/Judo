using Android.Gms.Tasks;
using System;
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

        public void OnComplete(Task task)
        {
            OnQueryComplete(task);
        }
    }
}