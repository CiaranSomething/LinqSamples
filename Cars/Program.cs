using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");

            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");


            ////returns an IEnumerable<Car>
            //var query = cars.Join(manufacturers,
            //                      c => new { c.Manufacturer, c.Year },
            //                      m => new { Manufacturer = m.Name,  m.Year},
            //                      (c, m) => new
            //                      {
            //                          m.Headquarters,
            //                          c.Name,
            //                          c.Combined
            //                      })
            //                .OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name);

            //foreach (var car in query.Take(10))
            //{
            //    Console.WriteLine($"{car.Name}: {car.Combined}");
            //}


            //var query = cars.GroupBy(c => c.Manufacturer.ToUpper())
            //                .OrderBy(g => g.Key);


            //Use the GroupJoin() method
            //var query = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) => new
            //    {
            //        Manufacturer = m,
            //        Cars = g
            //    })
            //    .OrderBy(m => m.Manufacturer.Name);

            //foreach (var group in query)
            //{
            //    Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");
            //    foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2)) 
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}


            //Take top 3 performing cars by country
            //var query = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
            //        (m, g) => new
            //                    {
            //                        Manufacturer = m,
            //                        Cars = g
            //                    })
            //                .GroupBy(m => m.Manufacturer.Headquarters);


            //foreach (var group in query)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach (var car in group.SelectMany(g => g.Cars)
            //                             .OrderByDescending(c => c.Combined)
            //                             .Take(3))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}


            //Use the values for the cars to work out some values
            //var query = cars.GroupBy(c => c.Manufacturer)
            //    .Select(g =>
            //    {
            //    var results = g.Aggregate(new CarStatistics(),
            //                        (acc, c) => acc.Accumulate(c),
            //                        acc => acc.Compute());
            //        return new
            //        {
            //            Name = g.Key,
            //            Avg = results.Average,
            //            Min = results.Min,
            //            Max = results.Max
            //        };              
            //    })
            //    .OrderByDescending(r => r.Max);

            //foreach (var result in query)
            //{
            //    Console.WriteLine(result.Name);
            //    Console.WriteLine($"\tMax: {result.Max}");
            //    Console.WriteLine($"\tMin; {result.Min}");
            //    Console.WriteLine($"\tAvg: {result.Avg}");
            //}


            ////returns a single Car by using the First() method
            //var topJag = cars.Where(c => c.Manufacturer == "Jaguar")
            //                                    .OrderByDescending(c => c.Combined)
            //                                    .ThenBy(c => c.Name)
            //                                    .Select(c => c)
            //                                    .First();

            //var result = cars.SelectMany(c => c.Name)
            //                    .OrderBy(c => c);


            //XML Stuff
            CreateXml();
            QueryXML();

            Console.WriteLine("Goodbye");

        }

        private static void QueryXML()
        {
            var ns = (XNamespace)"http://plurlsight.com/cars/2016";
            var ex = (XNamespace)"http://plurlsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");

            var query = document.Element(ns + "Cars").Elements(ex + "Car")
            //OR
            //var query = document.Descendants(ex + "Car")
                                .Where(e => e.Attribute("Manufacturer")?.Value == "BMW")    //? operator checks if the attribute is null
                                .Select(e => e.Attribute("Name")?.Value);

            foreach (var result in query)
            {
                Console.WriteLine(result);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var ns = (XNamespace)"http://plurlsight.com/cars/2016";
            var ex = (XNamespace)"http://plurlsight.com/cars/2016/ex";
            var document = new XDocument();

            //extension method syntax
            var allCars = new XElement(ns + "Cars",
                    records.Select(c =>
                    {
                        return new XElement(ex + "Car",
                            new XAttribute("Name", c.Name),
                            new XAttribute("Combined", c.Combined),
                            new XAttribute("Manufacturer", c.Manufacturer));
                    })
            );

            allCars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            ////The query syntax
            //var allCars = new XElement(ns + "Cars",

            //    from record in records
            //    select new XElement(ex + "Car",
            //                        new XAttribute("Name", record.Name),
            //                        new XAttribute("Combined", record.Combined),
            //                        new XAttribute("Manufacturer", record.Manufacturer))

            //    );

            document.Add(allCars);
            document.Save("fuel.xml");
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Where(l => l.Length > 1)
                            .ToManufacturer();

            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
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

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;

        }

        public CarStatistics Accumulate(Car car)
        {
            Count++;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);

            return this;
        }

        public  CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
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

    public static class ManufacturerExtensions
    {
        public static IEnumerable<Manufacturer> ToManufacturer(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Manufacturer
                {
                    Name = columns[0],
                    Headquarters = columns[1],              
                    Year = int.Parse(columns[2])
                };
            }
        }
    }
}