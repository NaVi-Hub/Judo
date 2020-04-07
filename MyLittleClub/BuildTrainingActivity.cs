
using Android.App;
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
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(530, 160);
        LinearLayout Overalllayout, InsideButtonsSVL, InsideTrainingSVL, ScrollViewsLayout;
        ScrollView BtnSV, TrainingSV;
        Spinner spin;
        List<string> groups;
        string groupname;
        string currGroup = "Choose Group";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.BuildGroupLayout);
            database = OpenActivity.database;
            admin = MainPageActivity.admin1;
            GetExercises();
            // Create your application here
        }
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
            ScrollViewsLayout = new LinearLayout(this);
            ScrollViewsLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
            ScrollViewsLayout.Orientation = Orientation.Horizontal;
            //
            BtnSV = new ScrollView(this);
            BtnSV.LayoutParameters = LLLP;
            InsideButtonsSVL = new LinearLayout(this);
            InsideButtonsSVL.LayoutParameters = LLLP;
            InsideButtonsSVL.SetBackgroundColor(Color.GreenYellow);
            BtnSV.SetBackgroundColor(Color.DarkSeaGreen);
            InsideButtonsSVL.Orientation = Orientation.Vertical;
            //
            buttons = new Button[exes.Count];
            //
            TrainingSV = new ScrollView(this);
            TrainingSV.LayoutParameters = LLLP;
            InsideTrainingSVL = new LinearLayout(this);
            InsideTrainingSVL.LayoutParameters = LLLP;
            InsideTrainingSVL.Orientation = Orientation.Vertical;
            InsideTrainingSVL.SetBackgroundColor(Color.DodgerBlue);
            TrainingSV.SetBackgroundColor(Color.DodgerBlue);

            TrainingSV.Drag += Button_Drag;
            //
            for (int i = 0; i<buttons.Length; i++)
            {
                buttons[i] = new Button(this);
                buttons[i].LayoutParameters = BLP;
                buttons[i].Text = exes[i].name;
                buttons[i].LongClick += this.BuildExerciseActivity_LongClick;
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
                inte.PutExtra("Admin", JsonConvert.SerializeObject(admin));
                StartActivity(inte);
                Toasty.Success(this, "Training Built Successfuly", 10, false).Show();
            }
            else
            {
                Toasty.Config.Instance
                .TintIcon(true) // optional (apply textColor also to the icon)
                .SetTextSize(20) // optional
                .Apply(); // required
            }
        }

        private bool InputLegit()
        {
            return currGroup != "Choose Group";
        }

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
                    TrainingSV.SetBackgroundColor(Color.LawnGreen);
                    InsideTrainingSVL.SetBackgroundColor(Color.LawnGreen);
                    InView = true;
                    break;
                case DragAction.Exited:
                    TrainingSV.SetBackgroundColor(Color.Red);
                    InsideTrainingSVL.SetBackgroundColor(Color.Red);
                    InView = false;
                    break;
                case DragAction.Drop:
                    if(InView)
                    {
                        Button copy = new Button(CurrButton.Context);
                        copy.LayoutParameters = CurrButton.LayoutParameters;
                        copy.Text = CurrButton.Text;
                        copy.Click += this.Copy_Click;
                        TrainingSV.SetBackgroundColor(Color.DodgerBlue);
                        InsideTrainingSVL.SetBackgroundColor(Color.DodgerBlue);
                        InsideTrainingSVL.AddView(copy);
                        e.Handled = true;
                        var data = e.Event.ClipData;
                        if (data != null)
                            a.Text = data.GetItemAt(0).Text;
                        GetSpecificExercise(a.Text);
                    }
                    else
                    {
                        InsideButtonsSVL.RemoveView(a);
                        InsideTrainingSVL.AddView(a);
                    }
                    break;
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {

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
            Query query = database.Collection("Exercises");
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
                            double duration = double.Parse(item.GetDouble("Duration").ToString());
                            string exp = (item.GetString("Explenation")).ToString();
                            exes.Add(new Exercise(name, duration, exp));
                        }
                    }
                }
                GetGroups();
            }
            ));
        }
        List<Exercise> ex = new List<Exercise>();
        public void GetSpecificExercise(string name)
        {
            Query query = database.Collection("Exercises").WhereEqualTo("Name", name);
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
                            string name1 = (item.GetString("Name")).ToString();
                            double duration = double.Parse(item.GetDouble("Duration").ToString());
                            string exp = (item.GetString("Explenation")).ToString();
                            ex.Add(new Exercise(name1, duration, exp));
                        }
                    }
                }
            }
            ));
        }

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