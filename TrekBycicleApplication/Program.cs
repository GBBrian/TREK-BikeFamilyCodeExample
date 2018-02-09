using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrekBycicleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var workingClass = new Work();

            var jsonStringObject = workingClass.GetJSONStringData();

            var output = workingClass.JSONStringToFamilyBikes(jsonStringObject);

            var mostPopular = workingClass.DetermineMostPopularBikeCombinations(output);

            Console.WriteLine("The 20 most most popular combinations of bikes from the survey are: ");
            Console.Write(Environment.NewLine);

            for (int i = 0; i < 20; i++)
            {
                for (int b = 0; b < mostPopular[i].bikes.Count(); b++)
                {
                    Console.Write(mostPopular[i].bikes[b]);

                    if (b < mostPopular[i].bikes.Count() - 1)
                    {
                        Console.Write(" | ");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine("(" + mostPopular[i].PopularityCount+ ")");
            }

            Console.Read();
        }
    }
}
