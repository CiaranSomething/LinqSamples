using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            //the Func<> delegate type takes generic type parameters defined in the <>
            //the last parameter is the return type
            Func<int, bool> isEven = x => x % 2 == 0; //returns a bool

            Func<int, int> square = x => x * x; //returns an int
            //does not need () as has 0 or 1 parameters

            Func<int, int, int> add = (x, y) => x + y; //returns an int
            //needs () to define parameters - (int x, int y)

            Console.WriteLine($"22 isEven: {isEven(22)}");
            Console.WriteLine($"3 squared is: {square(3)}");
            Console.WriteLine($"7 plus 8 is: {add(7, 8)}");
            Console.WriteLine($"The square of 7 + 9 is: {square(add(7, 9))}");


            //The Action delegate type is similar to Func<>, only it always returns void
            Action<string, string> insult = (first, second) => Console.WriteLine($"{first} {second}");

            insult("you", "smell");



            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Ciaran"},
                new Employee {Id = 2, Name = "Pooty"}
            };

            List<Employee> sales = new List<Employee>()
            {
                new Employee() {Id = 3, Name = "Pam"}
            };

            //This is basically what collections that implement IEnumerable<T> do when using for/foreach
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            //This uses an extension method off of IEnumerable<T> that we have created
            Console.WriteLine(developers.Count());

            //Use 'named method' in Where()
            foreach (var developer in developers.Where(NameStartsWithP))
            {
                Console.WriteLine("Named Method:");
                Console.WriteLine(developer.Name);
            }

            //use 'anonymous method' - i.e. delegate method - in Where()
            foreach (var developer in developers.Where(
                delegate(Employee employee) { return employee.Name.StartsWith("P"); }))
            {
                Console.WriteLine("Delegate Method:");
                Console.WriteLine(developer.Name);
            }

            //use 'lambda method' in Where()
            foreach (var developer in developers.Where(
                e => e.Name.StartsWith("P")))
            {
                Console.WriteLine("Lambda Method:");
                Console.WriteLine(developer.Name);
            }

            //use lambda expression to use a full LINQ query using lambda expressions
            foreach (var developer in developers.Where(e => e.Name.Length == 6)
                                                        .OrderBy(e => e.Name.StartsWith("C")))
            {
                Console.WriteLine("Developers with 6 letter names beginning with C:");
                Console.WriteLine(developer.Name);
            }

            //LINQ query - 'method syntax'
            var query = developers.Where(e => e.Name.Length == 5)
                                                        .OrderBy(e => e.Name);





            string[] cities = { "Boston", "Los Angeles", "Seattle", "London", "Hyderabad" };

            //LINQ query syntax
            IEnumerable<String> filteredCities1 =
                from city in cities
                where city.StartsWith("L") && city.Length < 15
                orderby city
                select city;

            //LINQ method syntax
            IEnumerable<String> filteredCities2 = cities.Where(c => c.StartsWith("L") && c.Length < 15)
                                                        .OrderBy(c => c);

        }

        private static bool NameStartsWithP(Employee employee)
        {
            return employee.Name.StartsWith("P");
        }
    }
}
