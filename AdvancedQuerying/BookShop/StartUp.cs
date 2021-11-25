namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //2. Age Restriction
            //var command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));

            //3. Golden Books
            //Console.WriteLine(GetGoldenBooks(db));

            //4. Books by Price
            //Console.WriteLine(GetBooksByPrice(db));

            //5. Not Released In
            //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));

            //6. Book Titles by Category
            //var input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, input));

            //7. Released Before Date
            string date = Console.ReadLine();
            Console.WriteLine(GetBooksReleasedBefore( db,  date));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            string commandToLower = command.ToLower();

            var books = context.Books
                //.ToArray()
                .Where(b => b.AgeRestriction == Enum.Parse<AgeRestriction>(command, true))
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldEditionBooks = Enum.Parse<EditionType>("Gold");

            var books = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == goldEditionBooks)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();
            
            var sb = new StringBuilder();

            foreach(var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new 
                { 
                    b.Title, 
                    b.Price 
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var sb = new StringBuilder();



                var books = context.Books
                    .Where(b => b.BookCategories
                        .Any(c => categories.Contains(c.Category.Name.ToLower())))
                    //.ToList()
                    //.Select(b => new
                    //{
                    //    b.Title,
                    //    BookCategories = b.BookCategories
                    //        .Select(c => new
                    //        {
                    //            CategoryName = c.Category.Name
                    //        })
                    //        .Where(c => c.CategoryName.ToLower() == cat)
                    //})
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToList();

                foreach (var book in books)
                {
                    sb.AppendLine(book);
                }


            return sb.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            //int year = int.Parse(date.Split('-')[2]);
            //int month = int.Parse(date.Split('-')[1]);
            //int day = int.Parse(date.Split('-')[0]);

            DateTime inputDate = Convert.ToDateTime(date);

            var books = context.Books
                .Where(b => b.ReleaseDate < inputDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            //Console.WriteLine(sb.ToString().Trim().Length);

            return sb.ToString().Trim();
        }
    }
}
