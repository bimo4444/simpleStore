using SimpleStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Products
                .Select(s => s.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }
    }
}