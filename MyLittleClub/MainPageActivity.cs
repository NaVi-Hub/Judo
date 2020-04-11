using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static Android.Widget.CalendarView;

namespace MyLittleClub
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainPageActivity : AppCompatActivity, IOnDateChangeListener
    {
        ISharedPreferences sp;
        LinearLayout MainPageOverallLayout, MainPageTitleLayout;
        TextView MainPageTitleTV, MainPageTitleTV2;
        LinearLayout.LayoutParams CalendarParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 1000);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        public static Admin1 admin1;
        FirebaseFirestore database;
        int abc = 0;
        List<String> dates;
        List<Group> groups;
        Button MainPageShowGroupsbtn;
        Dialog d;
        CalendarView calendar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageLayout);
            admin1 = MyStuff.GetAdmin();
            database = Context.database;
            GetDates();
        }
        public void BuildMainPage()
        {
            //Main Page Overall Layout defining
            MainPageOverallLayout = FindViewById<LinearLayout>(Resource.Id.MainPageLayout1);
            MainPageOverallLayout.Orientation = Orientation.Vertical;
            MainPageOverallLayout.SetGravity(Android.Views.GravityFlags.CenterHorizontal);
            BuildCalendar();

            //Tile layout
            MainPageTitleLayout = new LinearLayout(this);
            MainPageTitleLayout.LayoutParameters = WrapContParams;
            MainPageTitleLayout.Orientation = Orientation.Vertical;
            MainPageTitleLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Title TV
            MainPageTitleTV = new TextView(this);
            MainPageTitleTV.LayoutParameters = WrapContParams;
            MainPageTitleTV.Text = $"Welcome, {admin1.name}";
            MainPageTitleTV.TextSize = 55;
            MainPageTitleTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            MainPageTitleTV.SetTextColor(Android.Graphics.Color.DarkRed);
            //Title TV 2
            MainPageTitleTV2 = new TextView(this);
            MainPageTitleTV2.LayoutParameters = WrapContParams;
            int year = int.Parse(DateTime.Today.Year.ToString());
            int month = int.Parse(DateTime.Today.Month.ToString()) + 1;
            int day = int.Parse(DateTime.Today.Day.ToString());
            MainPageTitleTV2.Text = $"You have {abc} trainings on the {MyStuff.MakeDateString(year, month, day)}";
            MainPageTitleTV2.TextSize = 25;
            MainPageTitleTV2.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            MainPageTitleTV2.SetTextColor(Android.Graphics.Color.SaddleBrown);
            //adding to layouts
            MainPageTitleLayout.AddView(MainPageTitleTV);
            MainPageTitleLayout.AddView(MainPageTitleTV2);
            MainPageOverallLayout.AddView(MainPageTitleLayout);
            //Calendar
            MainPageOverallLayout.AddView(calendar);
            OnSelectedDayChange(calendar, int.Parse(DateTime.Today.Year.ToString()), int.Parse(DateTime.Today.Month.ToString()) - 1, int.Parse(DateTime.Today.Day.ToString()));
            //Button
            MainPageShowGroupsbtn = new Button(this);
            MainPageShowGroupsbtn.LayoutParameters = new LinearLayout.LayoutParams(1100, 600);
            MainPageShowGroupsbtn.Text = "Show Groups";
            MainPageOverallLayout.AddView(MainPageShowGroupsbtn);
            MainPageShowGroupsbtn.Click += this.MainPageShowGroupsbtn_Click;
        }
        //Build Main Page's Views
        private void MainPageShowGroupsbtn_Click(object sender, EventArgs e)
        {
            GetGroups();
        }
        //Defining and adding views to layout
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MyLittleMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        //Menu inflator
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuItem1:
                    {
                        Intent tryIntent = new Intent(this, typeof(AddGroupActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem2:
                    {
                        Intent tryIntent = new Intent(this, typeof(AddTrainingActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem3:
                    {
                        Intent tryIntent = new Intent(this, typeof(AddStudentActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem4:
                    {
                        Intent tryIntent = new Intent(this, typeof(BuildTrainingActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem5:
                    {
                        MyStuff.CancelLoginAbilityOnAllUsers();
                        Intent intent3 = new Intent(this, typeof(RegisterActivity));
                        StartActivity(intent3);
                        return true;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }
        //Menu selected
        public void BuildCalendar()
        {
            calendar = new CalendarView(this);
            calendar.LayoutParameters = CalendarParams;
            calendar.SetOnDateChangeListener(this);
        }
        //builds calendarview
        public void OnSelectedDayChange(CalendarView view, int year, int month, int dayOfMonth)
        {
            abc = 0;
            string txt = MyStuff.MakeDateString(year, month + 1, dayOfMonth);
            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i] == txt)
                {
                    abc++;
                }
            }
            int year1 = int.Parse(DateTime.Today.Year.ToString());
            int month1 = int.Parse(DateTime.Today.Month.ToString());
            int day1 = int.Parse(DateTime.Today.Day.ToString());
            if (txt == MyStuff.MakeDateString(year1, month1, day1))
            {
                MainPageTitleTV2.Text = $"You have {abc} trainings Today";
            }
            else
            {
                MainPageTitleTV2.Text = $"You have {abc} trainings on the {MyStuff.MakeDateString(year, month + 1, dayOfMonth)}";
            }
        }
        //changes the title TextView to show how many trainings are in the inputted date
        public void GetGroups()
        {
            groups = new List<Group>();
            Query query = database.Collection("Users").Document(admin1.email).Collection("Groups");
            query.Get().AddOnCompleteListener(new QueryListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = (QuerySnapshot)task.Result;
                    if (!snapshot.IsEmpty)
                    {
                        var document = snapshot.Documents;
                        foreach (DocumentSnapshot item in document)
                        {
                            string loc = (item.GetString("Location")).ToString();
                            string Age = (item.GetString("Age")).ToString();
                            string date = (item.GetString("Date")).ToString();
                            bool comp = item.GetBoolean("Comp").BooleanValue();
                            string level = (item.GetString("Level")).ToString();
                            string time = (item.GetString("Time")).ToString();
                            Group group1 = new Group(Age, level, comp, loc, date, time);
                            groups.Add(group1);
                        }
                    }
                }
            }
            ));
        }
        //Retrives the Groups from database
        public void GetDates()
        {
            dates = new List<string>();
            Query query = database.Collection("Users").Document(admin1.email).Collection("Groups");
            query.Get().AddOnCompleteListener(new QueryListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = (QuerySnapshot)task.Result;
                    if (!snapshot.IsEmpty)
                    {
                        var document = snapshot.Documents;
                        foreach (DocumentSnapshot item in document)
                        {
                            string day = (item.GetString("Date").ToString())[0] + "" + (item.GetString("Date").ToString())[1];
                            string month = (item.GetString("Date").ToString())[3] + "" + (item.GetString("Date").ToString())[4];
                            string year = (item.GetString("Date").ToString())[6] + "" + (item.GetString("Date").ToString())[7] + (item.GetString("Date").ToString())[8] + "" + (item.GetString("Date").ToString())[9];
                            int inday = int.Parse(day);
                            int inmonth = int.Parse(month);
                            int inyear = int.Parse(year);
                            dates.Add(MyStuff.MakeDateString(inyear, inmonth, inday));
                        }
                    }
                }
                BuildMainPage();
            }
            ));
        }
        //Retrives the Dates on which there are Groups from database
    }
}