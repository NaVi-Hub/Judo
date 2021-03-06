﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Java.Util;
using Xamarin.Essentials;
using Android.InputMethodServices;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Provider;
using Android.Runtime;
using System;
using Android.Graphics.Drawables;
using Android.Views.Animations;
using System.Linq;
using Android.Support.Design.Widget;
using Android;
using System.Threading.Tasks;
using Android.Telephony;

namespace MyLittleClub
{
    [Activity(Theme = "@style/AppTheme")]
    public class RegisterActivity : Activity
    {
        Admin1 admin;
        LinearLayout OverAllLoginLayout, NameLoginLayout, MailLoginLayout, SportLoginLayout, ButtonLoginLayout, LabelLoginLayout, PhoneNumberLoginLayout;
        TextView LabelLoginTV, LabelLoginTV1, NameLoginTV, MailLoginTV, SportLoginTV, PhoneNumberLoginTV, Login1;
        TextInputEditText NameLoginET, MailLoginET, SportLoginET, PhoneNumberLoginET;
        Button LoginButton;
        LinearLayout.LayoutParams MatchParentParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
        LinearLayout.LayoutParams OneTwentyParams = new LinearLayout.LayoutParams(420, 180);
        LinearLayout.LayoutParams ETparams = new LinearLayout.LayoutParams(500, 140);
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams MatchParentParams2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 200);
        FirebaseFirestore database;
        ISharedPreferences sp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.RegisterLayout);
            database = MyStuff.database;
            GetEmails();
            TryToGetPermissions();
        }
        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }

        }
        const int RequestLocationId = 0;
        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.SendSms,
                            Manifest.Permission.WriteSms,
             };
        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                return;
            }
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need SMS permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                return;
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "SMS permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //https://github.com/egarim/XamarinAndroidSnippets/blob/master/XamarinAndroidRuntimePermissions
        //https://youtu.be/Uzpy3qdYXmE
        #endregion
        void BuildRegisterScreen()
        {
            //Defining the parent layout
            OverAllLoginLayout = (LinearLayout)FindViewById(Resource.Id.LoginLinearLayout);
            OverAllLoginLayout.Orientation = Orientation.Vertical;
            OverAllLoginLayout.SetGravity(GravityFlags.CenterHorizontal);
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
            //Defining the Name Login TextInputEditText
            TextInputLayout name = new TextInputLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            NameLoginET = new TextInputEditText(this);
            NameLoginET.SetBackgroundResource(Resource.Drawable.MyBackground);
            NameLoginET.LayoutParameters = ETparams;
            NameLoginET.Hint = "Enter Name";
            NameLoginET.TextSize = 30;
            NameLoginET.InputType = InputTypes.TextVariationPersonName;
            NameLoginET.SetSingleLine();
            //Adding views to layout
            NameLoginLayout.AddView(NameLoginTV);
            name.AddView(NameLoginET);
            NameLoginLayout.AddView(name);
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
            //Defining the Mail Login TextInputEditText
            TextInputLayout mail = new TextInputLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            MailLoginET = new TextInputEditText(this);
            MailLoginET.LayoutParameters = ETparams;
            MailLoginET.SetBackgroundResource(Resource.Drawable.MyBackground);
            MailLoginET.Hint = "Enter EMail";
            MailLoginET.InputType = InputTypes.TextVariationEmailAddress;
            MailLoginET.TextSize = 30;
            MailLoginET.SetSingleLine();
            //Adding views to layout
            MailLoginLayout.AddView(MailLoginTV);
            mail.AddView(MailLoginET);
            MailLoginLayout.AddView(mail);
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
            //Defining the Sport Login TextInputEditText
            TextInputLayout sport = new TextInputLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            SportLoginET = new TextInputEditText(this);
            SportLoginET.SetBackgroundResource(Resource.Drawable.MyBackground);
            SportLoginET.LayoutParameters = ETparams;
            SportLoginET.Hint = "Sport";
            SportLoginET.TextSize = 30;
            SportLoginET.InputType = InputTypes.TextVariationShortMessage;
            SportLoginET.SetSingleLine();
            //Adding views to layout
            SportLoginLayout.AddView(SportLoginTV);
            sport.AddView(SportLoginET);
            SportLoginLayout.AddView(sport);
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
            //Defining the PhoneNumber Login TextInputEditText
            TextInputLayout phon = new TextInputLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = WrapContParams,
            };
            PhoneNumberLoginET = new TextInputEditText(this);
            PhoneNumberLoginET.SetBackgroundResource(Resource.Drawable.MyBackground);
            PhoneNumberLoginET.LayoutParameters = ETparams;
            PhoneNumberLoginET.Text = "05";
            PhoneNumberLoginET.TextSize = 30;
            PhoneNumberLoginET.InputType = InputTypes.ClassPhone;
            PhoneNumberLoginET.SetSingleLine();
            //Adding views to layout
            PhoneNumberLoginLayout.AddView(PhoneNumberLoginTV);
            phon.AddView(PhoneNumberLoginET);
            PhoneNumberLoginLayout.AddView(phon);
            OverAllLoginLayout.AddView(PhoneNumberLoginLayout);
            //=======================================================================================================================================
            //=======================================================================================================================================
            //Defining LoginLoginLayout
            LoginLoginLayout = new LinearLayout(this);
            LoginLoginLayout.LayoutParameters = WrapContParams;
            LoginLoginLayout.Orientation = Orientation.Horizontal;
            LoginLoginLayout.SetGravity(GravityFlags.Left);
            //
            Login1 = new TextView(this);
            Login1.Text = "Log-In";
            Login1.SetTextColor(Color.Blue);
            Login1.Click += this.Login1_Click;
            Login1.TextSize = 30;
            LoginLoginLayout.AddView(Login1);
            OverAllLoginLayout.AddView(LoginLoginLayout);
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
            CameraUsage();
            ButtonLoginLayout.AddView(LoginButton);
            OverAllLoginLayout.AddView(ButtonLoginLayout);
            OverAllLoginLayout.AddView(ImageViewProfileImage);
        }
        //Builds the enitre screen
        Dialog d;
        private void Login1_Click(object sender, System.EventArgs e)
        {
            Login1.SetTextColor(Color.Purple);
            BuildLoginScreen();
        }
        //Open Login Dialog
        LinearLayout LoginLoginLayout,dLayout, MailLoginLayout1, ButtonLoginLayout1,ImageViewProfileImageLayout;
        TextView MailLoginTV1;
        TextInputEditText MailLoginET1;
        Button LoginButton1, ImageViewProfileImageButton;
        private void BuildLoginScreen()
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.MyDialog);
            d.SetCancelable(true);
            d.SetTitle("LogIn");
            //OverallLayout
            dLayout = d.FindViewById<LinearLayout>(Resource.Id.AbcDEF);
            //Dialog Title Layout
            LinearLayout DialogTitleLayout = new LinearLayout(this);
            DialogTitleLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 100);
            DialogTitleLayout.SetFadingEdgeLength(100);
            DialogTitleLayout.SetBackgroundColor(Color.RoyalBlue);
            //Dialog TitleTV
            TextView LoginDialogTitle = new TextView(this);
            LoginDialogTitle.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LoginDialogTitle.SetTextColor(Color.WhiteSmoke);
            LoginDialogTitle.TextSize = 35;
            LoginDialogTitle.VerticalFadingEdgeEnabled = true;
            LoginDialogTitle.SetFadingEdgeLength(30);
            LoginDialogTitle.Text = "Welcome Back";
            DialogTitleLayout.AddView(LoginDialogTitle);
            dLayout.AddView(DialogTitleLayout);
            //Defining MailLoginLayout
            MailLoginLayout1 = new LinearLayout(this);
            MailLoginLayout1.LayoutParameters = WrapContParams;
            MailLoginLayout1.Orientation = Orientation.Horizontal;
            //Defining the Mail Login TextView
            MailLoginTV1 = new TextView(this);
            WrapContParams.SetMargins(20, 20, 5, 10);
            MailLoginTV1.LayoutParameters = WrapContParams;
            MailLoginTV1.Text = "EMail: ";
            MailLoginTV1.TextSize = 30;
            MailLoginTV1.SetForegroundGravity(Android.Views.GravityFlags.Center);
            MailLoginTV1.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            //Defining the Mail Login TextInputEditText
            TextInputLayout a = new TextInputLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            MailLoginET1 = new TextInputEditText(this);
            MailLoginET.SetBackgroundResource(Resource.Drawable.MyBackground);
            OneTwentyParams.SetMargins(5, 20, 20, 10);
            MailLoginET1.LayoutParameters = OneTwentyParams;
            MailLoginET1.Hint = "Enter EMail";
            MailLoginET1.InputType = InputTypes.TextVariationEmailAddress;
            MailLoginET1.TextSize = 30;
            MailLoginET1.SetSingleLine();
            MailLoginET1.SetBackgroundResource(Resource.Drawable.MyBackground);
            //Adding views to layout
            MailLoginLayout1.AddView(MailLoginTV1);
            a.AddView(MailLoginET1);
            MailLoginLayout1.AddView(a);
            dLayout.AddView(MailLoginLayout1);
            //
            //
            //Defining Login Button Layout
            ButtonLoginLayout1 = new LinearLayout(this);
            WrapContParams.SetMargins(20, 10, 20, 20);
            ButtonLoginLayout1.LayoutParameters = WrapContParams;
            ButtonLoginLayout1.Orientation = Orientation.Horizontal;
            //Defining Login Button
            LoginButton1 = new Button(this);
            LoginButton1.LayoutParameters = WrapContParams;
            LoginButton1.Text = "Log-In";
            LoginButton1.TextSize = 40;
            LoginButton1.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            LoginButton1.Click += this.LoginButton1_Click;
            //Adding views
            ButtonLoginLayout1.AddView(LoginButton1);
            dLayout.AddView(ButtonLoginLayout1);

            //
            d.Show();
        }
        //Build login Dialog
        ImageView ImageViewProfileImage;
        Bitmap BitProfilePic;
        public void CameraUsage()
        {
            ImageViewProfileImage = new ImageView(this);
            //Defining AddImage Button Layout
            ImageViewProfileImageLayout = new LinearLayout(this);
            WrapContParams.SetMargins(20, 10, 20, 20);
            ImageViewProfileImageLayout.LayoutParameters = WrapContParams;
            ImageViewProfileImageLayout.Orientation = Orientation.Horizontal;
            //Defining AddImage Button
            ImageViewProfileImageButton = new Button(this);
            ImageViewProfileImageButton.LayoutParameters = WrapContParams;
            ImageViewProfileImageButton.Text = "Take Profile Pic";
            ImageViewProfileImageButton.TextSize = 40;
            ImageViewProfileImageButton.Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf");
            ImageViewProfileImageButton.Click += this.ImageViewProfileImageButton_Click;

            ImageViewProfileImageLayout.AddView(ImageViewProfileImageButton);
            OverAllLoginLayout.AddView(ImageViewProfileImageLayout);
        }
        //Manages Camera
        private void ImageViewProfileImageButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
        //Intents to Camera
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                ImageViewProfileImage.SetImageBitmap(bitmap);
                BitmapDrawable drawable = (BitmapDrawable)ImageViewProfileImage.Drawable;
                BitProfilePic = drawable.Bitmap;
            }
            catch { }
        }
        //This is called after camera took a picture
        private void LoginButton1_Click(object sender, System.EventArgs e)
        {
            ButtonLoginLayout1.RequestFocus();
            string mail = MailLoginET1.Text;
            if (MyStuff.isValidEmail(mail ,this))
            {
                if(MyStuff.Emails.Contains(mail))
                {
                    GetAdmin(mail);
                }
                else
                {
                    Toasty.Error(this, "Email Not Found", 5, true);
                    MailLoginET1.Text = "";
                }
            }
        }
        //Log-in process and validation email is clear
        public void GetAdmin(string email)
        {
            Query query = database.Collection("Users").WhereEqualTo("EMail", email);
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
                            string adminemail = item.GetString("EMail");
                            string adminName = item.GetString("Name");
                            string adminphonenum = item.GetString("PhoneNum");
                            string adminsport = item.GetString("Sport");
                            string profilepic = item.GetString("Profile");
                            Admin1 a = new Admin1(adminsport, adminName, adminphonenum, adminemail, profilepic);
                            MyStuff.PutToShared(a);
                        }
                    }
                }
                Intent i = new Intent(this, typeof(MainPageActivity));
                Toasty.Success(this, "Logged-In Successfully", 5, true).Show();
                StartActivity(i);
            }
            ));
        }
        //Building Register Screen
        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            if (!MyStuff.Emails.Contains(MailLoginET.Text))
            {
                //validation of input
                if (MyStuff.IsValidName(NameLoginET.Text, NameLoginET, this) && MyStuff.IsValidSport(SportLoginET.Text, this) & MyStuff.isValidEmail(MailLoginET.Text, this) && PhoneNumberLoginET.Text.Length == 10 && PhoneNumberLoginET.Text.ToString().All(c => Char.IsLetterOrDigit(c)))
                {
                    string image = "";
                    try { image = MyStuff.ConvertBitMapToString(BitProfilePic); }
                    catch { };
                    Toasty.Config.Instance
                       .TintIcon(true)
                       .SetToastTypeface(Typeface.CreateFromAsset(Assets, "Katanf.ttf"));
                    admin = new Admin1(SportLoginET.Text, NameLoginET.Text, PhoneNumberLoginET.Text, MailLoginET.Text, image);
                    HashMap map = new HashMap();
                    map.Put("Name", admin.name);
                    map.Put("EMail", admin.email);
                    map.Put("PhoneNum", admin.phoneNumber);
                    map.Put("Sport", admin.sport);
                    map.Put("Profile", admin.ProfilePic);
                    DocumentReference DocRef = database.Collection("Users").Document(admin.email);
                    DocRef.Set(map);
                    MyStuff.PutToShared(admin);
                    Intent intent1 = new Intent(this, typeof(MainPageActivity));
                    Toasty.Success(this, "Edited successfully", 5, true).Show();
                    //
                    SmsManager sm = SmsManager.Default;
                    sm.SendTextMessage(PhoneNumberLoginET.Text /*מספר טלפון*/, null, "Welcome to T-POV, " + NameLoginET.Text + "!"/*תכולה*/, null, null);
                    //
                    StartActivity(intent1);
                }
            }
            else
            {
                Toasty.Error(this, "Email Already In Database", 5, true).Show();
                MailLoginET.Text = "";
            }
        }
        //When LogIn Button Is Clicked
        public List<string> GetEmails()
        {
            MyStuff.Emails = new List<string>();
            Query query = database.Collection("Users");
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
                            MyStuff.Emails.Add(item.GetString("EMail"));
                        }
                    }
                }
                BuildRegisterScreen();
            }
            )) ;
            return MyStuff.Emails;
        }
    }
}
/* https://www.youtube.com/watch?v=A9rcKZUm0zM
 * changing the action bar and removing it
 */

/*@Racil Hilan
 * https://stackoverflow.com/questions/22254479/xamarin-TextInputEditText-inputtype-password 
 * TextInputEditText inputtype
 */

/* taking an image
 * https://www.c-sharpcorner.com/article/camera-application-create/
 */