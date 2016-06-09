using SimpleStore.Domain.Abstract;
using SimpleStore.Domain.Entities;
using SimpleStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repo;
        public CartController(IProductRepository repo)
        {
            this.repo = repo;
        }
        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product product = repo.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if(product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repo.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if(cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }   
            return cart;
        }
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
	}
}