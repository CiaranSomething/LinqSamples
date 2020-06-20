using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    public class Movie
    {
        public string Title { get; set; }
        private int _year;

        public int Year
        {
            get
            {
                Console.WriteLine($"Returning year for {Title}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
        public double Rating { get; set; }
    }
}
