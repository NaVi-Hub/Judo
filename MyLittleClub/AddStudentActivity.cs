using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyLittleClub
{
    [Activity(Label = "AddStudentActivity")]
    public class AddStudentActivity : Activity
    {
        public static Admin1 admin;
        LinearLayout ButtonSendToMainPageLayout, OverAllAddStudentLayout, NameAddStudentLayout, PhoneNumAddStudentLayout, Parent1NameAddStudentLayout, Parent2NameAddStudentLayout, ButtonAddStudentLayout, AddStudentExplenationETLayout, LabelAddStudentLayout, EmailAddStudentLayout, CompetetiveRadioStudentLayout, AddStudentExplenationLayout;
        TextView LabelAddStudentTV, NameAddStudentTV, PhoneNumAddStudentTV, Parent1NameAddStudentTV, Parent2NameAddStudentTV, EmailAddStudentTV, AddStudentExplenationTV;
        EditText NameAddStudentET, PhoneNumAddStudentET, Parent1NameAddStudentET, Parent2NameAddStudentET, EmailAddStudentET, AddStudentExplenationET;
        Button AddStudentButton, SendBackToMainButton;
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(530, 160);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        FirebaseFirestore database;
        Student student;
        Spinner spin;
        LinearLayout SpinnerLayout;
        List<string> groups;
        string groupname;
        ISharedPreferences sp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddStudentsLayout);
            database = Context.database;
            admin = MyStuff.GetAdmin();
            groups = GetGroups();
            // Create your application here
        }
       
        void BuildAddStudentScreen()
        {
            //Defining the parent layout
            OverAllAddStudentLayout = (LinearLayout)FindViewById(Resource.Id.AddStudentl);
            OverAllAddStudentLayout.Orientation = Orientation.Vertical;
            OverAllAddStudentLayout.SetGravity(Android.Views.GravityFlags.CenterHorizontal);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the Label AddStudent Layout
            LabelAddStudentLayout = new LinearLayout(this);
            LabelAddStudentLayout.LayoutParameters = WrapContParams;
            LabelAddStudentLayout.Orientation = Orientation.Vertical;
            LabelAddStudentLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Defining the Label AddStudent TextView
            LabelAddStudentTV = new TextView(this);
            LabelAddStudentTV.LayoutParameters = WrapContParams;
            LabelAddStudentTV.Text = "New Student";
            LabelAddStudentTV.TextSize = 60;
            LabelAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LabelAddStudentTV.SetTextColor(Android.Graphics.Color.DarkRed);
            LabelAddStudentLayout.AddView(LabelAddStudentTV);
            OverAllAddStudentLayout.AddView(LabelAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the Name AddStudent layout
            NameAddStudentLayout = new LinearLayout(this);
            NameAddStudentLayout.LayoutParameters = WrapContParams;
            NameAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining the Name AddStudent TextView
            NameAddStudentTV = new TextView(this);
            NameAddStudentTV.LayoutParameters = WrapContParams;
            NameAddStudentTV.Text = "Name: ";
            NameAddStudentTV.TextSize = 30;
            NameAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Name AddStudent EditText
            NameAddStudentET = new EditText(this);
            NameAddStudentET.LayoutParameters = OneTwentyParams;
            NameAddStudentET.Hint = "Full Name";
            NameAddStudentET.TextSize = 30;
            NameAddStudentET.FirstBaselineToTopHeight = 10;
            //Adding views to layout
            NameAddStudentLayout.AddView(NameAddStudentTV);
            NameAddStudentLayout.AddView(NameAddStudentET);
            OverAllAddStudentLayout.AddView(NameAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining PhoneNum AddStudent Layout
            PhoneNumAddStudentLayout = new LinearLayout(this);
            PhoneNumAddStudentLayout.LayoutParameters = WrapContParams;
            PhoneNumAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining the PhoneNum AddStudent TextView
            PhoneNumAddStudentTV = new TextView(this);
            PhoneNumAddStudentTV.LayoutParameters = WrapContParams;
            PhoneNumAddStudentTV.Text = "Phone # ";
            PhoneNumAddStudentTV.TextSize = 30;
            PhoneNumAddStudentTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            PhoneNumAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the PhoneNum AddStudent EditText
            PhoneNumAddStudentET = new EditText(this);
            PhoneNumAddStudentET.LayoutParameters = OneTwentyParams;
            PhoneNumAddStudentET.Text = "05";
            PhoneNumAddStudentET.TextSize = 30;
            PhoneNumAddStudentET.SetSingleLine();
            PhoneNumAddStudentET.InputType = InputTypes.ClassPhone;
            //Adding views to layout
            PhoneNumAddStudentLayout.AddView(PhoneNumAddStudentTV);
            PhoneNumAddStudentLayout.AddView(PhoneNumAddStudentET);
            OverAllAddStudentLayout.AddView(PhoneNumAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            // Defining Email AddStudentLayout
            EmailAddStudentLayout = new LinearLayout(this);
            EmailAddStudentLayout.LayoutParameters = WrapContParams;
            EmailAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining the Email AddStudent TextView
            EmailAddStudentTV = new TextView(this);
            EmailAddStudentTV.LayoutParameters = WrapContParams;
            EmailAddStudentTV.Text = "Enter Email: ";
            EmailAddStudentTV.TextSize = 30;
            EmailAddStudentTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            EmailAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Email AddStudent EditText
            EmailAddStudentET = new EditText(this);
            EmailAddStudentET.LayoutParameters = OneTwentyParams;
            EmailAddStudentET.Hint = "Email";
            EmailAddStudentET.InputType = InputTypes.TextVariationEmailAddress;
            EmailAddStudentET.TextSize = 30;
            EmailAddStudentET.SetSingleLine();
            //Adding views to layout
            EmailAddStudentLayout.AddView(EmailAddStudentTV);
            EmailAddStudentLayout.AddView(EmailAddStudentET);
            OverAllAddStudentLayout.AddView(EmailAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining Parent1NameAddStudentLayout
            Parent1NameAddStudentLayout = new LinearLayout(this);
            Parent1NameAddStudentLayout.LayoutParameters = WrapContParams;
            Parent1NameAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining the Parent1Name AddStudent TextView
            Parent1NameAddStudentTV = new TextView(this);
            Parent1NameAddStudentTV.LayoutParameters = WrapContParams;
            Parent1NameAddStudentTV.Text = "Parent1 Name: ";
            Parent1NameAddStudentTV.TextSize = 30;
            Parent1NameAddStudentTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            Parent1NameAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Parent1Name AddStudent EditText
            Parent1NameAddStudentET = new EditText(this);
            Parent1NameAddStudentET.LayoutParameters = OneTwentyParams;
            Parent1NameAddStudentET.Hint = "Parent1";
            Parent1NameAddStudentET.TextSize = 30;
            //Adding views to layout
            Parent1NameAddStudentLayout.AddView(Parent1NameAddStudentTV);
            Parent1NameAddStudentLayout.AddView(Parent1NameAddStudentET);
            OverAllAddStudentLayout.AddView(Parent1NameAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining Parent2NameAddStudentLayout
            Parent2NameAddStudentLayout = new LinearLayout(this);
            Parent2NameAddStudentLayout.LayoutParameters = WrapContParams;
            Parent2NameAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining the Parent2Name AddStudent TextView
            Parent2NameAddStudentTV = new TextView(this);
            Parent2NameAddStudentTV.LayoutParameters = WrapContParams;
            Parent2NameAddStudentTV.Text = "Parent2 Name: ";
            Parent2NameAddStudentTV.TextSize = 30;
            Parent2NameAddStudentTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            Parent2NameAddStudentTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Parent2Name AddStudent EditText
            Parent2NameAddStudentET = new EditText(this);
            Parent2NameAddStudentET.LayoutParameters = OneTwentyParams;
            Parent2NameAddStudentET.Hint = "Parent2";
            Parent2NameAddStudentET.TextSize = 30;
            //Adding views to layout
            Parent2NameAddStudentLayout.AddView(Parent2NameAddStudentTV);
            Parent2NameAddStudentLayout.AddView(Parent2NameAddStudentET);
            OverAllAddStudentLayout.AddView(Parent2NameAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the AddStudent Explenation layout
            AddStudentExplenationLayout = new LinearLayout(this);
            AddStudentExplenationLayout.LayoutParameters = WrapContParams;
            AddStudentExplenationLayout.Orientation = Orientation.Vertical;
            //Defining the Explenation AddStudent TextView
            AddStudentExplenationTV = new TextView(this);
            AddStudentExplenationTV.LayoutParameters = WrapContParams;
            AddStudentExplenationTV.Text = "Student Notes: ";
            AddStudentExplenationTV.TextSize = 30;
            AddStudentExplenationTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Adding views to layout
            AddStudentExplenationLayout.AddView(AddStudentExplenationTV);
            OverAllAddStudentLayout.AddView(AddStudentExplenationLayout);
            //Defining The AddStudent Explenation ET layout
            AddStudentExplenationETLayout = new LinearLayout(this);
            AddStudentExplenationETLayout.LayoutParameters = new LinearLayout.LayoutParams(1100, 400);
            AddStudentExplenationETLayout.Orientation = Orientation.Vertical;
            AddStudentExplenationETLayout.SetBackgroundResource(Resource.Drawable.BlackOutLine);
            AddStudentExplenationETLayout.Click += this.AddStudentExplenationETLayout_Click;
            //Defining the Explenation AddStudent EditText
            AddStudentExplenationET = new EditText(this);
            AddStudentExplenationET.SetWidth(LinearLayout.LayoutParams.MatchParent);
            AddStudentExplenationET.Hint = "Notes";
            AddStudentExplenationET.TextSize = 25;
            AddStudentExplenationET.SetTextIsSelectable(true);
            AddStudentExplenationET.InputType = InputTypes.TextFlagMultiLine;
            AddStudentExplenationET.Gravity = GravityFlags.Top;
            AddStudentExplenationET.SetSingleLine(false);
            AddStudentExplenationET.SetBackgroundColor(Color.Transparent);
            //Adding viwes to overall layout
            AddStudentExplenationETLayout.AddView(AddStudentExplenationET);
            OverAllAddStudentLayout.AddView(AddStudentExplenationETLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining AddStudent Button Layout
            ButtonAddStudentLayout = new LinearLayout(this);
            ButtonAddStudentLayout.LayoutParameters = WrapContParams;
            ButtonAddStudentLayout.Orientation = Orientation.Horizontal;
            //Defining AddStudent Button
            AddStudentButton = new Button(this);
            AddStudentButton.LayoutParameters = WrapContParams;
            AddStudentButton.Text = "Add Student";
            AddStudentButton.TextSize = 40;
            AddStudentButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddStudentButton.Click += this.AddStudentButton_Click;
            //Adding views
            ButtonAddStudentLayout.AddView(AddStudentButton);
            OverAllAddStudentLayout.AddView(ButtonAddStudentLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Declare Spinner
            spin = new Spinner(this);
            spin.LayoutParameters = OneTwentyParams;
            spin.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, groups);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = adapter;
            //Defining Spinner Layout
            SpinnerLayout = new LinearLayout(this);
            SpinnerLayout.LayoutParameters = WrapContParams;
            SpinnerLayout.Orientation = Orientation.Horizontal;
            //Adding Views
            SpinnerLayout.AddView(spin);
            OverAllAddStudentLayout.AddView(SpinnerLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining AddStudent Button Layout
            ButtonSendToMainPageLayout = new LinearLayout(this);
            ButtonSendToMainPageLayout.LayoutParameters = WrapContParams;
            ButtonSendToMainPageLayout.Orientation = Orientation.Horizontal;
            //Defining AddStudent Button
            SendBackToMainButton = new Button(this);
            SendBackToMainButton.LayoutParameters = WrapContParams;
            SendBackToMainButton.Text = "Send Back To\nMain Page";
            SendBackToMainButton.TextSize = 40;
            SendBackToMainButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            SendBackToMainButton.SetTextColor(Color.DarkRed);
            SendBackToMainButton.Click += this.SendBackToMainButton_Click;
            //Adding views
            ButtonSendToMainPageLayout.AddView(SendBackToMainButton);
            OverAllAddStudentLayout.AddView(ButtonSendToMainPageLayout);
        }
        //Building Screen
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            currGroup = spinner.GetItemAtPosition(e.Position).ToString();
        }
        //Toasts the Selected item from the Spinner and saves it to CurrGroup
        string currGroup;
        private void SendBackToMainButton_Click(object sender, EventArgs e)
        {
            Intent intent1 = new Intent(this, typeof(MainPageActivity));
            StartActivity(intent1);
        }
        //Building the AddStudent Screen
        private void AddStudentButton_Click(object sender, EventArgs e)
        {
            if (IsValidName(NameAddStudentET.Text) && MyStuff.isValidEmail(EmailAddStudentET.Text, this) && spin.SelectedView.ToString() != "Choose Group")
            {
                if (Parent1NameAddStudentET.Text != "" && Parent2NameAddStudentET.Text != "") { student = new Student(NameAddStudentET.Text, PhoneNumAddStudentET.Text, EmailAddStudentET.Text, Parent1NameAddStudentET.Text, Parent2NameAddStudentET.Text, AddStudentExplenationET.Text, currGroup); }
                if (Parent1NameAddStudentET.Text == "" && Parent2NameAddStudentET.Text != "") { student = new Student(NameAddStudentET.Text, PhoneNumAddStudentET.Text, EmailAddStudentET.Text, Parent2NameAddStudentET.Text, AddStudentExplenationET.Text, currGroup); }
                if (Parent1NameAddStudentET.Text != "" && Parent2NameAddStudentET.Text == "") { student = new Student(NameAddStudentET.Text, PhoneNumAddStudentET.Text, EmailAddStudentET.Text, Parent1NameAddStudentET.Text, AddStudentExplenationET.Text, currGroup); }
                if (Parent1NameAddStudentET.Text == "" && Parent2NameAddStudentET.Text == "") { student = new Student(NameAddStudentET.Text, PhoneNumAddStudentET.Text, EmailAddStudentET.Text, AddStudentExplenationET.Text, currGroup); }

                HashMap map = new HashMap();
                map.Put("Name", student.name);
                map.Put("PhoneNum", student.phoneNumber);
                map.Put("Email", student.email);
                map.Put("Parent1", student.parentName1);
                map.Put("Parent2", student.parentName2);
                map.Put("Notes", student.notes);
                map.Put("Group", student.group);
                DocumentReference docref = database.Collection("Users").Document(admin.email).Collection("Students").Document(student.name + " " + student.phoneNumber);
                docref.Set(map);
                Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Success(this, "Student Added Sucesfully", 5, true).Show();
                NameAddStudentET.Text = "";
                PhoneNumAddStudentET.Text = "05";
                EmailAddStudentET.Text = "";
                Parent1NameAddStudentET.Text = "";
                Parent2NameAddStudentET.Text = "";
                AddStudentExplenationET.Text = "";
                spin.SetSelection(0);
            }
            else
            {
                Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Error(this, "Input InValid", 5, true).Show();
            }
        }
        //intents back to main page
        private void AddStudentExplenationETLayout_Click(object sender, EventArgs e)
        {
            AddStudentExplenationET.RequestFocus();
            MyStuff.showSoftKeyboard(this, AddStudentExplenationET);
            //@Tomer
            //https://gist.github.com/icalderond/742f98f2f8cda1fae1b0bc877df76bbc @Javier Pardo
        }
        //Makes a big EditText


        //Email Validaton
        public bool IsValidName(string name)
        {
            bool Tr = true;
            List<string> FN = new List<string>();
            FN.Add("Dick");
            FN.Add("69");
            FN.Add("420");
            FN.Add("Sex");
            FN.Add("Pussy");
            FN.Add("pussy");
            FN.Add("dick");
            FN.Add("sex");
            Tr = name.Length >= 4 && name.Length <= 16;
            for (int i = 0; i < FN.Count; i++)
            {
                if (name.Contains(FN[i]))
                {
                    Tr = false;
                }
            }
            if (!Tr) 
            {
                Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf")); 
                Toasty.Error(this, "Name InValid", 5, true).Show(); return Tr;
            }
            else
            {
                return Tr;
            }
        }
        //Name Validation
        public List<String> GetGroups()
        {
            groups = new List<string>();
            groups.Add("Choose Group");
            Query query = database.Collection("Users").Document(admin.email).Collection("Groups");
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
                            groupname = (item.GetString("Location")).ToString() + " " + (item.GetString("Time")).ToString() + " " + (item.GetString("Age")).ToString();
                            groups.Add(groupname);
                        }
                    }
                }
                BuildAddStudentScreen();
            }));
            return groups;
        }
        //Retreiving Groups From FireBase
    }
}