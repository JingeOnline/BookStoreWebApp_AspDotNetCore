using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;

        public BookController()
        {
            _bookRepository = new BookRepository();
        }

        
        public IActionResult Books()
        {
            
            return View(_bookRepository.GetAllBooks());
        }

        //[HttpGet("{id}")] 这样写，表示根路径/id
        [HttpGet("[controller]/{id}")]  //这样写的路径才是/book/{id}
        public IActionResult BookDetail(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        public IActionResult AddNewBook(bool isSuccess=false, int bookId=0)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Id = bookId;
            ViewBag.Categories = new List<string> { "Action", "Romance", "Adventure", "History", "Family" };
            return View();
        }

        [HttpPost]
        public IActionResult AddNewBook(BookModel book)
        {
            int id=_bookRepository.CreateABook(book);
            if (id > 0)
            {
                return RedirectToAction(nameof(AddNewBook), new { isSuccess = true,bookId=id });
            }
            return View();

            //直接重定向到被创建的书记页面
            //return RedirectToAction("BookDetail", new { id });
        }
    }
}
