using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using Firebase.Firestore;
using Java.Util;

namespace MyLittleClub
{
    [Activity(Label = "EditAdminActivity")]
    public class EditAdminActivity : Activity
    {
        Admin1 admin;
        FirebaseFirestore database;
        ISharedPreferences sp;
        bool EmailChanged = false;
        LinearLayout OverAllEditAdminLayout, NameEditAdminLayout, MailEditAdminLayout, SportEditAdminLayout, PhoneNumEditAdminLayout, ProfilePicEditAdminLayout, ButtonEditAdminLayout, ProfileEditAdminLyout;
        EditText NameEt, SportEt, MailEt, PhoneNumEt;
        TextView NameTv, SportTv, MailTv, phoneNumtv;
        Button ProfilePicButton, SaveButton;
        ImageView ProImg;
        Bitmap ProBit;
        LinearLayout.LayoutParams WrapContLay = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams BLP = new LinearLayout.LayoutParams(400, 200);
        LinearLayout.LayoutParams EtLP = new LinearLayout.LayoutParams(650, 200);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditAminLayout);
            admin = MyStuff.GetAdmin();
            database = MyStuff.database;
            sp = MyStuff.sp;
            BuildScreen();
            // Create your application here
        }
        public void BuildScreen()
        {
            OverAllEditAdminLayout = FindViewById<LinearLayout>(Resource.Id.EdaitAdminLinearLayout);
            OverAllEditAdminLayout.Orientation = Orientation.Vertical;
            OverAllEditAdminLayout.SetGravity(GravityFlags.CenterHorizontal);
            //
            NameEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            //
            NameTv = new TextView(this)
            {
                LayoutParameters = BLP,
                Text = "Name: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 30,
            };
            NameTv.SetTextColor(Color.DarkRed);
            //
            NameEt = new EditText(this)
            {
                LayoutParameters = EtLP,
                Text = admin.name,
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            //
            NameEditAdminLayout.AddView(NameTv);
            NameEditAdminLayout.AddView(NameEt);
            OverAllEditAdminLayout.AddView(NameEditAdminLayout);

            //

            MailEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            //
            MailTv = new TextView(this)
            {
                LayoutParameters = BLP,
                Text = "Email: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 30,
            };
            MailTv.SetTextColor(Color.DarkRed);
            //
            MailEt = new EditText(this)
            {
                LayoutParameters = EtLP,
                Text = admin.email,
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            MailEt.Click += this.MailEt_Click; 
            MailEditAdminLayout.AddView(MailTv);
            MailEditAdminLayout.AddView(MailEt);
            OverAllEditAdminLayout.AddView(MailEditAdminLayout);

            //

            PhoneNumEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            //
            phoneNumtv = new TextView(this)
            {
                LayoutParameters = BLP,
                Text = "PhonenNum: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 30,
            };
            phoneNumtv.SetTextColor(Color.DarkRed);
            //
            PhoneNumEt = new EditText(this)
            {
                LayoutParameters = EtLP,
                Text = admin.phoneNumber,
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            PhoneNumEditAdminLayout.AddView(phoneNumtv);
            PhoneNumEditAdminLayout.AddView(PhoneNumEt);
            OverAllEditAdminLayout.AddView(PhoneNumEditAdminLayout);

            //

            SportEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            SportTv = new TextView(this)
            {
                LayoutParameters = BLP,
                Text = "Sport: ",
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
                TextSize = 30,
            };
            SportTv.SetTextColor(Color.DarkRed);
            //
            SportEt = new EditText(this)
            {
                LayoutParameters = EtLP,
                Text = admin.sport,
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            //
            SportEditAdminLayout.AddView(SportTv);
            SportEditAdminLayout.AddView(SportEt);
            OverAllEditAdminLayout.AddView(SportEditAdminLayout);

            //
            
            ProfilePicEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            ProfilePicButton = new Button(this)
            {
                Text = "Take Pic",
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            ProfilePicButton.SetTextColor(Color.DarkRed);
            ProfilePicButton.Click += this.ProfilePicButton_Click;
            ProfilePicEditAdminLayout.AddView(ProfilePicButton);
            OverAllEditAdminLayout.AddView(ProfilePicEditAdminLayout);

            //

            ButtonEditAdminLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            SaveButton = new Button(this)
            {
                Text = "Save",
                TextSize = 30,
                Typeface = Typeface.CreateFromAsset(Assets, "Katanf.ttf"),
            };
            SaveButton.SetTextColor(Color.DarkRed);
            SaveButton.Click += this.SaveButton_Click;
            ButtonEditAdminLayout.AddView(SaveButton);
            OverAllEditAdminLayout.AddView(ButtonEditAdminLayout);

            //
            ProfileEditAdminLyout = new LinearLayout(this)
            {
                LayoutParameters = WrapContLay,
                Orientation = Orientation.Horizontal,
            };
            ProImg = new ImageView(this);
            ProImg.SetImageBitmap(MyStuff.ConvertStringToBitMap(admin.ProfilePic));
            ProfileEditAdminLyout.AddView(ProImg);
            ProImg.SetMinimumWidth(400);
            ProImg.SetMinimumHeight(650);
            BitmapDrawable drawable = (BitmapDrawable)ProImg.Drawable;
            ProBit = drawable.Bitmap;
            OverAllEditAdminLayout.AddView(ProfileEditAdminLyout);
        }
        //builds the screen
        private void MailEt_Click(object sender, EventArgs e)
        {
            EmailChanged = true;
        }
        //Sets EmailChanged to true

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (EmailChanged && !MyStuff.Emails.Contains(MailEt.Text))
            {
                //validation of input
                if (MyStuff.IsValidName(NameEt.Text, this) && MyStuff.IsValidSport(SportEt.Text, this) & MyStuff.isValidEmail(MailEt.Text, this) && PhoneNumEt.Text.Length == 10)
                {
                    string image = MyStuff.ConvertBitMapToString(ProBit);
                    //if(MailLoginET.text   Not in   database)
                    admin = new Admin1(SportEt.Text, NameEt.Text, PhoneNumEt.Text, MailEt.Text, image);
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
                    Toasty.Success(this, "Registered Succesfully", 5, true);
                    StartActivity(intent1);
                }
            }
            else if(EmailChanged && MyStuff.Emails.Contains(MailEt.Text))
            {
                Toasty.Error(this, "Email Already in database", 5, true).Show();
                MailEt.Text = "";
                MailEt.SetBackgroundColor(Color.Red);
            }
            else
            {

                string image = MyStuff.ConvertBitMapToString(ProBit);
                //if(MailLoginET.text   Not in   database)
                admin = new Admin1(SportEt.Text, NameEt.Text, PhoneNumEt.Text, MailEt.Text, image);
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
                Toasty.Success(this, "Registered Succesfully", 5, true);
                StartActivity(intent1);
            }
        }
        //Saves Changes
        private void ProfilePicButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
        //Intents to take picture
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bitmap = (Bitmap)data.Extras.Get("data");
            ProImg.SetImageBitmap(bitmap);
            BitmapDrawable drawable = (BitmapDrawable)ProImg.Drawable;
            ProBit = drawable.Bitmap;
        }
        //Gets called when picture is taked
    }
}