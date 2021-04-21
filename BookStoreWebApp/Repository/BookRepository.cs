using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public class BookRepository
    {
        private static List<BookModel> _booklist = new List<BookModel>()
        {
            new BookModel{Id=1,Title="JAVA",Author="Nitish",Language="English",Category="Action",CoverImageUrl="/imgs/user-320X320.png" },
            new BookModel{Id=2,Title="C#",Author="Boolin",Language="Chinese",Category="Romance",CoverImageUrl="/imgs/user-320X320.png"},
            new BookModel{Id=3,Title="Python",Author="Swanston",Language="Chinese",Category="Adventure" ,CoverImageUrl="/imgs/user-320X320.png"},
            new BookModel{Id=4,Title="F#",Author="Jony" ,Language="Chinese",Category="Adventure",CoverImageUrl="/imgs/user-320X320.png"},
            new BookModel{Id=5,Title="JavaScript",Author="Smith",Language="Chinese" ,Category="Adventure",CoverImageUrl="/imgs/user-320X320.png"},
            new BookModel{Id=6,Title="C++",Author="Tomsen",Language="Chinese",Category="Romance" ,CoverImageUrl="/imgs/user-320X320.png"},
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

        public int CreateABook(BookModel book)
        {
            if (book.Id == 0)
            {
                book.Id = _booklist.Max(x => x.Id) + 1;
            }
            _booklist.Add(book);
            return book.Id;
        }
    }
}
