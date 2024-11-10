using System;
using System.Collections;

namespace BookLibrary
{
    class Book : IComparable, ICloneable
    {
        string name;
        string author;

        public Book(string name, string author)
        {
            this.name = name;
            this.author = author;
        }
        public Book() : this("Том Сойер", "Марк Твен") { }

        public void Show()
        {
            Console.WriteLine("{0}   {1}", name, author);
        }
        public int CompareTo(object obj)
        {
            if (obj is Book)
                return name.CompareTo((obj as Book).name);
            throw new ArgumentException("Object is not a Book");
        }
        public object Clone()
        {
            return new Book(this.name, this.author);
        }
        public class SortByName : IComparer
        {
            public int Compare(object obj1, object obj2)
            {
                if (obj1 is Book && obj2 is Book)
                    return (obj1 as Book).name.CompareTo((obj2 as Book).name);
                throw new ArgumentException("Objects are not of type Book");
            }
        }
        public class SortByAuthor : IComparer
        {
            public int Compare(object obj1, object obj2)
            {
                if (obj1 is Book && obj2 is Book)
                    return (obj1 as Book).author.CompareTo((obj2 as Book).author);
                throw new ArgumentException("Objects are not of type Book");
            }
        }
    }
    class Library : IEnumerable
    {
        private Book[] books;

        public Library(Book[] books)
        {
            this.books = new Book[books.Length];
            for (int i = 0; i < books.Length; i++)
            {
                this.books[i] = (Book)books[i].Clone(); 
            }
        }
        public IEnumerator GetEnumerator()
        {
            return new LibraryEnumerator(books);
        }
        private class LibraryEnumerator : IEnumerator
        {
            private Book[] books;
            private int position = -1;

            public LibraryEnumerator(Book[] books)
            {
                this.books = books;
            }
            public bool MoveNext()
            {
                position++;
                return (position < books.Length);
            }
            public void Reset()
            {
                position = -1;
            }
            public object Current
            {
                get
                {
                    if (position == -1 || position >= books.Length)
                        throw new InvalidOperationException();
                    return books[position];
                }
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Book[] books = new Book[]
            {
                new Book("Три товарища", "Эрих Мария Ремарк"),
                new Book("Алхимик", "Пауло Коэльо"),
                new Book("Маленькая жизнь", "Ханья Янагихара"),
                new Book("451 градус по Фаренгейту", "Рэй Брэдбери"),
                new Book("Убить пересмешника", "Харпер Ли"),
                new Book("Том Сойер", "Марк Твен")
            };
            Console.WriteLine("Неупорядоченный массив:");
            foreach (Book book in books)
                book.Show();
            Array.Sort(books);
            Console.WriteLine("\nМассив сортированный по названиям книг(по умолчанию):");
            foreach (Book book in books)
                book.Show();
            Array.Sort(books, new Book.SortByName());
            Console.WriteLine("\nМассив сортированный по названиям книг:");
            foreach (Book book in books)
                book.Show();
            Array.Sort(books, new Book.SortByAuthor());
            Console.WriteLine("\nМассив сортированный по авторам:");
            foreach (Book book in books)
                book.Show();
            Library library = new Library(books);
            Console.WriteLine("\nПеребор книг в библиотеке:");
            foreach (Book book in library)
                book.Show();
        }
    }
}

