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
