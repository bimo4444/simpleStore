using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleStore.Domain.Abstract;

namespace SimpleStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult List()
        {
            return View(repository.Products);
        }
    }
}