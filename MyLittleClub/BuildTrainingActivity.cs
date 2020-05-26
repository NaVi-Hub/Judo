﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Firebase.Firestore;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using Newtonsoft.Json;
using System;
using Java.Util;
using ES.DMoral.ToastyLib;
using Android.Text;
using Javax.Xml.Datatype;

namespace MyLittleClub
{
    [Activity(Label = "BuildExerciseActivity")]
    public class BuildTrainingActivity : Activity
    {
        //https://pumpingco.de/blog/adding-drag-and-drop-to-your-android-application-with-xamarin/ 
        FirebaseFirestore database;
        Admin1 admin;
        Button[] buttons;
        bool InView = false;
        Button CurrButton;
        Button Send;
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(350, 200);
        LinearLayout.LayoutParams LLLP = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(650, 800);
        //
        ViewGroup.LayoutParams vlp = new ViewGroup.LayoutParams(350, 200);
        ViewGroup.LayoutParams vlllp = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        ViewGroup.LayoutParams vOneTwentyParams = new ViewGroup.LayoutParams(650, 800);
        LinearLayout Overalllayout, InsideButtonsSVL, InsideTrainingSVL, ScrollViewsLayout;
        ScrollView BtnSV, TrainingSV;
        EditText DurationDialogET;
        Spinner spin;
        List<string> groups;
        string groupname;
        string currGroup = "Choose Group";
        ISharedPreferences sp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.BuildGroupLayout);
            database = Context.database;
            admin = MyStuff.GetAdmin();
            GetExercises();
            // Create your application here
        }
        TextView OAdurationTV;
        public void BuildAddExScreen()
        {
            Overalllayout = (LinearLayout)FindViewById(Resource.Id.AddGroupL);
            Overalllayout.Orientation = Orientation.Vertical;
            Overalllayout.SetGravity(GravityFlags.CenterHorizontal);
            //
            spin = new Spinner(this);
            BLP.SetMargins(5, 5, 5, 5);
            spin.LayoutParameters = BLP;
            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, groups);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = adapter;
            spin.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spin_ItemSelected);
            Overalllayout.AddView(spin);
            //
            OAdurationTV = new TextView(this);
            OAdurationTV.LayoutParameters = BLP;
            OAdurationTV.Text = OAduration.ToString();
            OAdurationTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            OAdurationTV.TextSize = 30;
            Overalllayout.AddView(OAdurationTV);
            //
            ScrollViewsLayout = new LinearLayout(this);
            ScrollViewsLayout.LayoutParameters = LLLP;
            ScrollViewsLayout.Orientation = Orientation.Horizontal;
            //
            BtnSV = new ScrollView(this);
            BtnSV.LayoutParameters = LLLP;
            InsideButtonsSVL = new LinearLayout(this);
            InsideButtonsSVL.LayoutParameters = LLLP;
            InsideButtonsSVL.Orientation = Orientation.Vertical;
            //
            buttons = new Button[exes.Count];
            //
            TrainingSV = new ScrollView(this);
            TrainingSV.LayoutParameters = LLLP;
            InsideTrainingSVL = new LinearLayout(this);
            InsideTrainingSVL.LayoutParameters = LLLP;
            InsideTrainingSVL.Orientation = Orientation.Vertical;

            TrainingSV.Drag += Button_Drag;
            //
            for (int i = 0; i<buttons.Length; i++)
            {
                buttons[i] = new Button(this);
                buttons[i].LayoutParameters = BLP;
                buttons[i].Text = exes[i].name;
                buttons[i].LongClick += this.BuildExerciseActivity_LongClick;
                buttons[i].Click += this.BuildTrainingActivity_Click;
                InsideButtonsSVL.AddView(buttons[i]);
            }
            //
            BtnSV.AddView(InsideButtonsSVL);
            ScrollViewsLayout.AddView(BtnSV);
            TrainingSV.AddView(InsideTrainingSVL);
            ScrollViewsLayout.AddView(TrainingSV);
            //
            Send = new Button(this);
            Send.LayoutParameters = BLP;
            Send.Text = "Finish Exercise";
            Send.Click += this.Send_Click;
            Overalllayout.AddView(ScrollViewsLayout);
            Overalllayout.AddView(Send);

        }
        private void spin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            currGroup = spin.GetItemAtPosition(e.Position).ToString();
        }
        private void Send_Click(object sender, System.EventArgs e)
        {
            if (InputLegit())
            {
                HashMap map = new HashMap();
                Training training = new Training(ex);
                for (int i = 0; i < ex.Count; i++)
                {
                    map.Put("Ex" + i, ex[i].name);
                }
                DocumentReference doref = database.Collection("Trainings").Document(currGroup);
                doref.Set(map);
                Intent inte = new Intent(this, typeof(MainPageActivity));
                inte.PutExtra("Email", admin.email);
                StartActivity(inte);
                Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                Toasty.Success(this, "Training Built Successfuly", 10, true).Show();
            }
            else
            {
            }
        }
        private bool InputLegit()
        {
            return currGroup != "Choose Group";
        }
        Dialog DurationDialog;
        public void Button_Drag(object sender, View.DragEventArgs e)
        {
            Button a = CurrButton;
            var evt = e.Event;
            switch(evt.Action)
            {
                case DragAction.Ended:
                case DragAction.Started:
                    e.Handled = true;
                    break;
                case DragAction.Entered:
                    InView = true;
                    TrainingSV.SetBackgroundColor(Color.LawnGreen);
                    InsideTrainingSVL.SetBackgroundColor(Color.LawnGreen);
                    break;
                case DragAction.Exited:
                    InView = false;
                    TrainingSV.SetBackgroundColor(Color.Transparent);
                    InsideTrainingSVL.SetBackgroundColor(Color.Transparent);
                    break;
                case DragAction.Drop:
                    if(InView)
                    {
                        TrainingSV.SetBackgroundColor(Color.Transparent);
                        InsideTrainingSVL.SetBackgroundColor(Color.Transparent);
                        Button copy = new Button(CurrButton.Context);
                        copy.LayoutParameters = CurrButton.LayoutParameters;
                        copy.Text = CurrButton.Text;
                        copy.Click += this.Copy_Click;
                        InsideTrainingSVL.AddView(copy);
                        e.Handled = true;
                        var data = e.Event.ClipData;
                        if (data != null)
                            a.Text = data.GetItemAt(0).Text;
                        BuildDialog();
                    }
                    else
                    {
                        InsideButtonsSVL.RemoveView(a);
                        InsideTrainingSVL.AddView(a);
                    }
                    break;
            }
        }
        public void BuildDialog()
        {
            //Dialog
            DurationDialog = new Dialog(this);
            DurationDialog.SetCancelable(true);
            DurationDialog.SetContentView(Resource.Layout.MyDialog);
            //Linear Layout
            LinearLayout DurationDialogLayout = DurationDialog.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            //Text view
            TextView DurationDialogTV = new TextView(this);
            BLP.SetMargins(20, 20, 10, 10);
            DurationDialogTV.LayoutParameters = vlp;
            DurationDialogTV.Text = "Duration: ";
            DurationDialogTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            DurationDialogTV.TextSize = 30;
            //Edit text
            DurationDialogET = new EditText(this);
            DurationDialogET.Hint = "Duration";
            BLP.SetMargins(10, 20, 20, 10);
            DurationDialogET.LayoutParameters = vlp;
            DurationDialogET.TextSize = 30;
            DurationDialogET.InputType = InputTypes.NumberVariationNormal;
            //Input Layout
            LinearLayout DialogInputLayout = new LinearLayout(this);
            DurationDialogLayout.LayoutParameters = vlllp;
            DurationDialogLayout.Orientation = Orientation.Horizontal;
            //addind to layout
            DialogInputLayout.AddView(DurationDialogTV);
            DialogInputLayout.AddView(DurationDialogET);
            DurationDialogLayout.AddView(DialogInputLayout);
            //button
            Button DialogButton = new Button(this);
            DialogButton.LayoutParameters = vlp;
            DialogButton.Text = "Add";
            DialogButton.TextSize = 30;
            DialogButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            DialogButton.Click += this.DialogButton_Click;
            DurationDialogLayout.AddView(DialogButton);
            DurationDialog.Show();
        }
        private void DialogButton_Click(object sender, EventArgs e)
        {
            int a;
            int.TryParse(DurationDialogET.Text, out a);
            OAduration += a;
            DurationDialog.Dismiss();
        }
        public int OAduration = 0;
        Dialog d1;
        LinearLayout DialogOverLayout, DialogNameLayout, DialogDurationExplenation;
        TextView DialogNameTextView, DialogExplenationTextView;
        Button DialogRemoveButton;
        Exercise ab1;
        public void BuildDialog(object sender)
        {
            Button b = (Button)sender;
            d1 = new Dialog(this);
            //if (exes.Contains)
            d1.SetCancelable(true);
            d1.SetTitle(ab1.name);
            d1.SetContentView(Resource.Layout.MyDialog);
            LinearLayout ll = d1.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            DialogOverLayout.Orientation = Orientation.Vertical;
            //
            DialogNameTextView = new TextView(this);
            DialogNameTextView.Text = ab1.name;
            DialogExplenationTextView = new TextView(this);
            DialogExplenationTextView.Text = ab1.explenatiotn.ToString();
            d1.Show();
        }
        private void Copy_Click(object sender, EventArgs e)
        {
            BuildDialog(sender);
        }
        private void BuildTrainingActivity_Click(object sender, EventArgs e)
        {
            BuildDialog(sender);
        }
        private void BuildExerciseActivity_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            CurrButton = (Button)sender;
            var data = ClipData.NewPlainText("name", CurrButton.Text);
            CurrButton.StartDrag(data, new View.DragShadowBuilder(CurrButton), null, 0);
        }
        List<Exercise> exes;
        public void GetExercises()
        {
            exes = new List<Exercise>();
            Query query = database.Collection("Users").Document(admin.email).Collection("Exercises");
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
                            string name = (item.GetString("Name")).ToString();
                            string exp = (item.GetString("Explenation")).ToString();
                            exes.Add(new Exercise(name, exp));
                        }
                    }
                }
                GetGroups();
            }
            ));
        }
        List<Exercise> ex = new List<Exercise>();
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
                BuildAddExScreen();
            }));
            return groups;
        }
        //Retreiving Groups From FireBase
    }
}