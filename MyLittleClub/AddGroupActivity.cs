using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
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
        EditText LocationAddGroupET, AgeAddGroupET, GroupLevelAddGroupET;
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
        bool c;
        public static bool firstLogin = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            CheckBoxList = new List<CheckBox>();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddGroupLayout);
            database = OpenActivity.database;
            admin = MainPageActivity.admin1;
            if (firstLogin)
            {
                firstLogin = !firstLogin;
                GetStudents();
                c = false;
            }
            else
            {
                BuildAddGroupScreen();
            }

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
            //Defining the Location AddGroup EditText
            LocationAddGroupET = new EditText(this);
            LocationAddGroupET.LayoutParameters = OneTwentyParams;
            LocationAddGroupET.Hint = "Enter Adress";
            LocationAddGroupET.TextSize = 30;
            LocationAddGroupET.InputType = InputTypes.TextVariationPersonName;
            //Adding views to layout
            LocationAddGroupLayout.AddView(LocationAddGroupTV);
            LocationAddGroupLayout.AddView(LocationAddGroupET);
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
            //Defining the Age AddGroup EditText
            AgeAddGroupET = new EditText(this);
            AgeAddGroupET.LayoutParameters = OneTwentyParams;
            AgeAddGroupET.Hint = "Enter Age Range";
            AgeAddGroupET.TextSize = 30;
            AgeAddGroupET.SetSingleLine();
            //Adding views to layout
            AgeAddGroupLayout.AddView(AgeAddGroupTV);
            AgeAddGroupLayout.AddView(AgeAddGroupET);
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
            //Defining the GroupLevel AddGroup EditText
            GroupLevelAddGroupET = new EditText(this);
            GroupLevelAddGroupET.LayoutParameters = OneTwentyParams;
            GroupLevelAddGroupET.Hint = "Enter Level";
            GroupLevelAddGroupET.TextSize = 30;
            //Adding views to layout
            GroupLevelAddGroupLayout.AddView(GroupLevelAddGroupTV);
            GroupLevelAddGroupLayout.AddView(GroupLevelAddGroupET);
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
            BuildScrollView();
        }
        //Building the AddGroup Screen

        public void BuildScrollView()
        {
            for (int i = 0; i < students.Count; i++)
            {
                CheckBoxList.Add(new CheckBox(this));
            }
            OverSVLayout = new LinearLayout(this);
            OverSVLayout.LayoutParameters = new ViewGroup.LayoutParams(1100, 700);
            OverSVLayout.Orientation = Orientation.Vertical;
            AddGroupStudentsSv = new ScrollView(this);
            AddGroupStudentsSv.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
            OverSVLayout.AddView(AddGroupStudentsSv);
            AddGroupSvLayout = new LinearLayout(this);
            AddGroupSvLayout.LayoutParameters = MatchParentParams;
            AddGroupSvLayout.Orientation = Orientation.Vertical;

            for (int i = 0; i < students.Count; i++)
            {
                //Define Inside Layout
                InsideSVLayout = new LinearLayout(this);
                InsideSVLayout.Orientation = Orientation.Horizontal;
                InsideSVLayout.SetForegroundGravity(Android.Views.GravityFlags.Center);
                InsideSVLayout.LayoutParameters = new ViewGroup.LayoutParams(1100, 150);
                InsideSVLayout.SetBackgroundResource(Resource.Drawable.BlackOutLine);
                //Define CheckBox
                cb = CheckBoxList[i];
                cb.SetWidth(60);
                cb.SetHeight(40);
                cb.Text = "" + i;
                //Define Textview
                StudTV = new TextView(this);
                StudTV.SetHeight(40);
                StudTV.SetWidth(1100);
                StudTV.Text = students[i].name + " " + students[i].parentName1;
                try
                {
                    InsideSVLayout.AddView(cb);
                    InsideSVLayout.AddView(StudTV);
                }
                catch { Toasty.Warning(this, "Not FIrst time open", 5, false).Show(); }
                AddGroupSvLayout.AddView(InsideSVLayout);
            }
            AddGroupStudentsSv.AddView(AddGroupSvLayout);
            OverAllAddGroupLayout.AddView(OverSVLayout);
        }
        //Building the ScrollView

        public void GetStudents()
        {
            Query query = database.Collection("Users").Document(admin.email).Collection("Students");
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
                            Student s = new Student();
                            s.name = item.Get("Name").ToString();
                            s.parentName1 = item.Get("Parent1").ToString();
                            s.parentName2 = item.Get("Parent2").ToString();
                            s.phoneNumber = item.Get("PhoneNum").ToString();
                            s.email = item.Get("Email").ToString();
                            students.Add(s);
                        }
                    }
                }
                BuildAddGroupScreen();
            }
            ));
        }
        //Gets all the students from the FireBase

        private void AddGroupButton_Click(object sender, EventArgs e)
        {

            if (InputValid(LocationAddGroupET.Text, AgeAddGroupET.Text, GroupLevelAddGroupET.Text, c))
            {
                //add to firebase
                Group group = new Group(AgeAddGroupET.Text, GroupLevelAddGroupET.Text, CompRBAddGroup.Selected, LocationAddGroupET.Text, AddGroupDateButton.Text, AddGroupTimeButton.Text);
                HashMap map = new HashMap();
                map.Put("Location", group.Location);
                map.Put("Level", group.geoupLevel);
                map.Put("Age", group.age);
                map.Put("Comp", group.competetive);
                map.Put("Date", group.date);
                map.Put("Time", group.time);
                DocumentReference docref = database.Collection("Users").Document(admin.email).Collection("Groups").Document(group.Location + " " + group.time + " " + group.age);
                docref.Set(map);
                HashMap map2 = new HashMap();

                Toasty.Success(this, "Group Added Sucesfully", 5, false).Show();
                Intent intent1 = new Intent(this, typeof(MainPageActivity));
                intent1.PutExtra("Admin", JsonConvert.SerializeObject(admin));
                StartActivity(intent1);
            }
        }
        //Adds data to firebase and intents back to main page

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toasty.Info(this, rb.Text,5, false).Show();
            c = true;
        }
        //Toasts the RadioButton's selection

        private bool InputValid(string location, string ageRange, string grouplvl, bool cb)
        {
            int x = -1;
            if (location != "" && ageRange != "" && grouplvl != "" && AddGroupTimeButton.Text != "Select Time" && AddGroupDateButton.Text != "Select Date")
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
                    Toasty.Error(this, "Location InValid", 5, false).Show();
                    return false;
                case 1:
                    Toasty.Error(this, "Age Range InValid", 5, false).Show();
                    return false;
                case 2:
                    Toasty.Error(this, "Group Level InValid", 5, false).Show();
                    return false;
                case 3:
                    Toasty.Error(this, "Some Imputs Are InValid", 5, false).Show();
                    return false;
                case 4:
                    Toasty.Error(this, "The Competitivness Was'nt Clicked", 5, false).Show();
                    return false;
                default:
                    return false;
            }
        }
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
            AddGroupDateButton = new Button(this);
            AddGroupDateButton.LayoutParameters = WrapContParams;
            AddGroupDateButton.Text = "Select Date";
            AddGroupDateButton.TextSize = 35;
            AddGroupDateButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddGroupDateButton.Click += this.AddGroupDateButton_Click;
            AddGroupDateButton.SetTextColor(Color.DarkRed);
            //
            AddGroupTimeAndDateLayout.AddView(AddGroupTimeButton);
            AddGroupTimeAndDateLayout.AddView(AddGroupDateButton);
            OverAllAddGroupLayout.AddView(AddGroupTimeAndDateLayout);
        }
        //builds time and date selection buttons
        private void AddGroupDateButton_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DatePickerDialog datePickerDialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
            datePickerDialog.Show();
        }
        //inflates date picker dialog
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

            if (IsDateLegit(e.Date))
            {
                AddGroupDateButton.Text = txt;
            }
            else
            {
                Toasty.Error(this, "InValid Date",5, false).Show();
            }
        }
        //: formats the string in DD/MM/YYYY format
        public bool IsDateLegit(DateTime date)
        {
            if (date.Year < DateTime.Today.Year)
            {
                return false;
            }
            else if (date.Year > DateTime.Today.Year)
            {
                return true;
            }
            else
            {
                if (date.Month > DateTime.Today.Month)
                {
                    return true;
                }
                else if (date.Month > DateTime.Today.Month)
                {
                    return true;
                }
                else
                {
                    if (date.Day >= DateTime.Today.Day)
                    {
                        return true;
                    }
                    else if (date.Day > DateTime.Today.Day)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        //makes sure the date is in the future
    }
}
