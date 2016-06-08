using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleStore.Domain.Abstract;
using SimpleStore.WebUI.Models;

namespace SimpleStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int pageSize = 4;
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
        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(w => w.Category == null || w.Category == category)
                    .OrderBy(o => o.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Products.Count()
                }
            };
            return View(model);
        }
    }
}