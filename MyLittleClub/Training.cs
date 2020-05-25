using System.Collections.Generic;

namespace MyLittleClub
{
    public class Training
    {

        public List<Exercise> Exercises { get; set; }
        public double duration { get; set; }
        public Training()
        {

        }
        public Training(double duration)
        {
            this.duration = duration;
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
    }
}