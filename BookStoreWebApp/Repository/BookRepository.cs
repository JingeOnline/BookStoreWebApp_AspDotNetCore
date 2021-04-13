using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public class BookRepository
    {
        private List<BookModel> _booklist = new List<BookModel>()
        {
            new BookModel{Id=1,Title="JAVA",Author="Nitish" },
            new BookModel{Id=2,Title="C#",Author="Boolin" },
            new BookModel{Id=3,Title="Python",Author="Swanston" },
            new BookModel{Id=4,Title="F#",Author="Jony" },
            new BookModel{Id=5,Title="JavaScript",Author="Smith" },
            new BookModel{Id=6,Title="C++",Author="Tomsen" },
        };

        public List<BookModel> GetAllBooks()
        {
            return _booklist;
        }

        public BookModel GetBookById(int id)
        {
            BookModel book = _booklist.Where(x => x.Id == id).FirstOrDefault();
            return book;
        }

        public List<BookModel> SearchBookByTitle(string title)
        {
            List<BookModel> books = _booklist.Where(x => x.Title.Contains(title)).ToList();
            return books;
        }
    }
}
