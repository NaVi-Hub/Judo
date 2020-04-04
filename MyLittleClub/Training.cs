using System.Collections.Generic;

namespace MyLittleClub
{
    public class Training
    {

        public int SuggestedAge { get; set; }
        public List<Exercise> Exercises { get; set; }

        public Training()
        {

        }
        public Training(int sug)
        {
            this.SuggestedAge = sug;
        }
        public void AddTraining (Exercise ex)
        {
            this.Exercises.Add(ex);
        }
        public Training (Exercise[] exercises)
        {
            for (int k = 0; k < exercises.Length; k++)
            {
                this.Exercises[k] = exercises[k];
            }
        }
        public Training(List<Exercise> exercises)
        {
            this.Exercises = exercises;
        }

        public double DurationSum()
        {
            double TR = 0;
            for (int i = 0; i < this.Exercises.Count; i++)
            {
                TR += Exercises[i].duration;
            }
            return TR;
        }
        //sums the duration of all exercies in the array
    }
}