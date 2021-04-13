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
            new BookModel{Id=1,Title="C#",Author="Boolin" },
            new BookModel{Id=1,Title="Python",Author="Swanston" },
            new BookModel{Id=1,Title="F#",Author="Jony" },
            new BookModel{Id=1,Title="JavaScript",Author="Smith" },
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
