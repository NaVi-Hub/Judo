using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Java.Lang;

namespace MyLittleClub
{
    [Activity(Label = "TimerActivity")]
    public class TimerActivity : Activity
    {
        LinearLayout OverAllTimerLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TimerLayout);
            OverAllTimerLayout = FindViewById<LinearLayout>(Resource.Id.TimerLLayout);
            OverAllTimerLayout.SetGravity(GravityFlags.CenterHorizontal);
            times = new List<string>();
            for (int i = 1; i <= 50; i++)
            {
                times.Add(i + "");
            }
            BuildFirstSpiner();
            // Create your application here
        }
        Spinner FSpin;
        List<string> times;
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(650, 200);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        private void BuildFirstSpiner()
        {
            FSpin = new Spinner(this);
            BLP.SetMargins(5, 5, 5, 5);
            FSpin.LayoutParameters = BLP;
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, times);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            FSpin.Adapter = adapter;
            FSpin.ItemSelected += this.FSpin_ItemSelected;
            FSpin.Selected = false;
            OverAllTimerLayout.AddView(FSpin);
        }
        int t = 0;
        private void FSpin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            t = int.Parse(spin.GetItemAtPosition(e.Position).ToString());
            BuildallEdittexts(t);
        }
        EditText[] Edittexts;
        LinearLayout EdittextsLayout;
        ScrollView DaysSV;
        LinearLayout.LayoutParams LLLP = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
        private void BuildallEdittexts(int t)
        {
            try
            {
                DaysSV.RemoveAllViews();
                EdittextsLayout.RemoveAllViews();
                OverAllTimerLayout.RemoveView(DaysSV);
            }
            catch
            {
                Toasty.Normal(this, "First Inital", 5).Show();
                EdittextsLayout = new LinearLayout(this);
                EdittextsLayout.LayoutParameters = WrapContParams;
                EdittextsLayout.Orientation = Orientation.Vertical;
                EdittextsLayout.SetGravity(GravityFlags.Center);
                DaysSV = new ScrollView(this);
                DaysSV.LayoutParameters = LLLP;
            }
            Edittexts = new EditText[50];
            ////////////////////////////////
            for (int i = 0; i < t; i++)
            {
                Edittexts[i] = new EditText(this);
                BLP.SetMargins(5, 5, 5, 5);
                Edittexts[i].LayoutParameters = BLP;
                Edittexts[i].Selected = false;
                Edittexts[i].Hint = "Seconds";
                Edittexts[i].InputType = InputTypes.DatetimeVariationTime;
                EdittextsLayout.AddView(Edittexts[i]);
            }
            DaysSV.AddView(EdittextsLayout);
            OverAllTimerLayout.AddView(DaysSV);
            Start = new Button(this);
            Start.LayoutParameters = BLP;
            Start.Text = "START!";
            Start.Click += this.Start_Click;
        }
        Timer timer;
        private void Start_Click(object sender, EventArgs e)
        {
            timer = new Timer();
            for (int i = 0; i < Edittexts.Length; i++)
            {
                timer.Change(int.Parse(Edittexts[i].Text), 1);
            }
        }
        Button Start;
        private void Edittexts_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)//
        {
            Spinner spin = (Spinner)sender;
            Toasty.Info(this, "" + spin.SelectedItem, 3, true).Show();
        }
    }
}