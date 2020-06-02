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
namespace MyLittleClub
{
    [Activity(Theme = "@style/AppTheme")]
    public class AddExerciseActivity : Activity
    {
        LinearLayout AddTrainingOverAllLayout, AddTrainingLabelLayout, AddTrainingNameLayout, ButtonAddTrainingLayout, AddTrainingExplenationLayout, AddTrainingExplenationETLayout;
        TextView AddTrainingLabelTV, AddTrainingNameTV, AddTrainingExplenationTV;
        Button AddTrainingButton;
        EditText AddTrainingNameET, AddTrainingExplenationET;
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(420, 180);
        Admin1 admin;
        FirebaseFirestore database = Context.database;
        ISharedPreferences sp;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddTrainingLayout);
            admin = MyStuff.GetAdmin();
            BuildAddTrainingScreen();
        }
        
        public void BuildAddTrainingScreen()
        {
            //OverAll Layout
            AddTrainingOverAllLayout = (LinearLayout)FindViewById(Resource.Id.AddTrainingL);
            AddTrainingOverAllLayout.Orientation = Orientation.Vertical;
            //-------------------------------------
            //Defining Label Layout
            AddTrainingLabelLayout = new LinearLayout(this);
            AddTrainingLabelLayout.LayoutParameters = WrapContParams;
            AddTrainingLabelLayout.Orientation = Orientation.Vertical;
            AddTrainingLabelLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Defining the Label AddTraining TextView
            AddTrainingLabelTV = new TextView(this);
            AddTrainingLabelTV.LayoutParameters = WrapContParams;
            AddTrainingLabelTV.Text = "New Exercise";
            AddTrainingLabelTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddTrainingLabelTV.SetTextColor(Android.Graphics.Color.DarkRed);
            AddTrainingLabelTV.TextSize = 60;
            AddTrainingLabelLayout.AddView(AddTrainingLabelTV);
            AddTrainingOverAllLayout.AddView(AddTrainingLabelLayout);
            //Defining the AddTraining Name layout
            AddTrainingNameLayout = new LinearLayout(this);
            AddTrainingNameLayout.LayoutParameters = WrapContParams;
            AddTrainingNameLayout.Orientation = Orientation.Horizontal;
            //Defining the Name AddTraining TextView
            AddTrainingNameTV = new TextView(this);
            AddTrainingNameTV.LayoutParameters = WrapContParams;
            AddTrainingNameTV.Text = "Exercise Name: ";
            AddTrainingNameTV.TextSize = 30;
            AddTrainingNameTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Name AddTraining EditText
            AddTrainingNameET = new EditText(this);
            AddTrainingNameET.LayoutParameters = OneTwentyParams;
            AddTrainingNameET.Hint = "name";
            AddTrainingNameET.TextSize = 30;
            //Adding views to layout
            AddTrainingNameLayout.AddView(AddTrainingNameTV);
            AddTrainingNameLayout.AddView(AddTrainingNameET);
            AddTrainingOverAllLayout.AddView(AddTrainingNameLayout);
            //----------------------------------------------------------------------------------
            //----------------------------------------------------------------------------------------
            //Defining the AddTraining Explenation layout
            AddTrainingExplenationLayout = new LinearLayout(this);
            AddTrainingExplenationLayout.LayoutParameters = WrapContParams;
            AddTrainingExplenationLayout.Orientation = Orientation.Vertical;
            //Defining the Explenation AddTraining TextView
            AddTrainingExplenationTV = new TextView(this);
            AddTrainingExplenationTV.LayoutParameters = WrapContParams;
            AddTrainingExplenationTV.Text = "Exercise Explenation: ";
            AddTrainingExplenationTV.TextSize = 30;
            AddTrainingExplenationTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Adding views to layout
            AddTrainingExplenationLayout.AddView(AddTrainingExplenationTV);
            AddTrainingOverAllLayout.AddView(AddTrainingExplenationLayout);
            //Defining The AddTraining Explenation ET layout
            AddTrainingExplenationETLayout = new LinearLayout(this);
            AddTrainingExplenationETLayout.LayoutParameters = new LinearLayout.LayoutParams(1100, 800);
            AddTrainingExplenationETLayout.Orientation = Orientation.Vertical;
            AddTrainingExplenationETLayout.SetBackgroundResource(Resource.Drawable.BlackOutLine);
            AddTrainingExplenationETLayout.Click += this.AddTrainingExplenationETLayout_Click;
            //Defining the Explenation AddTraining EditText
            AddTrainingExplenationET = new EditText(this);
            AddTrainingExplenationET.SetWidth(LinearLayout.LayoutParams.MatchParent);
            AddTrainingExplenationET.Hint = "Explenation";
            AddTrainingExplenationET.TextSize = 25;
            AddTrainingExplenationET.SetTextIsSelectable(true);
            AddTrainingExplenationET.InputType = InputTypes.TextFlagMultiLine;
            AddTrainingExplenationET.Gravity = GravityFlags.Top;
            AddTrainingExplenationET.SetSingleLine(false);
            AddTrainingExplenationET.SetBackgroundColor(Color.Transparent);

            //Adding viwes to overall layout
            AddTrainingExplenationETLayout.AddView(AddTrainingExplenationET);
            AddTrainingOverAllLayout.AddView(AddTrainingExplenationETLayout);
            //---------------------------------------------------------------------------------------------------------
            //Defining AddTraining Button Layout
            ButtonAddTrainingLayout = new LinearLayout(this);
            ButtonAddTrainingLayout.LayoutParameters = WrapContParams;
            ButtonAddTrainingLayout.Orientation = Orientation.Horizontal;
            //Defining AddTraining Button
            AddTrainingButton = new Button(this);
            AddTrainingButton.LayoutParameters = WrapContParams;
            AddTrainingButton.Text = "Register";
            AddTrainingButton.TextSize = 40;
            AddTrainingButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            AddTrainingButton.Click += this.AddTrainingButton_Click;
            //Adding views
            ButtonAddTrainingLayout.AddView(AddTrainingButton);
            AddTrainingOverAllLayout.AddView(ButtonAddTrainingLayout);
            //Insert Image: 
            /*
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             */
        }
        //Builds The Screen
        private void AddTrainingButton_Click(object sender, EventArgs e)
        {
            Exercise ex = new Exercise(AddTrainingNameET.Text, AddTrainingExplenationET.Text);
            HashMap map = new HashMap();
            map.Put("Name", ex.name);
            map.Put("Explenation", ex.explenatiotn);
            DocumentReference DocRef = database.Collection("Users").Document(admin.email).Collection("Exercises").Document(ex.name);
            DocRef.Set(map);
            Toasty.Config.Instance
                   .TintIcon(true)
                   .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
            Toasty.Success(this, "Exercise was added secessfully", 5, true).Show();
            Intent intent1 = new Intent(this, typeof(MainPageActivity));
            intent1.PutExtra("Email", admin.email);
            StartActivity(intent1);
        }
        //Adds the exercise to the database.
        private void AddTrainingExplenationETLayout_Click(object sender, EventArgs e)
        {
            AddTrainingExplenationET.RequestFocus();
            showSoftKeyboard(this, AddTrainingExplenationET);
            //@Tomer
            //https://gist.github.com/icalderond/742f98f2f8cda1fae1b0bc877df76bbc @Javier Pardo

        }
        //Makes a big EditText
        public void showSoftKeyboard(Activity activity, View view)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Android.Content.Context.InputMethodService);
            view.RequestFocus();
            inputMethodManager.ShowSoftInput(view, 0);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);//personal line added
        }
        //Pops up soft keyboard
    }
}