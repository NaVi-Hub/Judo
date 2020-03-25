using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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
                SortArray(this.Warmup);
                this.Exercises.Add(this.Warmup);
                //
                this.MainPractice = mainPractice;
                SortArray(this.MainPractice);
                this.Exercises.Add(this.MainPractice);
                //
                this.EndClass = endClass;
                SortArray(this.EndClass);
                this.Exercises.Add(this.EndClass);
            }
         }
        //constructor

        public double DurationSum (Exercise[] ex)
        {
            double TR = 0;
            for(int i = 0; i<ex.Length; i++)
            {
                TR += ex[i].duration;
            }
            return TR;
        }
        //sums the duration of all exercies in the array

        public void SortArray(Exercise[] ex)
        {
            Exercise temp;
            bool NotSorted = true;
            bool happended = false;
            while (NotSorted)
            {
                for (int k = 1; k < ex.Length; k++)
                {
                    if (ex[k - 1].order > ex[k].order)
                    {
                        temp = ex[k];
                        ex[k] = ex[k - 1];
                        ex[k - 1] = temp;
                        happended = true;
                    }
                }
                if (!happended)
                    NotSorted = false;
                else
                    happended = false;
            }
        }
        //sorts the array by order
    }
}