using System.Web.Mvc;
using Moq;
using Ninject;
using SimpleStore.Domain.Entities;
using SimpleStore.Domain.Abstract;
using System.Collections.Generic;

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
        public object GetService(System.Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Category = "", Description = "", Name = "name1", Price = 111, ProductId = 0},
                new Product { Category = "", Description = "", Name = "name2", Price = 222, ProductId = 1},
                new Product { Category = "", Description = "", Name = "name3", Price = 333, ProductId = 2},
                new Product { Category = "", Description = "", Name = "name4", Price = 444, ProductId = 3}
            });
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }
    }
}