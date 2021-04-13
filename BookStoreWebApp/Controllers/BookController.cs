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
    }
}
