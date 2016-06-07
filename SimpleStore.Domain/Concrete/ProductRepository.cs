using SimpleStore.Domain.Abstract;
using SimpleStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        ToyDbContext dbContext = new ToyDbContext();
        public IEnumerable<Product> Products 
        {
            get { return dbContext.Products; }
        }
    }
}
