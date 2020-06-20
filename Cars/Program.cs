using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            //returns an IEnumerable<Car>
            var query = cars.Where(c => c.Manufacturer == "Jaguar")
                                                .OrderByDescending(c => c.Combined)
                                                .ThenBy(c => c.Name);

            //returns a Car
            var topJag = cars.Where(c => c.Manufacturer == "Jaguar")
                                                .OrderByDescending(c => c.Combined)
                                                .ThenBy(c => c.Name)
                                                .Select(c => c)
                                                .First();

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            var query =

            //method syntax
            File.ReadAllLines(path)
                    .Skip(1) //skip first line as it contains headers
                    .Where(l => l.Length > 1) //
                    .ToCar();


            //query syntax
            //    from line in File.ReadAllLines(path).Skip(1)
            //    where line.Length > 1
            //    select Car.ParseFromCsv(line);


            return query.ToList();
        }

    }
     
    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<String> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
