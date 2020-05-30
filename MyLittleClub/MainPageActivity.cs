using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
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
        LinearLayout MainPageOverallLayout, MainPageTitleLayout, MainPageProfilePictureLayout;
        TextView MainPageTitleTV, MainPageTitleTV2;
        LinearLayout.LayoutParams CalendarParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 1000);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        public static Admin1 admin1;
        FirebaseFirestore database;
        ImageView Profile;
        int abc = 0;
        List<String> dates;
        List<Group> groups;
        Button MainPageShowGroupsbtn;
        Dialog d;
        ViewGroup.LayoutParams VLP = new ViewGroup.LayoutParams(600, 800);
        CalendarView calendar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageLayout);
            admin1 = MyStuff.GetAdmin();
            database = MyStuff.database;
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
            //Profile Picture Layout
            MainPageProfilePictureLayout = new LinearLayout(this);
            MainPageProfilePictureLayout.LayoutParameters = WrapContParams;
            MainPageProfilePictureLayout.Orientation = Orientation.Vertical;
            MainPageProfilePictureLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Profile Pic
            Profile = new ImageView(this);
            Profile.SetImageBitmap(MyStuff.ConvertStringToBitMap(admin1.ProfilePic));
            Profile.SetMaxWidth(250);
            Profile.SetMinimumHeight(400);
            MainPageProfilePictureLayout.AddView(Profile);
            //Title TV 2
            MainPageTitleTV2 = new TextView(this);
            MainPageTitleTV2.LayoutParameters = WrapContParams;
            int year = int.Parse(DateTime.Today.Year.ToString());
            int month = int.Parse(DateTime.Today.Month.ToString()) + 1;
            int day = int.Parse(DateTime.Today.Day.ToString());
            MainPageTitleTV2.Text = $"You have {abc} trainings on the {MyStuff.MakeDateString(year, month, day)}";
            MainPageTitleTV2.TextSize = 25;
            MainPageTitleTV2.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            MainPageTitleTV2.SetTextColor(Color.SaddleBrown);
            //adding to layouts
            MainPageTitleLayout.AddView(MainPageTitleTV);
            MainPageOverallLayout.AddView(MainPageProfilePictureLayout);
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
        //Calls GetGroups()
        private void BuildMainPageShowGroupsDialogRecycler()
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.MyDialog);
            d.SetCancelable(true);
            LinearLayout DialogLayout = d.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            LinearLayout[] LinearList = new LinearLayout[groups.Count];
            LinearLayout[] BLinearList = new LinearLayout[groups.Count];
            TextView[] TextViewList = new TextView[groups.Count];
            TextView[] TextViewList2 = new TextView[groups.Count];
            MyButton[] ButtonList = new MyButton[groups.Count];
            MyButton[] ButtonList2 = new MyButton[groups.Count];
            ScrollView SV = new ScrollView(this);
            SV.LayoutParameters = new ViewGroup.LayoutParams(950, 1500);
            DialogLayout.AddView(SV);
            LinearLayout ll = new LinearLayout(this);
            ll.LayoutParameters = MatchParentParams;
            ll.Orientation = Orientation.Vertical;
            ll.SetGravity(GravityFlags.CenterHorizontal);
            for (int i = 0; i < groups.Count; i++)
            {
                LinearList[i] = new LinearLayout(this);
                LinearList[i].LayoutParameters = new LinearLayout.LayoutParams(950, 450);
                LinearList[i].Orientation = Orientation.Horizontal;
                LinearList[i].SetBackgroundResource(Resource.Drawable.BlackOutLine);
                //
                TextViewList[i] = new TextView(this);
                TextViewList[i].LayoutParameters = WrapContParams;
                TextViewList[i].Text = "Location: " + "\nTime: " + "\nAge: ";
                TextViewList[i].TextSize = 35;
                TextViewList[i].Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
                TextViewList[i].SetTextColor(Color.Black);
                //
                TextViewList2[i] = new TextView(this);
                TextViewList2[i].LayoutParameters = WrapContParams;
                TextViewList2[i].Text = groups[i].Location + "\n" + groups[i].time + "\n" + groups[i].age;
                TextViewList2[i].TextSize = 30;
                TextViewList2[i].Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
                TextViewList2[i].SetTextColor(Color.Red);
                //
                BLinearList[i] = new LinearLayout(this);
                BLinearList[i].LayoutParameters = new LinearLayout.LayoutParams(950, LinearLayout.LayoutParams.WrapContent);
                BLinearList[i].Orientation = Orientation.Horizontal;
                BLinearList[i].SetBackgroundResource(Resource.Drawable.BlackOutLine);
                //
                ButtonList[i] = new MyButton(this, i)
                {
                    Text = "Students",
                    TextSize = 30,
                    Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                };
                ButtonList[i].SetTextColor(Color.DarkRed);
                ButtonList[i].Click += this.ButtonList_Click;
                //
                ButtonList2[i] = new MyButton(this, i)
                {
                    Text = "Edit",
                    TextSize = 30,
                    Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                };
                ButtonList2[i].SetTextColor(Color.DarkRed);
                ButtonList2[i].Click += this.ButtonList2_Click;
                //
                LinearList[i].AddView(TextViewList[i]);
                LinearList[i].AddView(TextViewList2[i]);
                BLinearList[i].AddView(ButtonList[i]);
                BLinearList[i].AddView(ButtonList2[i]);
                ll.AddView(LinearList[i]);
                ll.AddView(BLinearList[i]);
            }
            SV.AddView(ll);
            d.Show();
        }
        //Builds Groups Dialog
        private void ButtonList_Click(object sender, EventArgs e)
        {
            MyButton b = (MyButton)sender;
            GetStudents(groups[b.id]);
        }
        //Opens Students in this group dialog
        private void ButtonList2_Click(object sender, EventArgs e)
        {
            MyButton b = (MyButton)sender;
            BuildEditGroupDialog(groups[b.id].Location + " " + groups[b.id].time + " " + groups[b.id].age);
        }
        //Edit Group
        Dialog StuD, GrouD;
        public void BuildStudentsDialog()
        {
            StuD = new Dialog(this);
            StuD.SetContentView(Resource.Layout.MyDialog);
            StuD.SetCancelable(true);
            //
            LinearLayout OAStusLayout = StuD.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            //
            LinearLayout[] LinearList = new LinearLayout[Students.Count];
            TextView[] TextViewList = new TextView[Students.Count];
            TextView[] TextViewList2 = new TextView[Students.Count];
            ScrollView SV = new ScrollView(this);
            SV.LayoutParameters = new ViewGroup.LayoutParams(950, 1500);
            OAStusLayout.AddView(SV);
            LinearLayout ll = new LinearLayout(this);
            ll.LayoutParameters = MatchParentParams;
            ll.Orientation = Orientation.Vertical;
            ll.SetGravity(GravityFlags.CenterHorizontal);
            if (Students.Count > 0)
            {
                for (int i = 0; i < Students.Count; i++)
                {
                    LinearList[i] = new LinearLayout(this);
                    LinearList[i].LayoutParameters = new LinearLayout.LayoutParams(950, 600);
                    LinearList[i].Orientation = Orientation.Horizontal;
                    LinearList[i].SetBackgroundResource(Resource.Drawable.BlackOutLine);
                    //
                    TextViewList[i] = new TextView(this);
                    TextViewList[i].Text = "Name: " + "\nPhoneNum: " + "\nEmail: " + "\nParent1: " + "\nParent2: " + "\nNotes";
                    TextViewList[i].LayoutParameters = WrapContParams;
                    TextViewList[i].TextSize = 30;
                    TextViewList[i].Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
                    TextViewList[i].SetTextColor(Color.Black);
                    //
                    TextViewList2[i] = new TextView(this);
                    TextViewList2[i].Text = Students[i].name + "\n" + Students[i].phoneNumber + "\n" + Students[i].email + "\n" + Students[i].parentName1 + "\n" + Students[i].parentName2 + "\n" + Students[i].notes;
                    TextViewList2[i].LayoutParameters = WrapContParams;
                    TextViewList2[i].TextSize = 25;
                    TextViewList2[i].Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
                    TextViewList2[i].SetTextColor(Color.Red);
                    //
                    LinearList[i].AddView(TextViewList[i]);
                    LinearList[i].AddView(TextViewList2[i]);
                    ll.AddView(LinearList[i]);
                }
                SV.AddView(ll);
                StuD.Show();
            }
            else
                Toasty.Error(this, "Group Empty", 5, true).Show();
        }
        //Build Dialog Students in group
        public void BuildEditGroupDialog(string groupName)
        {

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
                        Intent tryIntent = new Intent(this, typeof(AddExerciseActivity));
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
                        Intent tryIntent = new Intent(this, typeof(EditAdminActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem6:
                    {
                        Intent tryIntent = new Intent(this, typeof(TimerActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem7:
                    {
                        MyStuff.RemoveFromShared();
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
                BuildMainPageShowGroupsDialogRecycler();
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
        List<Student> Students;
        public void GetStudents(Group group)
        {
            Students = new List<Student>();
            Query query = database.Collection("Users").Document(admin1.email).Collection("Students");
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
                            string group1 = item.GetString("Group");
                            if (group1 == group.Location + " " + group.time + " " + group.age)
                            {
                                string name = item.GetString("Name");
                                string Email = item.GetString("Email");
                                string notes = item.GetString("Notes");
                                string p1 = item.GetString("Parent1");
                                string p2 = item.GetString("Parent2");
                                string PN = item.GetString("PhoneNum");
                                Students.Add(new Student(name, PN, Email, p1, p2, notes, group1));
                            }
                        }
                        BuildStudentsDialog();
                    }
                }
            }
            ));
        }
        //Gets all students of received group
    }
}