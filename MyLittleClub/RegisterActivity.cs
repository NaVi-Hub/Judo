using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Text;
using Android.Content;
using Firebase.Firestore;
using Firebase;
using Java.Util;
using System.Collections.Generic;
using Android.Gms.Tasks;
using Newtonsoft.Json;

namespace MyLittleClub
{
    [Activity(Theme = "@style/AppTheme")]
    public class RegisterActivity : Activity
    {
        Admin1 admin;
        LinearLayout OverAllLoginLayout, NameLoginLayout, MailLoginLayout, SportLoginLayout, ButtonLoginLayout, LabelLoginLayout, PhoneNumberLoginLayout, AgeLoginLayout, CBLoginLayout;
        TextView LabelLoginTV, LabelLoginTV1, NameLoginTV, MailLoginTV, SportLoginTV, PhoneNumberLoginTV, AgeLoginTV;
        EditText NameLoginET, MailLoginET, SportLoginET, PhoneNumberLoginET, AgeLoginET;
        Button LoginButton;
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(420, 180);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams MatchParentParams2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 200);
        FirebaseFirestore database;
        CheckBox LoginCB;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.RegisterLayout);
            BuildRegisterScreen();
            database = OpenActivity.database;
            admin = MainPageActivity.admin1;
        }

        void BuildRegisterScreen()
        {
            //Defining the parent layout
            OverAllLoginLayout = (LinearLayout)FindViewById(Resource.Id.LoginLinearLayout);
            OverAllLoginLayout.Orientation = Orientation.Vertical;
            OverAllLoginLayout.SetGravity(Android.Views.GravityFlags.CenterHorizontal);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the Label login Layout
            LabelLoginLayout = new LinearLayout(this);
            LabelLoginLayout.LayoutParameters = WrapContParams;
            LabelLoginLayout.Orientation = Orientation.Vertical;
            LabelLoginLayout.SetGravity(Android.Views.GravityFlags.Center);
            //Defining the Label Login TextView
            LabelLoginTV = new TextView(this);
            LabelLoginTV.LayoutParameters = WrapContParams;
            LabelLoginTV.Text = "Welcome!";
            LabelLoginTV.TextSize = 80;
            LabelLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LabelLoginTV.SetTextColor(Android.Graphics.Color.DarkRed);
            LabelLoginLayout.AddView(LabelLoginTV);
            //Defining the Second Label Login Textview
            LabelLoginTV1 = new TextView(this);
            LabelLoginTV1.LayoutParameters = WrapContParams;
            LabelLoginTV1.Text = "Register";
            LabelLoginTV1.TextSize = 40;
            LabelLoginTV1.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LabelLoginTV1.SetTextColor(Android.Graphics.Color.DarkRed);
            LabelLoginLayout.AddView(LabelLoginTV1);
            OverAllLoginLayout.AddView(LabelLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining the NameLogin layout
            NameLoginLayout = new LinearLayout(this);
            NameLoginLayout.LayoutParameters = WrapContParams;
            NameLoginLayout.Orientation = Orientation.Horizontal;
            //Defining the Name Login TextView
            NameLoginTV = new TextView(this);
            NameLoginTV.LayoutParameters = WrapContParams;
            NameLoginTV.Text = "Name: ";
            NameLoginTV.TextSize = 30;
            NameLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Name Login EditText
            NameLoginET = new EditText(this);
            NameLoginET.LayoutParameters = OneTwentyParams;
            NameLoginET.Hint = "Enter Name";
            NameLoginET.TextSize = 30;
            NameLoginET.InputType = InputTypes.TextVariationPersonName;
            NameLoginET.SetSingleLine();
            //Adding views to layout
            NameLoginLayout.AddView(NameLoginTV);
            NameLoginLayout.AddView(NameLoginET);
            OverAllLoginLayout.AddView(NameLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining MailLoginLayout
            MailLoginLayout = new LinearLayout(this);
            MailLoginLayout.LayoutParameters = WrapContParams;
            MailLoginLayout.Orientation = Orientation.Horizontal;
            //Defining the Mail Login TextView
            MailLoginTV = new TextView(this);
            MailLoginTV.LayoutParameters = WrapContParams;
            MailLoginTV.Text = "EMail: ";
            MailLoginTV.TextSize = 30;
            MailLoginTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            MailLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Mail Login EditText
            MailLoginET = new EditText(this);
            MailLoginET.LayoutParameters = OneTwentyParams;
            MailLoginET.Hint = "Enter EMail";
            MailLoginET.InputType = InputTypes.TextVariationEmailAddress;
            MailLoginET.TextSize = 30;
            MailLoginET.SetSingleLine();
            //Adding views to layout
            MailLoginLayout.AddView(MailLoginTV);
            MailLoginLayout.AddView(MailLoginET);
            OverAllLoginLayout.AddView(MailLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining SportLoginLayout
            SportLoginLayout = new LinearLayout(this);
            SportLoginLayout.LayoutParameters = WrapContParams;
            SportLoginLayout.Orientation = Orientation.Horizontal;
            //Defining the Sport Login TextView
            SportLoginTV = new TextView(this);
            SportLoginTV.LayoutParameters = WrapContParams;
            SportLoginTV.Text = "Sport: ";
            SportLoginTV.TextSize = 30;
            SportLoginTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            SportLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Sport Login EditText
            SportLoginET = new EditText(this);
            SportLoginET.LayoutParameters = OneTwentyParams;
            SportLoginET.Hint = "Sport";
            SportLoginET.TextSize = 30;
            SportLoginET.InputType = InputTypes.TextVariationShortMessage;
            SportLoginET.SetSingleLine();
            //Adding views to layout
            SportLoginLayout.AddView(SportLoginTV);
            SportLoginLayout.AddView(SportLoginET);
            OverAllLoginLayout.AddView(SportLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining PhoneNumberLoginLayout
            PhoneNumberLoginLayout = new LinearLayout(this);
            PhoneNumberLoginLayout.LayoutParameters = WrapContParams;
            PhoneNumberLoginLayout.Orientation = Orientation.Horizontal;
            //Defining the PhoneNumber Login TextView
            PhoneNumberLoginTV = new TextView(this);
            PhoneNumberLoginTV.LayoutParameters = WrapContParams;
            PhoneNumberLoginTV.Text = "PhoneNum: ";
            PhoneNumberLoginTV.TextSize = 30;
            PhoneNumberLoginTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            PhoneNumberLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the PhoneNumber Login EditText
            PhoneNumberLoginET = new EditText(this);
            PhoneNumberLoginET.LayoutParameters = OneTwentyParams;
            PhoneNumberLoginET.Text = "05";
            PhoneNumberLoginET.TextSize = 30;
            PhoneNumberLoginET.InputType = InputTypes.ClassPhone;
            PhoneNumberLoginET.SetSingleLine();
            //Adding views to layout
            PhoneNumberLoginLayout.AddView(PhoneNumberLoginTV);
            PhoneNumberLoginLayout.AddView(PhoneNumberLoginET);
            OverAllLoginLayout.AddView(PhoneNumberLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining AgeLoginLayout
            AgeLoginLayout = new LinearLayout(this);
            AgeLoginLayout.LayoutParameters = WrapContParams;
            AgeLoginLayout.Orientation = Orientation.Horizontal;
            //Defining the Age Login TextView
            AgeLoginTV = new TextView(this);
            AgeLoginTV.LayoutParameters = WrapContParams;
            AgeLoginTV.Text = "Age: ";
            AgeLoginTV.TextSize = 30;
            AgeLoginTV.SetForegroundGravity(Android.Views.GravityFlags.Center);
            AgeLoginTV.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Age Login EditText
            AgeLoginET = new EditText(this);
            AgeLoginET.LayoutParameters = OneTwentyParams;
            AgeLoginET.Hint = "Age";
            AgeLoginET.TextSize = 30;
            AgeLoginET.InputType = InputTypes.ClassPhone;
            AgeLoginET.SetSingleLine();
            //Adding views to layout
            AgeLoginLayout.AddView(AgeLoginTV);
            AgeLoginLayout.AddView(AgeLoginET);
            OverAllLoginLayout.AddView(AgeLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            CBLoginLayout = new LinearLayout(this);
            CBLoginLayout.LayoutParameters = MatchParentParams2;
            LoginCB = new CheckBox(this);
            LoginCB.SetWidth(900);
            LoginCB.SetHeight(50);
            LoginCB.Text = "Keep LoggedIn?";
            CBLoginLayout.AddView(LoginCB);
            OverAllLoginLayout.AddView(CBLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining Login Button Layout
            ButtonLoginLayout = new LinearLayout(this);
            ButtonLoginLayout.LayoutParameters = WrapContParams;
            ButtonLoginLayout.Orientation = Orientation.Horizontal;
            //Defining Login Button
            LoginButton = new Button(this);
            LoginButton.LayoutParameters = WrapContParams;
            LoginButton.Text = "Register";
            LoginButton.TextSize = 40;
            LoginButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LoginButton.Click += this.LoginButton_Click;
            //Adding views
            ButtonLoginLayout.AddView(LoginButton);
            OverAllLoginLayout.AddView(ButtonLoginLayout);
        }
        //Building Register Screen

        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            bool keep = LoginCB.Checked;
            int AgeParsed = 0;
            int.TryParse(AgeLoginET.Text, out AgeParsed);
            //validation of input
            if (IsValidName(NameLoginET.Text) && IsValidSport(SportLoginET.Text) & isValidEmail(MailLoginET.Text) && PhoneNumberLoginET.Text.Length == 10 && AgeParsed > 0 && AgeParsed <= 99)
            {
                Toast.MakeText(this, "Logged-in", ToastLength.Long).Show();
                //if(MailLoginET.text   Not in   database)
                admin = new Admin1(int.Parse(AgeLoginET.Text), SportLoginET.Text, NameLoginET.Text, PhoneNumberLoginET.Text, MailLoginET.Text, keep);
                HashMap map = new HashMap();
                map.Put("Name", admin.name);
                map.Put("EMail", admin.email);
                map.Put("Age", AgeParsed);
                map.Put("PhoneNum", admin.phoneNumber);
                map.Put("Sport", admin.sport);
                map.Put("Login", keep);
                DocumentReference DocRef = database.Collection("Users").Document(admin.email);
                if (keep) //CancelLoginAbilityOnAllUsers();
                DocRef.Set(map);
                Intent intent1 = new Intent(this, typeof(MainPageActivity));
                intent1.PutExtra("Admin", JsonConvert.SerializeObject(admin));
                StartActivity(intent1);
            }
        }
        //When LogIn Button Is Clicked

        public bool isValidEmail(string email)
        {
            if (Android.Util.Patterns.EmailAddress.Matcher(email).Matches()) return true;
            else { Toast.MakeText(this, "MailInvalid", ToastLength.Long).Show(); return false; }
            //https://www.c-sharpcorner.com/article/how-to-validate-an-email-address-in-xamarin-android-app-using-visual-studio-2015/ @Delpin Susai Raj 
        }
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
            Tr =  name.Length >= 4 && name.Length <= 16;
            for (int i = 0; i < FN.Count; i++)
            {
                if (name.Contains(FN[i])) Tr = false;
            }
            if (!Tr) { Toast.MakeText(this, "Name InValid", ToastLength.Long).Show(); return Tr; }
            else return Tr;
        }
        //Name Validation

        public void CancelLoginAbilityOnAllUsers()
        {
            Query query = database.Collection("Users").WhereEqualTo("Login", true);
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
                            if ((item.Get("Login")).ToString() == "true")
                            {
                                HashMap map = new HashMap();
                                map.Put("Name", item.Get("Name").ToString());
                                map.Put("Age", int.Parse(item.Get("Age").ToString()));
                                map.Put("Sport", item.Get("Sport").ToString());
                                map.Put("EMail", item.Get("EMail").ToString());
                                map.Put("PhoneNum", item.Get("PhoneNum").ToString());
                                map.Put("LogIn", false);
                                DocumentReference docref = database.Collection("Users").Document(item.Get("EMail").ToString());
                                docref.Set(map);
                            }
                        }
                    }
                }
            }
            ));
        }
        //Makes sure only one user has Login true
        public bool IsValidSport(string sport)
        {
            if (sport.Length > 3) return true;
            else { Toast.MakeText(this, "Sport InValid", ToastLength.Long).Show(); return false;}
        }
        //Sport Validation

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
/* https://www.youtube.com/watch?v=A9rcKZUm0zM
 * changing the action bar and removing it
 */

/*@Racil Hilan
 * https://stackoverflow.com/questions/22254479/xamarin-edittext-inputtype-password 
 * edittext inputtype
 */
