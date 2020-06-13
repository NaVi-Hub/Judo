using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Util;

namespace MyLittleClub
{
    [Activity(Label = "AddMeetingActivity")]
    public class AddMeetingActivity : Activity
    {
        LinearLayout TitleLayout, SpinnerLayout, LocLayout, AgeLayout, LevelLayout, CompLayout, TimeLayout, DateLayout, SendLayout, OALayout;
        Spinner spin;
        TextView TitleTV, LocTV, AgeTV, LVLTV;
        string currGroup;
        List<string> groups;
        List<Group> GGroups;
        FirebaseFirestore database;
        Admin1 admin;
        TextInputEditText LocationET, AgeET, LVLET;
        Button TimeBtn, DateBtn, SendBtn;
        RadioGroup compRG;
        RadioButton compRB, nonCompRB;
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(420, 180);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        //https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/graphics-and-animation
        protected override void OnCreate(Bundle savedInstanceState)
        {
                        WrapContParams.SetMargins(5, 5, 5, 5);
            OneTwentyParams.SetMargins(5, 5, 5, 5);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddMeetingLayout);
            database = MyStuff.database;
            admin = MyStuff.GetAdmin();
            GetGroups();
        }

        public void BuildScreen()
        {
            #region Title
            TitleLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            //
            TitleTV = new TextView(this)
            {
                Text = "Add Meeting",
                TextSize = 50,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                LayoutParameters = WrapContParams,
            };
            TitleTV.SetTextColor(Color.DarkRed);
            TitleLayout.SetGravity(GravityFlags.CenterHorizontal);
            //
            TitleLayout.AddView(TitleTV);
            #endregion

            #region spinner
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
            #endregion

            #region Location
            LocLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            LocTV = new TextView(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Location: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            LocTV.SetTextColor(Color.DarkRed);
            LocationET = new TextInputEditText(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Location",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            LocationET.SetBackgroundResource(Resource.Drawable.MyBackground);
            LocationET.Enabled = false;
            LocLayout.AddView(LocTV);
            LocLayout.AddView(LocationET);
            #endregion

            #region Time
            TimeLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            TimeBtn = new Button(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Time",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
                Enabled = false,
            };
            TimeBtn.SetTextColor(Color.DarkRed);
            TimeLayout.AddView(TimeBtn);
            #endregion

            #region Age
            AgeLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            AgeTV = new TextView(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Age: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            AgeTV.SetTextColor(Color.DarkRed);
            AgeET = new TextInputEditText(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Age",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            AgeET.SetBackgroundResource(Resource.Drawable.MyBackground);
            AgeET.Enabled = false;
            AgeLayout.AddView(AgeTV);
            AgeLayout.AddView(AgeET);
            #endregion

            #region Radio
            CompLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            compRG = new RadioGroup(this);
            compRG.Orientation = Orientation.Vertical;
            //Defining Competitive group Radio Button
            compRB = new RadioButton(this)
            {
                Text = "Competetive",
                TextSize = 35,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            compRB.SetTextColor(Color.DarkBlue);
            //Defining not Competetive group radio Button
            nonCompRB = new RadioButton(this)
            {
                Text = "Not Competetive",
                TextSize = 35,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            nonCompRB.SetTextColor(Color.DarkBlue);
            compRG.AddView(compRB);
            compRG.AddView(nonCompRB);
            CompLayout.AddView(compRG);
            compRB.Enabled = false;
            nonCompRB.Enabled = false;
            #endregion

            #region Level
            LevelLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            LVLTV = new TextView(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Level: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            LVLTV.SetTextColor(Color.DarkRed);
            LVLET = new TextInputEditText(this)
            {
                LayoutParameters = OneTwentyParams,
                Text = "Level",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
            };
            LVLET.SetBackgroundResource(Resource.Drawable.MyBackground);
            LVLET.Enabled = false;
            LevelLayout.AddView(LVLTV);
            LevelLayout.AddView(LVLET);
            #endregion

            #region Save Button
            SendLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            SendBtn = new Button(this)
            {
                Text = "Save Meeting",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 35,
                LayoutParameters = OneTwentyParams,
            };
            SendBtn.Click += this.SendBtn_Click;
            SendLayout.AddView(SendBtn);
            #endregion

            #region Date
            DateLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            DateBtn = new Button(this)
            {
                Text = "Date",
                TextSize = 35,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                LayoutParameters = OneTwentyParams,
            };
            DateBtn.SetTextColor(Color.DarkRed);
            DateBtn.Click += this.DateBtn_Click;
            DateLayout.AddView(DateBtn);
            #endregion

            #region Adding To Views
            OALayout = FindViewById<LinearLayout>(Resource.Id.AddMeetingL);
            OALayout.SetGravity(GravityFlags.CenterHorizontal);
            OALayout.AddView(TitleLayout);
            OALayout.AddView(SpinnerLayout);
            OALayout.AddView(LocLayout);
            OALayout.AddView(TimeLayout);
            OALayout.AddView(AgeLayout);
            OALayout.AddView(LevelLayout);
            OALayout.AddView(CompLayout);
            OALayout.AddView(DateLayout);
            OALayout.AddView(SendLayout);
            #endregion
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            if (DateBtn.Text != "Date")
            {
                //add to firebase
                HashMap map = new HashMap();
                map.Put("Date", DateBtn.Text);
                DocumentReference docref = database.Collection("Users").Document(admin.email).Collection("Meetings").Document();
                docref.Set(map);
                HashMap map2 = new HashMap();
                Toasty.Success(this, "Meeting Added Sucesfully", 5, true).Show();
                Intent intent1 = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent1);
            }
            else
            {
                Toasty.Error(this, "Pick Date", 5, true).Show();
            }
        }

        private void DateBtn_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DatePickerDialog datePickerDialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
            datePickerDialog.Show();
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

            if (MyStuff.IsDateLegit(e.Date, this))
            {
                DateBtn.Text = txt;
            }
            else
            {
                Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Error(this, "InValid Date", 5, true).Show();
            }
        }
        //formats the string in DD/MM/YYYY format
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            currGroup = spinner.GetItemAtPosition(e.Position).ToString();
            for (int i = 0; i < GGroups.Count; i++)
            {
                if(GGroups[i].Location + " " + GGroups[i].time + " " + GGroups[i].age == currGroup)
                {
                    GCurrentGroup = GGroups[i];
                    LocationET.Text = GGroups[i].Location;
                    TimeBtn.Text = GGroups[i].time;
                    AgeET.Text = GGroups[i].age;
                    LVLET.Text = GGroups[i].geoupLevel;
                    if (GGroups[i].competetive)
                    {
                        compRG.Check(compRB.Id);
                    }
                    else
                    {
                        compRG.Check(nonCompRB.Id);
                    }
                }
            }
        }
        //Toasts the Selected item from the Spinner and saves it to CurrGroup
        Group GCurrentGroup;
        public void GetGroups()
        {
            GGroups = new List<Group>();
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
                            string loc = item.GetString("Location");
                            string tim = item.GetString("Time");
                            string age = item.GetString("Age");
                            string date = item.GetString("Date");
                            bool comp = bool.Parse(item.GetBoolean("Comp").ToString());
                            string lvl = item.GetString("Level");
                            GGroups.Add(new Group(age, lvl, comp, loc, tim));
                        }
                    }
                    GetAStringGroupList();
                    BuildScreen();
                }
            }
            ));
        }
        //Gets Specified Groups
        public void GetAStringGroupList()
        {
            groups = new List<string>();
            for(int i = 0; i<GGroups.Count; i++)
            {
                groups.Add(GGroups[i].Location + " " + GGroups[i].time + " " + GGroups[i].age);
            }
        }
    }
}