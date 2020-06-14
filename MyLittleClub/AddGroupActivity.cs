using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyLittleClub
{
    [Activity(Label = "AddGroupActivity")]
    public class AddGroupActivity : Activity
    {
        public static Admin1 admin;
        LinearLayout AddGroupSvLayout, InsideSVLayout, OverSVLayout, AddGroupTimeAndDateLayout, OverAllAddGroupLayout, LocationAddGroupLayout, AgeAddGroupLayout, GroupLevelAddGroupLayout, ButtonAddGroupLayout, LabelAddGroupLayout;
        TextView StudTV, LabelAddGroupTV, LocationAddGroupTV, AgeAddGroupTV, GroupLevelAddGroupTV;
        TextInputEditText LocationAddGroupET, AgeAddGroupET, GroupLevelAddGroupET;
        Button AddGroupButton, AddGroupTimeButton, AddGroupDateButton;
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(530, 180);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        FirebaseFirestore database;
        RadioButton CompRBAddGroup, NotCompRBAddGroup;
        RadioGroup IsGroupCompRGAddGroup;
        public static List<Student> students = new List<Student>();
        public static List<CheckBox> CheckBoxList;
        ScrollView AddGroupStudentsSv;
        CheckBox cb;
        List<string> times;
        List<string> days;
        bool c = false;
        Spinner[] spinners;
        LinearLayout SpinnersLayout;
        ScrollView DaysSV;
        LinearLayout.LayoutParams LLLP = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(350, 200);
        Spinner FSpin;
        public static bool firstLogin = true;
        int t;
        ISharedPreferences sp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            CheckBoxList = new List<CheckBox>();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddGroupLayout);
            database = MyStuff.database;
            admin = MyStuff.GetAdmin();
            times = new List<string>();
            for (int i = 1; i<=7; i++)
            {
                times.Add(i + "");
            }
            days = new List<string>();
            days.Add("Sunday");
            days.Add("Monday");
            days.Add("Tuesday");
            days.Add("Wednsday");
            days.Add("Thursday");
            days.Add("Friday");
            days.Add("Saturday");
            BuildAddGroupScreen();
            // Create your application here
        }
        void BuildAddGroupScreen()
        {
            //Defining the parent layout
            OverAllAddGroupLayout = (LinearLayout)FindViewById(Resource.Id.AddGroupL);
            OverAllAddGroupLayout.Orientation = Orientation.Vertical;
            OverAllAddGroupLayout.SetGravity(Android.Views.GravityFlags.CenterHorizontal);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the Label AddGroup Layout
            LabelAddGroupLayout = new LinearLayout(this);
            LabelAddGroupLayout.LayoutParameters = WrapContParams;
            LabelAddGroupLayout.Orientation = Orientation.Vertical;
            LabelAddGroupLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Defining the Label AddGroup TextView
            LabelAddGroupTV = new TextView(this);
            LabelAddGroupTV.LayoutParameters = WrapContParams;
            LabelAddGroupTV.Text = "Add Group:";
            LabelAddGroupTV.TextSize = 80;
            LabelAddGroupTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LabelAddGroupTV.SetTextColor(Android.Graphics.Color.DarkRed);
            LabelAddGroupLayout.AddView(LabelAddGroupTV);
            OverAllAddGroupLayout.AddView(LabelAddGroupLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the Location AddGroup layout
            LocationAddGroupLayout = new LinearLayout(this);
            LocationAddGroupLayout.LayoutParameters = WrapContParams;
            LocationAddGroupLayout.Orientation = Orientation.Horizontal;
            //Defining the Location AddGroup TextView
            LocationAddGroupTV = new TextView(this);
            LocationAddGroupTV.LayoutParameters = WrapContParams;
            LocationAddGroupTV.Text = "Location: ";
            LocationAddGroupTV.TextSize = 30;
            LocationAddGroupTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Location AddGroup TextInputEditText
            TextInputLayout Location = new TextInputLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            LocationAddGroupET = new TextInputEditText(this);
            LocationAddGroupET.SetBackgroundResource(Resource.Drawable.MyBackground);
            LocationAddGroupET.LayoutParameters = OneTwentyParams;
            LocationAddGroupET.Hint = "Enter Adress";
            LocationAddGroupET.TextSize = 30;
            LocationAddGroupET.InputType = InputTypes.TextVariationPersonName;
            Location.AddView(LocationAddGroupET);
            //Adding views to layout
            LocationAddGroupLayout.AddView(LocationAddGroupTV);
            LocationAddGroupLayout.AddView(Location);
            OverAllAddGroupLayout.AddView(LocationAddGroupLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining Age AddGroup Layout
            AgeAddGroupLayout = new LinearLayout(this);
            AgeAddGroupLayout.LayoutParameters = WrapContParams;
            AgeAddGroupLayout.Orientation = Orientation.Horizontal;
            //Defining the Age AddGroup TextView
            AgeAddGroupTV = new TextView(this);
            AgeAddGroupTV.LayoutParameters = WrapContParams;
            AgeAddGroupTV.Text = "Age Range: ";
            AgeAddGroupTV.TextSize = 30;
            AgeAddGroupTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            AgeAddGroupTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Age AddGroup TextInputEditText
            TextInputLayout Age = new TextInputLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            AgeAddGroupET = new TextInputEditText(this);
            AgeAddGroupET.SetBackgroundResource(Resource.Drawable.MyBackground);
            AgeAddGroupET.LayoutParameters = OneTwentyParams;
            AgeAddGroupET.Hint = "Enter Age Range";
            AgeAddGroupET.TextSize = 30;
            AgeAddGroupET.SetSingleLine();
            //Adding views to layout
            AgeAddGroupLayout.AddView(AgeAddGroupTV);
            Age.AddView(AgeAddGroupET);
            AgeAddGroupLayout.AddView(Age);
            OverAllAddGroupLayout.AddView(AgeAddGroupLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining GroupLevelAddGroupLayout
            GroupLevelAddGroupLayout = new LinearLayout(this);
            GroupLevelAddGroupLayout.LayoutParameters = WrapContParams;
            GroupLevelAddGroupLayout.Orientation = Orientation.Horizontal;
            //Defining the GroupLevel AddGroup TextView
            GroupLevelAddGroupTV = new TextView(this);
            GroupLevelAddGroupTV.LayoutParameters = WrapContParams;
            GroupLevelAddGroupTV.Text = "Group Level: ";
            GroupLevelAddGroupTV.TextSize = 30;
            GroupLevelAddGroupTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            GroupLevelAddGroupTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the GroupLevel AddGroup TextInputEditText
            TextInputLayout LVL = new TextInputLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            GroupLevelAddGroupET = new TextInputEditText(this);
            GroupLevelAddGroupET.SetBackgroundResource(Resource.Drawable.MyBackground);
            GroupLevelAddGroupET.LayoutParameters = OneTwentyParams;
            GroupLevelAddGroupET.Hint = "Enter Level";
            GroupLevelAddGroupET.TextSize = 30;
            //Adding views to layout
            GroupLevelAddGroupLayout.AddView(GroupLevelAddGroupTV);
            LVL.AddView(GroupLevelAddGroupET);
            GroupLevelAddGroupLayout.AddView(LVL);
            OverAllAddGroupLayout.AddView(GroupLevelAddGroupLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining Radio Group Layout
            GroupLevelAddGroupLayout = new LinearLayout(this);
            GroupLevelAddGroupLayout.LayoutParameters = WrapContParams;
            GroupLevelAddGroupLayout.Orientation = Orientation.Horizontal;
            //Defining RadioGroup
            IsGroupCompRGAddGroup = new RadioGroup(this);
            IsGroupCompRGAddGroup.Orientation = Orientation.Horizontal;
            //Defining Competitive group Radio Button
            CompRBAddGroup = new RadioButton(this);
            CompRBAddGroup.Text = "Competetive";
            CompRBAddGroup.TextSize = 25;
            CompRBAddGroup.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            CompRBAddGroup.SetTextColor(Android.Graphics.Color.DarkBlue);
            CompRBAddGroup.Click += this.RadioButtonClick;
            //Defining not Competetive group radio Button
            NotCompRBAddGroup = new RadioButton(this);
            NotCompRBAddGroup.Text = "Not Competetive";
            NotCompRBAddGroup.TextSize = 25;
            NotCompRBAddGroup.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            NotCompRBAddGroup.SetTextColor(Android.Graphics.Color.DarkBlue);
            NotCompRBAddGroup.Click += this.RadioButtonClick;
            //Adding RB to RG
            IsGroupCompRGAddGroup.AddView(CompRBAddGroup);
            IsGroupCompRGAddGroup.AddView(NotCompRBAddGroup);
            //Adding view to overall layout
            OverAllAddGroupLayout.AddView(IsGroupCompRGAddGroup);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining AddGroup Button Layout
            ButtonAddGroupLayout = new LinearLayout(this);
            ButtonAddGroupLayout.LayoutParameters = WrapContParams;
            ButtonAddGroupLayout.Orientation = Orientation.Horizontal;
            //Defining AddGroup Button
            AddGroupButton = new Button(this);
            AddGroupButton.LayoutParameters = WrapContParams;
            AddGroupButton.Text = "Add Group";
            AddGroupButton.TextSize = 40;
            AddGroupButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddGroupButton.Click += this.AddGroupButton_Click;
            //Adding views
            ButtonAddGroupLayout.AddView(AddGroupButton);
            OverAllAddGroupLayout.AddView(ButtonAddGroupLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            BuildTimeAndDate();
        }
        //Building the AddGroup Screen
        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            
            if (InputValid(LocationAddGroupET.Text, AgeAddGroupET.Text, GroupLevelAddGroupET.Text, c))
            {
                //add to firebase
                Group group = new Group(AgeAddGroupET.Text, GroupLevelAddGroupET.Text, CompRBAddGroup.Selected, LocationAddGroupET.Text, AddGroupTimeButton.Text);
                HashMap map = new HashMap();
                map.Put("Location", group.Location);
                map.Put("Level", group.geoupLevel);
                map.Put("Age", group.age);
                map.Put("Comp", group.competetive);
                map.Put("Time", group.time);
                DocumentReference docref = database.Collection("Users").Document(admin.email).Collection("Groups").Document(group.Location + " " + group.time + " " + group.age);
                docref.Set(map);
                HashMap map2 = new HashMap();
                Toasty.Config.Instance
                    .TintIcon(true)
                    .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Success(this, "Group Added Sucesfully", 5, true).Show();
                Intent intent1 = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent1);
            }
        }
        //Adds data to firebase and intents back to main page
        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
            Toasty.Info(this, rb.Text,5, false).Show();
            c = true;
        }
        //Toasts the RadioButton's selection
        private bool InputValid(string location, string ageRange, string grouplvl, bool cb)
        {
            int x = -1;
            if (location != "" && ageRange != "" && grouplvl != "" && AddGroupTimeButton.Text != "Select Time")
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

        #region Time And Date
        //Checks all inputs and Toasts Whom Crashed the test
        public void BuildTimeAndDate()
        {
            AddGroupTimeAndDateLayout = new LinearLayout(this);
            AddGroupTimeAndDateLayout.LayoutParameters = WrapContParams;
            AddGroupTimeAndDateLayout.Orientation = Orientation.Horizontal;
            //
            AddGroupTimeButton = new Button(this);
            AddGroupTimeButton.LayoutParameters = WrapContParams;
            AddGroupTimeButton.Text = "Select Time";
            AddGroupTimeButton.TextSize = 35;
            AddGroupTimeButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddGroupTimeButton.Click += this.AddGroupTimeButton_Click;
            AddGroupTimeButton.SetTextColor(Color.DarkRed);
            //
            AddGroupTimeAndDateLayout.AddView(AddGroupTimeButton);
            OverAllAddGroupLayout.AddView(AddGroupTimeAndDateLayout);
        }
        //Builds time and date selection buttons
        private void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string str;
            string hour = "";
            if (e.Minute < 10)
            {
                if (e.HourOfDay < 10)
                {
                    hour = 0 + "" + e.HourOfDay;
                }
                else
                {
                    hour = e.HourOfDay.ToString();
                }
                str = hour + ":0" + e.Minute;
            }
            else
            {
                if (e.HourOfDay < 10)
                {
                    hour = 0 + "" + e.HourOfDay;
                }
                else
                {
                    hour = e.HourOfDay.ToString();
                }
                str = hour + ":" + e.Minute;
            }

            AddGroupTimeButton.Text = str;
        }
        //formats the string in HH:MM format
        private void AddGroupTimeButton_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            TimePickerDialog timePickerDialog = new TimePickerDialog(this, OnTimeSet, today.Hour, today.Minute, true);
            timePickerDialog.Show();
        }
        //inflates time picker dialog
        #endregion
    }
}
