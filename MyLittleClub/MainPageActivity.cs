using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Lang;
using Java.Security;
using Java.Util;
using Java.Util.Logging;
using Newtonsoft.Json;
using Org.Apache.Http.Impl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Android.Widget.CalendarView;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace MyLittleClub
{
    [Activity(Theme = "@style/AppTheme")]
    public class MainPageActivity : AppCompatActivity, IOnDateChangeListener
    {
        protected Android.Media.MediaPlayer player = MyStuff.player;
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
        List<string> dates;
        List<Group> groups;
        Button MainPageShowGroupsbtn;
        Dialog d;
        ViewGroup.LayoutParams VLP = new ViewGroup.LayoutParams(600, 800);
        CalendarView calendar;
        Switch Swi;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            admin1 = MyStuff.GetAdmin();
            database = MyStuff.database;
            SetContentView(Resource.Layout.MainPageLayout);
            GetDates();
        }
        public void BuildMainPage()
        {
            //Main Page Overall Layout defining
            MainPageOverallLayout = FindViewById<LinearLayout>(Resource.Id.AAA);
            MainPageOverallLayout.Orientation = Orientation.Vertical;
            MainPageOverallLayout.SetGravity(GravityFlags.CenterHorizontal);
            BuildCalendar();
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
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
            MainPageProfilePictureLayout.Orientation = Orientation.Horizontal;
            MainPageProfilePictureLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Profile Pic
            Profile = new ImageView(this);
            Profile.SetImageBitmap(MyStuff.ConvertStringToBitMap(admin1.ProfilePic));
            Profile.SetMaxWidth(250);
            Profile.SetMinimumHeight(400);
            Profile.Click += this.Profile_Click;
            MainPageProfilePictureLayout.AddView(Profile);
            //
            Swi = new Switch(this);
            Swi.SetHeight(15);
            Swi.SetWidth(70);
            Swi.Checked = false;
            Swi.TextOff = "Off";
            Swi.TextOn = "On";
            Swi.CheckedChange += this.Swi_CheckedChange;
            MainPageProfilePictureLayout.AddView(Swi);
            //
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
            MainPageShowGroupsbtn.LayoutParameters = new LinearLayout.LayoutParams(500, 250);
            MainPageShowGroupsbtn.Text = "Show Groups";
            MainPageOverallLayout.AddView(MainPageShowGroupsbtn);
            MainPageShowGroupsbtn.Click += this.MainPageShowGroupsbtn_Click;
            //

        }
        //Build Main Page's Views

        #region Service
        private void Swi_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {

            if(e.IsChecked)
            {
                Toasty.Success(this, "Music Started", 5, false).Show();
                StartMyService();
            }
            else
            {
                Toasty.Success(this, "Music Stopped", 5, false).Show();
                StopMyService();
            }
        }
        public void StartMyService()
        {
            Intent i = new Intent(this, typeof(MyService));
            this.StartService(i);
        }
        public void StopMyService()
        {
            Intent i = new Intent(this, typeof(MyService));
            this.StopService(i);
        }
        #endregion

        private void Profile_Click(object sender, EventArgs e)
        {
            System.Random rnd = new System.Random();
;           Toast.MakeText(this, rnd.Next(1, 11) + "/10", ToastLength.Short).Show();
        }

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
            BuildEditGroupDialog(groups[b.id]);
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
        LinearLayout TitleLayout, LocationLayout, AgeLayout, LVLLayout, TimeAndDateLayout, SaveLayout;
        TextView TitleTV, LocTV, AgeTV, LVLTV, CompTV, TimeTV;
        TextInputEditText LocET, AgeET, LVLET;
        bool c = false;
        RadioGroup CompRG;
        RadioButton CompRB, NotCompRB;
        Button TimeButton, SaveButton;
        public void BuildEditGroupDialog(Group group)
        {
            GrouD = new Dialog(this);
            GrouD.SetContentView(Resource.Layout.MyDialog);
            GrouD.SetCancelable(true);
            //
            LinearLayout OAGroupsLayout = GrouD.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            OAGroupsLayout.Orientation = Orientation.Vertical;

            LinearLayout l2 = new LinearLayout(this);
            l2.LayoutParameters = new LinearLayout.LayoutParams(750, 850);
            l2.Orientation = Orientation.Vertical;
            l2.SetBackgroundResource(Resource.Drawable.BlackOutLine);
            //
            #region Title
            TitleLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            TitleTV = new TextView(this)
            {
                LayoutParameters = WrapContParams,
                Text = "Edit Group ",
                TextSize = 40,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            TitleLayout.AddView(TitleTV);
            #endregion

            #region Location Defining
            LocationLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            LocTV = new TextView(this)
            {
                LayoutParameters = WrapContParams,
                Text = "Location: ",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            LocTV.SetTextColor(Color.DarkRed);
            //
            LocET = new TextInputEditText(this)
            {
                LayoutParameters = WrapContParams,
                Text = group.Location,
                TextSize = 20,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            TextInputLayout Loc = new TextInputLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            LocationLayout.AddView(LocTV);
            LocationLayout.AddView(LocET);
            #endregion

            #region Age Defining
            AgeLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            AgeTV = new TextView(this)
            {
                LayoutParameters = WrapContParams,
                Text = "Age: ",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            AgeTV.SetTextColor(Color.DarkRed);
            //
            AgeET = new TextInputEditText(this)
            {
                LayoutParameters = WrapContParams,
                Text = group.age,
                TextSize = 20,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            AgeLayout.AddView(AgeTV);
            AgeLayout.AddView(AgeET);
            #endregion

            #region Level Defining
            LVLLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            LVLTV = new TextView(this)
            {
                LayoutParameters = WrapContParams,
                Text = "Group Level: ",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            LVLTV.SetTextColor(Color.DarkRed);
            //
            LVLET = new TextInputEditText(this)
            {
                LayoutParameters = WrapContParams,
                Text = group.geoupLevel,
                TextSize = 20,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            LVLLayout.AddView(LVLTV);
            LVLLayout.AddView(LVLET);
            #endregion

            #region Competetive defining
            CompRG = new RadioGroup(this);
            CompRG.Orientation = Orientation.Vertical;
            //Defining Competitive group Radio Button
            CompRB = new RadioButton(this)
            { 
                Text = "Competetive",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            CompRB.SetTextColor(Android.Graphics.Color.DarkBlue);
            CompRB.Click += this.RB_Click;
            //Defining not Competetive group radio Button
            NotCompRB = new RadioButton(this)
            {
                Text = "Not Competetive",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            NotCompRB.SetTextColor(Android.Graphics.Color.DarkBlue);
            NotCompRB.Click += this.RB_Click;

            CompRG.AddView(CompRB);
            CompRG.AddView(NotCompRB);
            if (group.competetive)
            {
                CompRG.Check(CompRB.Id);
            }
            else
            {
                CompRG.Check(NotCompRB.Id);
            }
            #endregion

            #region Time And Date
            TimeAndDateLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            TimeButton = new Button(this)
            {
                LayoutParameters = WrapContParams,
                Text = group.time,
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            TimeButton.SetTextColor(Color.Red);
            TimeButton.Click += this.TimeButton_Click;
            TimeAndDateLayout.AddView(TimeButton);
            #endregion

            #region Save Button
            SaveLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            SaveButton = new Button(this)
            {
                LayoutParameters = WrapContParams,
                Text = "Save",
                TextSize = 25,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            SaveButton.SetBackgroundColor(Color.BurlyWood);
            SaveButton.Click += this.SaveButton_Click;
            SaveLayout.AddView(SaveButton);
            #endregion

            l2.AddView(TitleLayout);
            l2.AddView(LocationLayout);
            l2.AddView(AgeLayout);
            l2.AddView(LVLLayout);
            l2.AddView(CompRG);
            l2.AddView(TimeAndDateLayout);
            l2.AddView(SaveLayout);
            OAGroupsLayout.AddView(l2);
            //
            GrouD.Show();
        }
        private bool InputValid(string location, string ageRange, string grouplvl, bool cb)
        {
            int x = -1;
            if (location != "" && ageRange != "" && grouplvl != "" && TimeButton.Text != "Select Time")
            {
                return true;
            }
            else
            {
                if (location == "" && ageRange != "" && grouplvl != "" && cb)
                {
                    x = 0;
                }
                else if (location != "" && ageRange == "" && grouplvl != "" && cb)
                {
                    x = 1;
                }
                else if (location != "" && ageRange != "" && grouplvl == "" && cb)
                {
                    x = 2;
                }
                else if (location != "" && ageRange != "" && grouplvl != "" && !cb)
                {
                    x = 4;
                }
                else if (location != "" && ageRange != "" && grouplvl != "" && cb)
                {
                    x = 5;
                }
                else
                {
                    x = 3;
                }
            }
            switch (x)
            {
                case 0:
                    Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    Toasty.Error(this, "Location InValid", 5, true).Show();
                    return false;
                case 1:
                    Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    Toasty.Error(this, "Age Range InValid", 5, true).Show();
                    return false;
                case 2:
                    Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    Toasty.Error(this, "Group Level InValid", 5, true).Show();
                    return false;
                case 3:
                    Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    Toasty.Error(this, "Some Imputs Are InValid", 5, true).Show();
                    return false;
                case 4:
                    Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    Toasty.Error(this, "The Competitivness Was'nt Clicked", 5, true).Show();
                    return false;
                default:
                    return false;
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (InputValid(LocET.Text, AgeET.Text, LVLET.Text, c))
            {
                //add to firebase
                Group group = new Group(AgeET.Text, LVLET.Text, CompRB.Checked, LocET.Text, TimeButton.Text);
                HashMap map = new HashMap();
                map.Put("Location", group.Location);
                map.Put("Level", group.geoupLevel);
                map.Put("Age", group.age);
                map.Put("Comp", group.competetive);
                map.Put("Time", group.time);
                DocumentReference docref = database.Collection("Users").Document(admin1.email).Collection("Groups").Document(group.Location + " " + group.time + " " + group.age);
                docref.Set(map);
                HashMap map2 = new HashMap();
                Toasty.Config.Instance
                    .TintIcon(true)
                    .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Success(this, "Group Added Sucesfully", 5, true).Show();
                GrouD.Dismiss();
                d.Dismiss();
                this.Recreate();
            }
        }

        #region Time And Date Funcs
        private void DateButton_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DatePickerDialog datePickerDialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
            datePickerDialog.Show();
        }
        private void TimeButton_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            TimePickerDialog timePickerDialog = new TimePickerDialog(this, OnTimeSet, today.Hour, today.Minute, true);
            timePickerDialog.Show();
        }
        private void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string str;
            if (e.Minute < 10)
            {
                str = e.HourOfDay + ":0" + e.Minute;
            }
            else
            {
                str = e.HourOfDay + ":" + e.Minute;
            }

            TimeButton.Text = str;
        }
        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            string txt;
            if (e.Date.Month < 10 && e.Date.Day < 10)
            {
                txt = string.Format("0{0}.0{1}.{2}", e.Date.Day, e.Date.Month, e.Date.Year);
            }
            else if (e.Date.Day < 10)
            {
                txt = string.Format("0{0}.{1}.{2}", e.Date.Day, e.Date.Month, e.Date.Year);
            }
            else if (e.Date.Month < 10)
            {
                txt = string.Format("{0}.0{1}.{2}", e.Date.Day, e.Date.Month, e.Date.Year);
            }
            else
            {
                txt = string.Format("{0}.{1}.{2}", e.Date.Day, e.Date.Month, e.Date.Year);
            }

        }
            //formats the string in DD/MM/YYYY format
            #endregion

        private void RB_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
            Toasty.Info(this, rb.Text, 5, false).Show();
            c = true;
        }

        //Defining and adding views to layout
        /*public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        } */
        //Menu inflator
        /*public override bool OnOptionsItemSelected(IMenuItem item)
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
                        Intent tryIntent = new Intent(this, typeof(AddMeetingActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem3:
                    {
                        Intent tryIntent = new Intent(this, typeof(AddExerciseActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem4:
                    {
                        Intent tryIntent = new Intent(this, typeof(AddStudentActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem5:
                    {
                        Intent tryIntent = new Intent(this, typeof(BuildTrainingActivity));
                        StartActivity(tryIntent);
                        return true;
                    }
                case Resource.Id.menuItem6: 
                    {
                        Intent tryIntent = new Intent(this, typeof(EditAdminActivity));
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
        }*/
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
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
                            bool comp = item.GetBoolean("Comp").BooleanValue();
                            string level = (item.GetString("Level")).ToString();
                            string time = (item.GetString("Time")).ToString();
                            Group group1 = new Group(Age, level, comp, loc, time);
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
            Query query = database.Collection("Users").Document(admin1.email).Collection("Meetings");
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
                            try
                            {
                                string day = (item.GetString("Date").ToString())[0] + "" + (item.GetString("Date").ToString())[1];
                                string month = (item.GetString("Date").ToString())[3] + "" + (item.GetString("Date").ToString())[4];
                                string year = (item.GetString("Date").ToString())[6] + "" + (item.GetString("Date").ToString())[7] + "" + (item.GetString("Date").ToString())[8] + "" + (item.GetString("Date").ToString())[9];
                                int inday = int.Parse(day);
                                int inmonth = int.Parse(month);
                                int inyear = int.Parse(year);
                                dates.Add(MyStuff.MakeDateString(inyear, inmonth, inday));
                            }
                            catch
                            {
                                Toasty.Normal(this, "Empty", 5).Show();
                            }
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