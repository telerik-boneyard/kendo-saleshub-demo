using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SalesHub.Core.Models;

namespace SalesHub.Data
{
    public class SalesHubDbInitializer : DropCreateDatabaseIfModelChanges<SalesHubDbContext>
    {
        private readonly Random _random = new Random();

        protected override void Seed(SalesHubDbContext context)
        {
            base.Seed(context);

            User user = new User
            {
                UserId = 1,
                UserName = "test",
                FullName = "John Smith",
                PhoneNumber = "Something",
                EmailAddress = "test@test.com"
            };

            SellingCompany sellingCompany = new SellingCompany
            {
                CompanyName = "Goods Selling Company",
                Abbreviation = "GSC",
                ManagedBy = new List<User>
                {
                    user
                }
            };

            CurrencyType currencyType = new CurrencyType
                {
                    CurrencyName = "USD"
                };
            context.CurrencyTypes.Add(currencyType);

            context.SellingCompanies.Add(sellingCompany);

            user.SellingCompanies = new List<SellingCompany> { sellingCompany };
            context.Users.Add(user);

            context.SaveChanges();

            GeneratePackageTypes(context);
            GeneratePaymentTermTypes(context);
            GenerateCustomersForCompany(sellingCompany, context);

            context.SaveChanges();
        }
  
        private void GeneratePaymentTermTypes(SalesHubDbContext context)
        {
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 1, Name = "After Arrival" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 2, Name = "After Arrival & Inspection" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 3, Name = "After B/L Date" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 4, Name = "After Invoice Date" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 5, Name = "Against Packing" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 6, Name = "Against Receipt of Documents" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 7, Name = "Inter-company" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 8, Name = "On Due Date" });
            context.PaymentTermTypes.Add(new PaymentTermType { PaymentTermTypeId = 9, Name = "Prepayment" });
        }

        private void GenerateCustomersForCompany(SellingCompany sellingCompany, SalesHubDbContext context)
        {
            string [] customerNames = new[]
                {
                "Alfreds Futterkiste",
                "Ana Trujillo Emparedados y helados",
                "Antonio Moreno Taquería",
                "Around the Horn",
                "Berglunds snabbköp",
                "Blauer See Delikatessen",
                "Blondesddsl père et fils",
                "Bólido Comidas preparadas",
                "Bon app'",
                "Bottom-Dollar Markets",
                "B's Beverages",
                "Cactus Comidas para llevar",
                "Centro comercial Moctezuma",
                "Chop-suey Chinese",
                "Comércio Mineiro",
                "Consolidated Holdings",
                "Die Wandernde Kuh",
                "Drachenblut Delikatessen",
                "Du monde entier",
                "Eastern Connection",
                "Folk och fä HB",
                "France restauration",
                "Franchi S.p.A.",
                "Frankenversand",
                "Furia Bacalhau e Frutos do Mar",
                "alería del gastrónomo",
                "Godos Cocina Típica",
                "Lazy K Kountry Store",
                "Lehmanns Marktstand",
                "Let's Stop N Shop",
                "LILA-Supermercado",
                "LINO-Delicateses",
                "Lonesome Pine Restaurant",
                "Magazzini Alimentari Riuniti",
                "Maison Dewey",
                "Mère Paillarde",
                "Morgenstern Gesundkost",
                "North/South",
                "Rancho grande",
                "Rattlesnake Canyon Grocery",
                "Reggiani Caseifici",
                "Ricardo Adocicados",
                "Richter Supermarkt",
                "Romero y tomillo",
                "Santé Gourmet",
                "Save-a-lot Markets",
                "Suprêmes délices",
                "The Big Cheese",
                "The Cracker Box",
                "Victuailles en stock",
                "Vins et alcools Chevalier",
                "Wartian Herkku",
                "Wellington Importadora",
                "White Clover Markets",
                "Wilman Kala",
                "Wolski  Zajazd"
            };

            foreach (string customerName in customerNames)
            {
                var customer = new Customer
                    {
                        CustomerName = customerName,
                        SellingCompany = sellingCompany,
                        Orders = new List<Order>(),
                        OrderDetailOriginVisible = _random.Next() % 2 == 0
                    };

                context.Customers.Add(customer);
                context.SaveChanges();
                GenerateOrdersForCustomer(customer, context);
            }
        }

        private void GenerateOrdersForCustomer(Customer customer, SalesHubDbContext context)
        {
            int orderCount = _random.Next(10, 20);

            for (int i = 0; i < orderCount; ++i)
            {
                var order = new Order
                {
                    ContractAmount = _random.Next(100, 1000000),
                    ContractWeight = _random.Next(10, 1000000),
                    ContractCurrencyTypeId = 0,
                    Customer = customer,
                    IsActive = true,
                    OrderDate = DateTime.Today,
                    OrderNumber = "Order - " + i,
                };

                customer.Orders.Add(order);
                context.Orders.Add(order);
                context.SaveChanges();
                var orderDetails = GenerateOrderDetailsForOrder(order, context);

                order.ContractAmount = orderDetails.Sum(od => od.PricePerUnitOfWeight*od.Units);
                order.ContractWeight = orderDetails.Sum(od => od.Units*od.UnitWeight);
            }
        }

        private List<OrderDetail> GenerateOrderDetailsForOrder(Order order, SalesHubDbContext context)
        {
            var orderDetails = new List<OrderDetail>();
            var numToGenerate = _random.Next(2, 10);
            for (int i = 0; i < numToGenerate; i++)
            {
                var orderDetail = new OrderDetail
                    {
                        PricePerUnitOfWeight = _random.Next(4, 100),
                        CropYear = _random.Next(2002, 2013),
                        Destination = "",
                        LotNumber = "",
                        OrderId = order.OrderId,
                        Origin = "",
                        PackageTypeId = _random.Next(1, context.PackageTypes.Count()),
                        UnitWeight = _random.Next(10, 20),
                        Units = _random.Next(2000, 6000),
                        ValueDate = DateTime.Now.AddDays(_random.Next(2, 19))
                    };
                orderDetail.NetWeight = orderDetail.UnitWeight * orderDetail.Units;
                orderDetails.Add(orderDetail);
                context.OrderDetails.Add(orderDetail);
            }
            return orderDetails;
        }

        private void GeneratePackageTypes(SalesHubDbContext context)
        {
            context.PackageTypes.Add(new PackageType { PackageTypeId = 1, Name = "Cases" });
            context.PackageTypes.Add(new PackageType {PackageTypeId = 2, Name = "Bales"});
            context.PackageTypes.Add(new PackageType {PackageTypeId = 3, Name = "Hogsheads"});
            context.PackageTypes.Add(new PackageType { PackageTypeId = 4, Name = "Bags" });
            context.SaveChanges();
        }
    }
}
