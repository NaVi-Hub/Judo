using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase;
using Firebase.Firestore;
using Java.Util;

using Android.Util;

namespace MyLittleClub
{
    static class MyStuff
    {
        public static Admin1 GetAdmin()
        {
            string email = ReverseEmail(sp.GetString("Email", null));
            string sport = sp.GetString("Sport", null);
            string name = sp.GetString("Name", null);
            string phoneNum = sp.GetString("PhoneNum", null);
            return new Admin1(sport, name, phoneNum, email);
        }
        public static string MakeDateString(int Year, int Month, int Day)
        {
            string txt;
            if (Month < 10 && Day < 10)
            {
                txt = string.Format("0{0}.0{1}.{2}", Day, Month, Year);
            }
            else if (Month < 10)
            {
                txt = string.Format("{0}.0{1}.{2}", Day, Month, Year);
            }
            else if (Day < 10)
            {
                txt = string.Format("0{0}.{1}.{2}", Day, Month, Year);
            }
            else
            {
                txt = string.Format("{0}.{1}.{2}", Day, Month, Year);
            }

            return txt;
        }
        public static bool isValidEmail(string email, Activity c)
        {
            if (Patterns.EmailAddress.Matcher(email).Matches())
            {
                return true;
            }
            else
            {
                Toasty.Config.Instance
                .TintIcon(true);
                Toasty.Error(c, "MailInvalid", 5, true).Show();
                return false;
            }
            //https://www.c-sharpcorner.com/article/how-to-validate-an-email-address-in-xamarin-android-app-using-visual-studio-2015/ @Delpin Susai Raj 
        }
        //Makes the Date string comfortable
        public static void showSoftKeyboard(Activity activity, View view)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Android.Content.Context.InputMethodService);
            view.RequestFocus();
            inputMethodManager.ShowSoftInput(view, 0);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);//personal line added
        }
        //Pops up soft keyboard
        public static ISharedPreferences sp { get; set; }
        public static FirebaseFirestore database { get; set; }
        public static string MakeEmail(string email)
        {
            try
            {
                return email.Replace("@", "]");
            }
            catch
            {
                return "Failed";
            }
        }
        public static string ReverseEmail(string email)
        {
            try
            {
                return email.Replace("]", "@");
            }
            catch
            {
                return "Failed";
            }
        }
        public static void DefineDatabase(FirebaseFirestore database)
        {
            MyStuff.database = database;
        }
        public static void DefineShared(ISharedPreferences sp)
        {
            MyStuff.sp = sp;
        }
        public static void PutToShared(Admin1 admin)
        {
            var editor = sp.Edit();
            editor.PutString("Name", admin.name);
            editor.PutString("Sport", admin.sport);
            editor.PutString("PhoneNum", admin.phoneNumber);
            editor.PutString("Email", MakeEmail(admin.email));
            editor.Commit();
        }
        public static void RemoveFromShared()
        {
            var editor = sp.Edit();
            editor.PutString("Name", default);
            editor.PutString("Sport", default);
            editor.PutString("PhoneNum", default);
            editor.PutString("Email", default);
            editor.Commit();
        }
        //Firebase defining
        public static bool IsDateLegit(DateTime date)
        {
            bool tr = false;
            DateTime EndDate = new DateTime();
            if (DateTime.Today.Month <= 8)
            {
                EndDate = new DateTime(DateTime.Today.Year, 9, 1);
            }
            else if(DateTime.Today.Month >= 9)
            {
                EndDate = new DateTime(DateTime.Today.Year + 1, 9, 1);
            }
            if (date.Year < DateTime.Today.Year)
            {
                tr = tr && false;
            }
            else if (date.Year > DateTime.Today.Year)
            {
                tr = tr && true;
            }
            else
            {
                if (date.Month > DateTime.Today.Month)
                {
                    tr = tr && true;
                }
                else if (date.Month > DateTime.Today.Month)
                {
                    tr = tr && true;
                }
                else
                {
                    if (date.Day >= DateTime.Today.Day)
                    {
                        tr = tr && true;
                    }
                    else if (date.Day > DateTime.Today.Day)
                    {
                        tr = tr && true;
                    }
                    else
                    {
                        tr = tr && false;
                    }
                }
            }
            if (date.Year < EndDate.Year)
            {
                tr = tr && false;
            }
            else if (date.Year > EndDate.Year)
            {
                tr = tr && true;
            }
            else
            {
                if (date.Month > EndDate.Month)
                {
                    tr = tr && true;
                }
                else if (date.Month > EndDate.Month)
                {
                    tr = tr && true;
                }
                else
                {
                    if (date.Day >= EndDate.Day)
                    {
                        tr = tr && true;
                    }
                    else if (date.Day > EndDate.Day)
                    {
                        tr = tr && true;
                    }
                    else
                    {
                        tr = tr && false;
                    }
                }
            }
            return tr;
        }
        //makes sure the date is in legit
    }
}