using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;

namespace MyLittleClub
{
    [Activity(Label = "AddMeetingActivity")]
    public class AddMeetingActivity : Activity
    {
        LinearLayout TitleLayout, SpinnerLayout;
        Spinner spin;
        TextView TitleTV;
        string currGroup;
        List<string> groups;
        List<Group> GGroups;
        FirebaseFirestore database;
        Admin1 admin;
        EditText LocationET, AgeET, LVLET;
        Button TimeBtn, DateBtn;
        RadioGroup compRG;
        RadioButton compRB, nonCompRB;
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(530, 160);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        //https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/graphics-and-animation
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddMeetingLayout);
            database = MyStuff.database;
            admin = MyStuff.GetAdmin();
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
                TextSize = 35,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                LayoutParameters = WrapContParams,
            };
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


        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            currGroup = spinner.GetItemAtPosition(e.Position).ToString();
            for (int i = 0; i < GGroups.Count; i++)
            {
                if(GGroups[i].Location + " " + GGroups[i].time + " " + GGroups[i].age == currGroup)
                {
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
        public void GetSpecificGroup(Group group)
        {
            groups = new List<string>();
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
                            GCurrentGroup = new Group(age, lvl, comp, loc, date, tim);
                            GGroups.Add(GCurrentGroup);
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
            for(int i = 0; i<GGroups.Count; i++)
            {
                groups.Add(GGroups[i].Location + " " + GGroups[i].time + " " + GGroups[i].age);
            }
        }
    }
}