using Android.App;
using Android.Content;
using Android.OS;
using Firebase;
using Firebase.Firestore;
using Newtonsoft.Json;

namespace MyLittleClub
{
    [Activity(Label = "OpenActivity", MainLauncher = true)]
    public class OpenActivity : Activity
    {

        public static FirebaseFirestore database;
        Admin1 admin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            database = GetDataBase();
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
                }
            }));
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
        //Firebase defining
    }
}