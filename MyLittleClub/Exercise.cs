namespace MyLittleClub
{
    public class Exercise
    {
        public string name { get; set; }
        public double duration { get; set; }
        public string explenatiotn { get; set; }
        public Exercise(string name, double duration, string explenation)
        {
            this.name = name;
            this.duration = duration;
            this.explenatiotn = explenation;
        }
        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=netframework-4.8  
    }
}