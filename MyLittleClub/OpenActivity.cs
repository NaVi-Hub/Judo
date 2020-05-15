using Android.App;
using Android.Content;
using Android.OS;
using Firebase;
using Firebase.Firestore;

namespace MyLittleClub
{
    [Activity(Label = "TPOV", MainLauncher = true)]
    public class Context : Activity
    {
        public static FirebaseFirestore database;
        ISharedPreferences sp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            var editor = sp.Edit();
            base.OnCreate(savedInstanceState);
            database = GetDataBase();
            MyStuff.DefineShared(sp);
            MyStuff.DefineDatabase(database);
            if (sp.GetString("Name", "noname") != "noname")
            {
                Intent intent = new Intent(this, typeof(MainPageActivity));
                StartActivity(intent);
            }
            else
            {
                Intent intent = new Intent(this, typeof(RegisterActivity));
                StartActivity(intent);
            }
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