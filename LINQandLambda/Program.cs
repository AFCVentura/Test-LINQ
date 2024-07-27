using LINQandLambda.Entities;
using System.Linq;

namespace LINQandLambda
{
    internal class Program
    {
        static void Print<T> (string message, IEnumerable<T> collection)
        {
            Console.WriteLine(message);
            foreach (T obj in collection)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            #region instantiating
            #region categories
            Category c1 = new Category()
            {
                Id = 1,
                Name = "Tools",
                Tier = 2
            };
            Category c2 = new Category()
            {
                Id = 2,
                Name = "Computers",
                Tier = 1
            };
            Category c3 = new Category()
            {
                Id = 3,
                Name = "Eletronics",
                Tier = 1
            };
            #endregion

            #region products
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Computer",
                    Price = 1100,
                    Category = c2
                },
                new Product()
                {
                    Id = 2,
                    Name = "Hammer",
                    Price = 90,
                    Category = c1
                },
                new Product()
                {
                    Id = 3,
                    Name = "TV",
                    Price = 1700,
                    Category = c3
                },
                new Product()
                {
                    Id = 4,
                    Name = "Notebook",
                    Price = 1300,
                    Category = c2
                },
                new Product()
                {
                    Id = 5,
                    Name = "Saw",
                    Price = 80,
                    Category = c1
                },
                new Product()
                {
                    Id = 6,
                    Name = "Tablet",
                    Price = 700,
                    Category = c2
                },
                new Product()
                {
                    Id = 7,
                    Name = "Camera",
                    Price = 700,
                    Category = c3
                },
                new Product()
                {
                    Id = 8,
                    Name = "Printer",
                    Price = 350,
                    Category = c3
                },
                new Product()
                {
                    Id = 9,
                    Name = "MacBook",
                    Price = 1800,
                    Category = c2
                },
                new Product()
                {
                    Id = 10,
                    Name = "Sound Bar",
                    Price = 700,
                    Category = c3
                },
                new Product()
                {
                    Id = 11,
                    Name = "Level",
                    Price = 70,
                    Category = c1
                }
            };
            #endregion
            #endregion


            // Note: I used var on the collections, but their type is IEnumereable (at least for most of them)

            // FIRST EXAMPLE: Show the products with Tier 1 and price below $900.00
            // Note: Where expects a Predicate, but the IDE says it expects a Func,
            // but a Func with a bool return and an object as parameter is basically a Predicate
            var r1 = products.Where(p => p.Category.Tier == 1 && p.Price < 900);
            Print("TIER 1 AND PRICE < 900", r1);


            // SECOND EXAMPLE: Show the name of the products from the category Tools
            var r2 = products.Where(p => p.Category.Name == "Tools").Select(p => p.Name);
            Print("NAMES OF PRODUCTS FROM TOOLS", r2);


            // THIRD EXAMPLE: Show products whose names start with C
            // Note: it is possible to create an anonymous object that receives some attributes of the product
            // Note 2: it is possible to give an alias to an attribute, just like I did with p.Category.Name, calling it
            // CategoryName, you need to do it if there's ambiguity in the attribute's names
            var r3 = products.Where(p => p.Name[0] == 'C').Select(p => new { p.Name, p.Price, CategoryName = p.Category.Name });
            Print("PRODUCTS THAT START WITH C", r3);


            // FOURTH EXAMPLE: Show products whose tier is 1 ordered by price
            // Note: ThenBy is basically a second OrderBy to break the tie of the first one
            var r4 = products.Where(p => p.Category.Tier == 1).OrderBy(p => p.Price).ThenBy(p => p.Name);
            Print("PRODUCTS WHOSE TIER IS 1 ORDERED BY PRICE", r4);


            // FIFTH EXAMPLE: Use the previous collection to skip 2 products and then take 4
            var r5 = r4.Skip(2).Take(4);
            Print("PRODUCTS WHOSE TIER IS 1 ORDERED BY PRICE, SKIPING 2 AND TAKING 4", r5);


            // SIXTH EXAMPLE: Pick the first product 
            // Note: This time, the var type is just Product, not IEnumerable<Product>
            var r6 = products.First();
            Console.WriteLine($"First product: {r6}");


            // SEVENTH EXAMPLE: Trying to pick the first product of an empty collection
            /*var r7 = products.Where(p => p.Price > 3000).First();   
             *This line will throw an exception because there's no product with a price this big*/
            // Note: To avoid exceptions, you can use FirstOrDefault, wich returns null in this case
            var r7 = products.Where(p => p.Price > 3000).FirstOrDefault();
            Console.WriteLine($"First product: {r7}");


            // EIGHTH EXAMPLE: Trying to pick a product based on it's ID
            // Note: The method SingleOrDefault (there's also just Single, but they differentiate just like First and FirstOrDefault)
            // Transforms what would be a collection with one item in, an item
            // If you use it, r8 will be a Product, if not, it'll be an IEnumerable<Product> with just one Product
            var r8 = products.Where(p => p.Id == 3).SingleOrDefault();
            Console.WriteLine($"Single of Default - Test1: {r8}");
            // Using Single or SingleOrDefault with a query that returns more than an item will throw an exception


            // NINTH EXAMPLE: Get the biggest and lowest price
            // Note: If you don't put any parameters, the method Max will expect that the type of the collection
            // has the IComparable interface implemented, in this case I used the price to compare, wich is a double
            // and double is a type who has the IComparable implemented
            var r9 = products.Max(p => p.Price);
            Console.WriteLine($"Max Price: ${r9:F2}");
            var r10 = products.Min(p => p.Price);
            Console.WriteLine($"Min Price: ${r10:F2}");


            // TENTH EXAMPLE: Get all the items of the first category and sum their prices
            var r11 = products.Where(p => p.Category.Id == 1).Sum(p => p.Price);
            Console.WriteLine($"Category 1 value: {r11:F2}");


            // ELEVENTH EXAMPLE: Average price
            var r12 = products.Where(p => p.Category.Id == 1).Average(p => p.Price);
            Console.WriteLine($"Category 1 average price: {r12:F2}");
            // Note: Average will return an exception if you try to calculate the average of an empty collection
            // To solve it, you can select the price and use DefaultIfEmpty to define a default value to the double collection
            var r13 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).DefaultIfEmpty(0.0).Average();
            Console.WriteLine($"Category 5 average price: {r13:F2}");


            // TWELFTH EXAMPLE: Creating your own function
            // Note: Aggregate is a method that you can use to apply your own method as if it is an aggregation function
            // In other languages, it is called "Reduce"
            var r14 = products.Where(p => p.Category.Id == 1).Select(p => p.Price).Aggregate((x, y) => x + y);
            Console.WriteLine($"Category 1 aggregated price sum: ${r14:F2}");
            // Note: If you pass an empty collecition, it'll throw an exception
            // To solve it, you can use another overload of this method that allows a default value
            var r15 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).Aggregate(0.0, (x, y) => x + y);
            Console.WriteLine($"Category 5 aggregated price sum: ${r15:F2}");

            // THIRTEENTH EXAMPLE: Group by
            // Note: if you look at r16's type, it'll show an IEnumerable of IGroupings of Category and Product
            // IGrouping is a very interesting interface, it has a key (in this case, category) and a collection of values
            // (in this case, a collection of Products) that can be associated with that key
            var r16 = products.GroupBy(p => p.Category);
            // Note: if you want to iterate over a list, you need to use a special foreach
            Console.WriteLine();
            foreach (IGrouping<Category, Product> group in r16)
            {
                Console.WriteLine($"Category: {group.Key.Name}:");
                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }
            

        }
    }
}
