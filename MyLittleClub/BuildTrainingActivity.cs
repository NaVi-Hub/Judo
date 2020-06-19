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
using Android.Text;
using Javax.Xml.Datatype;
using Android.Graphics.Drawables;
using System.Linq;
using Android.Support.Design.Widget;

namespace MyLittleClub
{
    [Activity(Label = "BuildExerciseActivity")]
    public class BuildTrainingActivity : Activity
    {
        //https://pumpingco.de/blog/adding-drag-and-drop-to-your-android-application-with-xamarin/ 
        #region Parameters
        FirebaseFirestore database;
        Admin1 admin;
        MyButton[] buttons;
        bool InView = false;
        MyButton CurrMyButton;
        MyButton Send;
        Dialog d1;
        LinearLayout DialogOverLayout, DialogNameLayout, DialogDurationExplenation;
        EditText DialogExplenationTextView, ExDialogTitle;
        MyButton DialogRemoveMyButton;
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        Exercise ab1;
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(350, 200);
        LinearLayout.LayoutParams LLLP = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(650, 800);
        //
        ViewGroup.LayoutParams vlp = new ViewGroup.LayoutParams(350, 200);
        ViewGroup.LayoutParams vlllp = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        ViewGroup.LayoutParams vOneTwentyParams = new ViewGroup.LayoutParams(650, 400);
        ViewGroup.LayoutParams vWrap = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
        //
        LinearLayout Overalllayout, InsideMyButtonsSVL, InsideTrainingSVL, ScrollViewsLayout;
        ScrollView BtnSV, TrainingSV;
        TextInputEditText DurationDialogET;
        TextView OAdurationTV;
        Spinner spin; 
        Dialog DurationDialog;
        List<string> groups;
        string groupname;
        string currGroup = "Choose Group";
        private int OAduration = 0;
        ISharedPreferences sp;
        List<Exercise> exes;
        List<Exercise> selectedExercises = new List<Exercise>();
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Times = new List<int>();
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.BuildGroupLayout);
            database = Context.database;
            admin = MyStuff.GetAdmin();
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
            OAdurationTV = new TextView(this);
            OAdurationTV.LayoutParameters = BLP;
            OAdurationTV.Text = "";
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
            InsideMyButtonsSVL = new LinearLayout(this);
            InsideMyButtonsSVL.LayoutParameters = LLLP;
            InsideMyButtonsSVL.Orientation = Orientation.Vertical;
            //
            buttons = new MyButton[exes.Count];
            //
            TrainingSV = new ScrollView(this);
            TrainingSV.LayoutParameters = LLLP;
            InsideTrainingSVL = new LinearLayout(this);
            InsideTrainingSVL.LayoutParameters = LLLP;
            InsideTrainingSVL.Orientation = Orientation.Vertical;

            TrainingSV.Drag += MyButton_Drag;
            //
            for (int i = 0; i<buttons.Length; i++)
            {
                buttons[i] = new MyButton(this, i);
                buttons[i].LayoutParameters = BLP;
                buttons[i].Text = exes[i].name;
                buttons[i].LongClick += this.BuildExerciseActivity_LongClick;
                buttons[i].Click += this.BuildTrainingActivity_Click;
                InsideMyButtonsSVL.AddView(buttons[i]);
            }
            //
            BtnSV.AddView(InsideMyButtonsSVL);
            ScrollViewsLayout.AddView(BtnSV);
            TrainingSV.AddView(InsideTrainingSVL);
            ScrollViewsLayout.AddView(TrainingSV);
            //
            Send = new MyButton(this);
            Send.LayoutParameters = BLP;
            Send.Text = "Finish Training";
            Send.Click += this.Send_Click;
            Overalllayout.AddView(ScrollViewsLayout);
            Overalllayout.AddView(Send);

        }
        //Build the Screen
        private void spin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            currGroup = spin.GetItemAtPosition(e.Position).ToString();
        }
        //Spinner item changed
        private void Send_Click(object sender, System.EventArgs e)
        {
            if (InputLegit())
            {
                if (OAdurationTV.Text != "")
                {
                    if (int.Parse(OAdurationTV.Text) > 50 || int.Parse(OAdurationTV.Text) < 40)
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Alert!");
                        alert.SetMessage("Are you sure you want to add a group\nwith time setting difrent from the recommended");
                        alert.SetCancelable(false);
                        alert.SetIcon(Resource.Drawable.ShameLogo);
                        alert.SetPositiveButton("YES", (senderAlert, args) =>
                        {
                            HashMap map = new HashMap();
                            Training training = new Training(selectedExercises);
                            for (int i = 0; i < selectedExercises.Count; i++)
                            {
                                map.Put("Ex" + i, selectedExercises[i].name);
                            }
                            DocumentReference doref = database.Collection("Users").Document(admin.email).Collection("Trainings").Document(currGroup);
                            doref.Set(map);
                            Intent inte = new Intent(this, typeof(MainPageActivity));
                            inte.PutExtra("Email", admin.email);
                            StartActivity(inte);
                            Toasty.Config.Instance
                               .TintIcon(true)
                               .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                            Toasty.Success(this, "Training Built Successfuly", 10, true).Show();
                        });

                        alert.SetNegativeButton("NO", (senderAlert, args) =>
                        {
                            Dialog d = alert.Create();
                            d.Dismiss();
                        });

                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        HashMap map = new HashMap();
                        Training training = new Training(selectedExercises);
                        for (int i = 0; i < groups.Count -1 ; i++)
                        {
                            if (currGroup == Ggroups[i].Location + " " + Ggroups[i].time + " " + Ggroups[i].age)
                            {
                                Ggroups[i].CurrentTraining = training;
                            }
                        }
                        for (int i = 0; i < selectedExercises.Count; i++)
                        {
                            map.Put("Ex" + i, selectedExercises[i].name);
                        }
                        DocumentReference doref = database.Collection("Users").Document(admin.email).Collection("Trainings").Document(currGroup);
                        doref.Set(map);
                        Intent inte = new Intent(this, typeof(MainPageActivity));
                        inte.PutExtra("Email", admin.email);
                        StartActivity(inte);
                        Toasty.Config.Instance
                           .TintIcon(true)
                           .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                        Toasty.Success(this, "Training Built Successfuly", 10, true).Show();
                    }
                }
            }
            else
            {
                
            }
        }
        //Final Send MyButton
        private bool InputLegit()
        {
            bool tr = false;
            if (currGroup != "Choose Group")
            {
                tr = true;
            }
            else
            {
                Toasty.Error(this, "Choose Group", 5, true).Show();
                tr = false;
            }
            return tr;
        }
        //Input Validation
        public void MyButton_Drag(object sender, View.DragEventArgs e)
        {
            MyButton a = CurrMyButton;
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
                        MyButton copy = new MyButton(this);
                        copy.LayoutParameters = a.LayoutParameters;
                        copy.Text = a.Text;
                        copy.Time = a.Time;
                        copy.Click += this.Copy_Click;
                        InsideTrainingSVL.AddView(copy);
                        e.Handled = true;
                        var data = e.Event.ClipData;
                        if (data != null)
                            a.Text = data.GetItemAt(0).Text;
                        for (int i = 0; i < exes.Count; i++)
                        {
                            if (exes[i].name.Equals(a.Text))
                            {
                                selectedExercises.Add(exes[i]);
                            }
                        }
                        BuildDialog();
                    }
                    else
                    {
                        InsideMyButtonsSVL.RemoveView(a);
                        InsideTrainingSVL.AddView(a);
                    }
                    break;
            }
        }
        //Drag Events
        public void BuildDialog()
        {
            //Dialog
            DurationDialog = new Dialog(this);
            DurationDialog.SetCancelable(false);
            DurationDialog.SetContentView(Resource.Layout.MyDialog);
            LinearLayout DurationDialogLayout = DurationDialog.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            DurationDialogLayout.Orientation = Orientation.Vertical;
            DurationDialogLayout.SetGravity(GravityFlags.Center);
            //Text view
            TextView DurationDialogTV = new TextView(this);
            DurationDialogTV.LayoutParameters = vlp;
            DurationDialogTV.Text = "Duration: ";
            DurationDialogTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            DurationDialogTV.TextSize = 30;
            //Edit text
            DurationDialogET = new TextInputEditText(this);
            DurationDialogET.SetBackgroundResource(Resource.Drawable.MyBackground);
            DurationDialogET.Hint = "Duration";
            DurationDialogET.LayoutParameters = vlp;
            DurationDialogET.TextSize = 30;
            DurationDialogET.InputType = InputTypes.ClassPhone;
            //Input Layout
            LinearLayout DialogInputLayout = new LinearLayout(this);
            DialogInputLayout.LayoutParameters = vOneTwentyParams;
            DialogInputLayout.Orientation = Orientation.Horizontal;
            //addind to layout
            DialogInputLayout.AddView(DurationDialogTV);
            DialogInputLayout.AddView(DurationDialogET);
            //Linear Layout
            DurationDialogLayout.AddView(DialogInputLayout);
            //button
            MyButton DialogMyButton = new MyButton(this);
            DialogMyButton.LayoutParameters = vlp;
            DialogMyButton.Text = "Add";
            DialogMyButton.TextSize = 30;
            DialogMyButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            DialogMyButton.Click += this.DialogMyButton_Click;
            DurationDialogLayout.AddView(DialogMyButton);
            DurationDialog.Show();
            MyStuff.showSoftKeyboard(this, DurationDialogET);
        }
        //Build Duration Dialog
        private void DialogMyButton_Click(object sender, EventArgs e)
        {
            int a;
            int.TryParse(DurationDialogET.Text, out a);
            OAduration += a;
            OAdurationTV.Text = OAduration.ToString();
            if (OAduration <= 50 && OAduration >= 40)
            {
                OAdurationTV.SetTextColor(Color.LawnGreen);
            }
            else if (OAduration > 50)
            {
                OAdurationTV.SetTextColor(Color.Red);
            }
            else
            {
                OAdurationTV.SetTextColor(Color.Black);
            }
            CurrMyButton.Time = a;
            Times.Add(int.Parse(DurationDialogET.Text));
            DurationDialog.Dismiss();
        }
        bool h = false;
        //Dialog MyButton Click
        public void BuildDialog(object sender, int f)
        {
            MyButton b = (MyButton)sender;
            for (int i = 0; i<exes.Count; i++)
            {
                if (exes[i].name.Equals(b.Text))
                {
                    ab1 = exes[i];
                }
            }
            d1 = new Dialog(this);
            //if (exes.Contains)
            d1.SetCancelable(true);
            d1.SetTitle(ab1.name);
            d1.SetContentView(Resource.Layout.MyDialog);
            LinearLayout ll = d1.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            ll.Orientation = Orientation.Vertical;
            //
            LinearLayout DialogLayout = new LinearLayout(this);
            DialogLayout.LayoutParameters = vWrap;
            DialogLayout.Orientation = Orientation.Vertical;
            DialogLayout.SetGravity(GravityFlags.CenterVertical);
            //Dialog TitleTV
            ExDialogTitle = new EditText(this);
            ExDialogTitle.SetBackgroundColor(Color.RoyalBlue);
            ExDialogTitle.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            ExDialogTitle.SetTextColor(Color.DarkRed);
            ExDialogTitle.TextSize = 40; 
            ExDialogTitle.Text = ab1.name;
            DialogLayout.AddView(ExDialogTitle);
            //
            DialogExplenationTextView = new EditText(this);
            DialogExplenationTextView.Text = ab1.explenatiotn.ToString();
            DialogExplenationTextView.TextSize = 30;
            DialogExplenationTextView.SetTextColor(Color.Black);
            //
            if (f == 1)
            {
                if (!h)
                {
                    for (int i = 0; i < exes.Count; i++)
                    {
                        if (buttons[i].Text == b.Text)
                        {
                            b.Time = buttons[i].Time;

                            h = true;
                        }
                    }
                }
                dur = new EditText(this)
                {
                    Text = b.Time.ToString(),
                    LayoutParameters = OneTwentyParams,
                    Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                    TextSize = 35,
                };
                dur.SetBackgroundResource(Resource.Drawable.MyBackground);
                dur.InputType = InputTypes.ClassPhone;
                //
            }
            Save = new MyButton(this)
            {
                Text = "Save",
                LayoutParameters = OneTwentyParams,
                TextSize = 35,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            bool a = false;
            Save.Click += delegate
                {
                    OAduration -= b.Time;
                    b.Time = int.Parse(dur.Text);
                    OAduration += b.Time;
                    OAdurationTV.Text = OAduration.ToString();
                    a = true;
                    if (OAduration <= 50 && OAduration >= 40)
                    {
                        OAdurationTV.SetTextColor(Color.LawnGreen);
                    }
                    else if (OAduration > 50)
                    {
                        OAdurationTV.SetTextColor(Color.Red);
                    }
                    else
                    {
                        OAdurationTV.SetTextColor(Color.Black);
                    }
                    if (ExDialogTitle.Text != ab1.name)
                    {
                        ab1.name = ExDialogTitle.Text;
                        a = true;
                        Toasty.Info(this, "Happened2", 2, false).Show();
                    }
                    else if (DialogExplenationTextView.Text != ab1.explenatiotn)
                    {
                        ab1.explenatiotn = DialogExplenationTextView.Text;
                        a = true;
                        Toasty.Info(this, "Happened3", 2, false).Show();
                    }
                    if (a == true)
                    {
                        HashMap map = new HashMap();
                        map.Put("Explenation", ab1.explenatiotn);
                        map.Put("Name", ab1.name);
                        DocumentReference DocRef = database.Collection("Users").Document(admin.email).Collection("Exercises").Document(ab1.name);
                        DocRef.Set(map);
                    }
                };
            ////Add BitMap
            //
            DialogLayout.AddView(DialogExplenationTextView);
            if (f == 1)
            {
                DialogLayout.AddView(dur);
            }
            DialogLayout.AddView(Save);
            ll.AddView(DialogLayout);
            d1.Show();
        }
        //Builds Ex Dialog
        EditText dur;
        MyButton Save;
        List<int> Times;
        private void Copy_Click(object sender, EventArgs e)
        {
            BuildDialog(sender, 1);
        }
        //Calls BuildDialog(object sender)
        private void BuildTrainingActivity_Click(object sender, EventArgs e)
        {
            BuildDialog(sender, 0);
        }
        //Calls BuildDialog(object sender)
        private void BuildExerciseActivity_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            CurrMyButton = (MyButton)sender;
            var data = ClipData.NewPlainText("name", CurrMyButton.Text);
            CurrMyButton.StartDrag(data, new View.DragShadowBuilder(CurrMyButton), null, 0);
        }
        //Start Drag
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
                OrganizeList(exes);
                GetGroups();
            }
            ));
        }
        //Retreiving Exercies From FireBase
        List<Group> Ggroups;
        public List<Group> GetGroups()
        {
            Ggroups = new List<Group>();
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
                            bool comp = bool.Parse(item.GetBoolean("Comp").ToString());
                            string lvl = item.GetString("Level");
                            Group g = new Group(age, lvl, comp, loc);
                            Ggroups.Add(g);
                        }
                    }
                }
                MakeStringList();
            }));
            return Ggroups;
        }
        //Retreiving Groups From FireBase
        private void MakeStringList()
        {
            groups = new List<string>();
            groups.Add("Choose Group");
            for (int i = 0; i < Ggroups.Count; i++)
            {
                groups.Add(Ggroups[i].Location + " " + Ggroups[i].time + " " + Ggroups[i].age);
            }
            BuildAddExScreen();
        }
        public void OrganizeList(List<Exercise> list)
        {
            list.OrderBy(w => w.name);
        }
    }
}