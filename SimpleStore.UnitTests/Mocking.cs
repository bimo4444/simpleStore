using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleStore.Domain.Abstract;
using SimpleStore.Domain.Entities;
using SimpleStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using SimpleStore.WebUI.Models;
using SimpleStore.WebUI.HtmlHelpers;
namespace SimpleStore.UnitTests
{
    [TestClass]
    public class Mocking
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 0, Name="test0"},
                new Product { ProductId = 1, Name="test1"},
                new Product { ProductId = 2, Name="test2"},
                new Product { ProductId = 3, Name="test3"},
                new Product { ProductId = 4, Name="test4"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // assert
            List<Product> games = result.Products.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "test3");
            Assert.AreEqual(games[1].Name, "test4");
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            Mock<IProductRepository> mock = GetMock();
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((ProductsListViewModel)controller.List("cat0").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)controller.List("cat1").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("cat2").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 1);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 2);
            Assert.AreEqual(resAll, 5);
        }

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product product1 = new Product { ProductId = 1, Name = "name1" };
            Product product2 = new Product { ProductId = 2, Name = "name2" };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            List<CartLine> result = cart.Lines.ToList();
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Product, product1);
            Assert.AreEqual(result[1].Product, product2);
        }

        [TestMethod]
        public void Can_Add_New_Lines_To_Existing_Lines()
        {
            Product product1 = new Product { ProductId = 1, Name = "name1" };
            Product product2 = new Product { ProductId = 2, Name = "name2" };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            List<CartLine> result = cart.Lines.OrderBy(o => o.Product.ProductId).ToList();
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product product1 = new Product { ProductId = 1, Name = "name1" };
            Product product2 = new Product { ProductId = 2, Name = "name2" };
            Product product3 = new Product { ProductId = 3, Name = "name3" };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 4);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 1);
            cart.RemoveLine(product2);
            Assert.AreEqual(cart.Lines.Where(w => w.Product == product2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product product1 = new Product { ProductId = 1, Name = "name1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "name2", Price = 55 };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            decimal result = cart.ComputeTotalValue();
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product product1 = new Product { ProductId = 1, Name = "name1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "name2", Price = 55 };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.Clear();
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            Mock<IProductRepository> mock = GetMock();
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Product> result = ((ProductsListViewModel)controller.List("cat2", 1).Model)
                .Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "test3" && result[0].Category == "cat2");
            Assert.IsTrue(result[1].Name == "test4" && result[1].Category == "cat2");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductRepository> mock = GetMock();

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "cat0";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }
        private Mock<IProductRepository> GetMock()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
                {
                    new Product { ProductId = 0, Name="test0", Category="cat0"},
                    new Product { ProductId = 1, Name="test1", Category="cat1"},
                    new Product { ProductId = 2, Name="test2", Category="cat1"},
                    new Product { ProductId = 3, Name="test3", Category="cat2"},
                    new Product { ProductId = 4, Name="test4", Category="cat2"}
                });
            return mock;
        }

        [TestMethod]
        public void Can_Create_categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = GetMock();

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "cat0");
            Assert.AreEqual(results[1], "cat1");
            Assert.AreEqual(results[2], "cat2");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = GetMock();
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };


            Func<int, string> pageUrlDelegate = i => "Page" + i;


            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
    }
}
