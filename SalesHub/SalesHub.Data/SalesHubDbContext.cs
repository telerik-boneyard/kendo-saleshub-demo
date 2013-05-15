using SalesHub.Core.Models;
using SalesHub.Core.Repositories;
using System.Data.Entity;

namespace SalesHub.Data
{
    public class SalesHubDbContext : DbContext, ISalesHubDbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<SellingCompany> SellingCompanies { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderDetail> OrderDetails { get; set; }
        public IDbSet<PaymentTerm> PaymentTerms { get; set; }
        public IDbSet<PaymentTermType> PaymentTermTypes { get; set; }
        public IDbSet<CurrencyType> CurrencyTypes { get; set; }
        public IDbSet<OrderNote> OrderNotes { get; set; }
        public IDbSet<PackageType> PackageTypes { get; set; }

        public void DisableProxies()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}
