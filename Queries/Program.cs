using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>()
            {
                new Movie(){Title = "The Dark Knight", Rating = 8.9f, Year = 2008},
                new Movie(){Title = "The King's Speech", Rating = 8.0f, Year = 2010},
                new Movie(){Title = "Casablanca", Rating = 8.5f, Year = 1942},
                new Movie(){Title = "Star Wars V", Rating = 8.7f, Year = 1980}
            };

            var query = movies.Filter(m => m.Year > 2000);

            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

            //This uses a static method with an infinite loop, which uses deferred execution (facilitated by 'yield return'),
            //to allow only 10 results to be returned, rather than looping forever. This is because the Random() method yields to
            //caller - Take(10) - which completes once 10 values have been found.
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }
        }
    }
}
