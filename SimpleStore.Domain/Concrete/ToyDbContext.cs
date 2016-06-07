using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SimpleStore.Domain.Entities;
    
namespace SimpleStore.Domain.Concrete
{
    public class ToyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
