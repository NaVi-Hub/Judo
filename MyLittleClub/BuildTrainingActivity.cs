
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Firebase.Firestore;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using Java.Util;
using Newtonsoft.Json;

namespace MyLittleClub
{
    [Activity(Label = "BuildExerciseActivity")]
    public class BuildTrainingActivity : Activity
    {
        //https://pumpingco.de/blog/adding-drag-and-drop-to-your-android-application-with-xamarin/ 
        FirebaseFirestore database;
        Admin1 admin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.BuildGroupLayout);
            database = OpenActivity.database;
            admin = MainPageActivity.admin1;
            GetExercises();
            // Create your application here
        }
        Button[] buttons;
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(350, 200);
        LinearLayout.LayoutParams LLLP = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
        LinearLayout Overalllayout, InsideButtonsSVL, InsideTrainingSVL;
        ScrollView BtnSV, TrainingSV;
        public void BuildAddExScreen()
        {
            Overalllayout = (LinearLayout)FindViewById(Resource.Id.AddGroupL);
            Overalllayout.Orientation = Orientation.Horizontal;
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
            InsideTrainingSVL.SetBackgroundColor(Color.DodgerBlue);
            TrainingSV.SetBackgroundColor(Color.DodgerBlue);
            TrainingSV.AddView(InsideTrainingSVL);

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
            Overalllayout.AddView(BtnSV);
            Overalllayout.AddView(TrainingSV);
            //
            Send = new Button(this);
            Send.LayoutParameters = BLP;
            Send.Text = "Finish Exercise";
            Send.Click += this.Send_Click;
            Overalllayout.AddView(Send);
        }

        private void Send_Click(object sender, System.EventArgs e)
        {
            HashMap map = new HashMap();
            Training training = new Training(ex);
            for(int i = 0; i<ex.Count; i++)
            {
                map.Put("Ex" + i, ex[i].name);
            }
            DocumentReference doref = database.Collection("Trainings").Document("Training"/*insert Edittext and insert here the title of training*/);
            doref.Set(map);
            Intent inte = new Intent(this, typeof(MainPageActivity));
            inte.PutExtra("Admin", JsonConvert.SerializeObject(admin));
            StartActivity(inte);
        }

        bool InView = false;
        Button CurrButton;
        Exercise[] training;
        Button Send;
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
                        InsideButtonsSVL.RemoveView(a);
                        TrainingSV.SetBackgroundColor(Color.DodgerBlue);
                        InsideTrainingSVL.SetBackgroundColor(Color.DodgerBlue);
                        InsideTrainingSVL.AddView(a);
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
                BuildAddExScreen();
            }
            ));
        }
        List<Exercise> ex = new List<Exercise>();
        //Number Of Exercises So Far
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
    }
}