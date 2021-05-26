using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = new BookRepository();
            _webHostEnvironment = webHostEnvironment;
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

        [Authorize]
        public IActionResult AddNewBook(bool isSuccess=false, int bookId=0)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Id = bookId;
            ViewBag.Categories = new List<string> { "Action", "Romance", "Adventure", "History", "Family" };
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel book)
        {
            if (book.CoverImage != null)
            {
                string folder = "imgs/userUploadImages";
                string serverRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() +"_" + book.CoverImage.FileName;
                string serverFolderPath = Path.Combine(serverRootPath, folder,fileName);

                await book.CoverImage.CopyToAsync(new FileStream(serverFolderPath, FileMode.Create));
                
                //这是供前端使用的URL地址，在HTML中需要在图片地址前加上"/"
                book.CoverImageUrl = "/"+Path.Combine(folder, fileName);
            }
            if (book.BookPdf != null)
            {
                string folder = "pdfs";
                string serverRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + book.BookPdf.FileName;
                string serverFolderPath = Path.Combine(serverRootPath, folder, fileName);

                await book.BookPdf.CopyToAsync(new FileStream(serverFolderPath, FileMode.Create));
                book.BookPdfUrl = "/" + Path.Combine(folder, fileName);
            }
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
