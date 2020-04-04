using System.Collections.Generic;

namespace MyLittleClub
{
    public class Training
    {

        public int SuggestedAge { get; set; }
        public Exercise[] Warmup { get; set; }
        public Exercise[] MainPractice { get; set; }
        public Exercise[] EndClass { get; set; }
        public List<Exercise[]> Exercises { get; set; }

        public Training(int suggestedAge, Exercise[] warmup, Exercise[] mainPractice, Exercise[] endClass)
        {
            this.SuggestedAge = suggestedAge;
            this.Exercises = new List<Exercise[]>();
            if (DurationSum(warmup) + DurationSum(mainPractice) + DurationSum(endClass) >= 40 && DurationSum(warmup) + DurationSum(mainPractice) + DurationSum(endClass) <= 50)
            {
                this.Warmup = warmup;
                this.Exercises.Add(this.Warmup);
                //
                this.MainPractice = mainPractice;
                this.Exercises.Add(this.MainPractice);
                //
                this.EndClass = endClass;
                this.Exercises.Add(this.EndClass);
            }
        }
        //constructor

        public double DurationSum(Exercise[] ex)
        {
            double TR = 0;
            for (int i = 0; i < ex.Length; i++)
            {
                TR += ex[i].duration;
            }
            return TR;
        }
        //sums the duration of all exercies in the array
    }
}