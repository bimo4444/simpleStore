using System.Web.Mvc;
using Moq;
using Ninject;
using SimpleStore.Domain.Entities;
using SimpleStore.Domain.Abstract;
using System.Collections.Generic;
using System;
using SimpleStore.Domain.Concrete;

namespace SimpleStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Category = "gggg", Description = "hhhh", Name = "name1", Price = 111, ProductId = 0},
                new Product { Category = "gggg", Description = "hhhh", Name = "name2", Price = 222, ProductId = 1},
                new Product { Category = "gg", Description = "hhhh", Name = "name3", Price = 333, ProductId = 2},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 3},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 4},
                new Product { Category = "hhh", Description = "hhhh", Name = "name4", Price = 444, ProductId = 5},
                new Product { Category = "hhh", Description = "hhhh", Name = "name4", Price = 444, ProductId = 6},
                new Product { Category = "hhh", Description = "hhhh", Name = "name4", Price = 444, ProductId = 7},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 8},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 9},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 10},
                new Product { Category = "gg", Description = "hhhh", Name = "name4", Price = 444, ProductId = 11}
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            //kernel.Bind<IProductRepository>().To<ProductRepository>();
        }
    }
}