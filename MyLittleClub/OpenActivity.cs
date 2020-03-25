using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using Java.Lang;
using Java.Util;
using Newtonsoft.Json;

namespace MyLittleClub
{
    [Activity(Label = "OpenActivity", MainLauncher = true)]
    public class OpenActivity : Activity, IOnSuccessListener 
    {
        public static FirebaseFirestore database;
        Admin1 admin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            database = GetDataBase();
            FetchData();
        }
        public void FetchData()
        {
            database.Collection("Users")
                .Get()
                .AddOnSuccessListener(this);
        }
        public FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore database;
            var options = new FirebaseOptions.Builder()
                .SetProjectId("mylittleclubproject")
                .SetApplicationId("mylittleclubproject")
                .SetApiKey("AIzaSyDG3jgrxvbvW8pwKZRPXjsm1EHNAkM_k5U")
                .SetDatabaseUrl("https://mylittleclubproject.firebaseio.com")
                .SetStorageBucket("mylittleclubproject.appspot.com")
                .Build();
            var app = FirebaseApp.InitializeApp(this, options);
            database = FirebaseFirestore.GetInstance(app);
       
            return database;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach (DocumentSnapshot item in documents)
                {
                    if ((item.Get("Login")).ToString() == "true")
                    {
                        admin = new Admin1();
                        admin.name = item.Get("Name").ToString();
                        admin.age = int.Parse(item.Get("Age").ToString());
                        admin.sport = item.Get("Sport").ToString();
                        admin.email = item.Get("EMail").ToString();
                        admin.phoneNumber = item.Get("PhoneNum") != null ? item.Get("PhoneNum").ToString() : "05 null";
                        admin.LogIn = (bool)item.Get("LogIn");
                        Intent intent1 = new Intent(this, typeof(MainPageActivity));
                        intent1.PutExtra("Admin", JsonConvert.SerializeObject(admin));
                        StartActivity(intent1);
                    }
                }
            }
            else
            {
                Intent intent2 = new Intent(this, typeof(RegisterActivity));
                StartActivity(intent2);
            }
        }
        //Firebase defining
    }
}
/*
 * var snapshot = (QuerySnapshot)task.Result;
                    if (!snapshot.IsEmpty)
                    {
                        var document = snapshot.Documents;
                        foreach (DocumentSnapshot item in document)
                        {
                            if ((item.Get("Login")).ToString() == "true")
                            {
                                admin = new Admin1();
                                admin.name = item.Get("Name").ToString();
                                admin.age = int.Parse(item.Get("Age").ToString());
                                admin.sport = item.Get("Sport").ToString();
                                admin.email = item.Get("EMail").ToString();
                                admin.phoneNumber = item.Get("PhoneNum").ToString();
                                admin.LogIn = (bool)item.Get("LogIn");
                                Intent intent1 = new Intent(this, typeof(MainPageActivity));
                                intent1.PutExtra("Admin", JsonConvert.SerializeObject(admin));
                                StartActivity(intent1);
                            }
                        }
                    }
                    else
                    {
                        Intent intent2 = new Intent(this, typeof(RegisterActivity));
                        StartActivity(intent2);
                    }
*/