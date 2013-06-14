using System;
using System.Linq;
using NUnit.Framework;
using SalesHub.Client.Builders;
using SalesHub.Core.Models;

namespace SalesHub.Client.UnitTests.Builders.CustomerPathBuilderTests
{
    [TestFixture]
    public class BuildCustomerPathShould
    {
        [TestCase]
        public void ShouldReturnTheSellingCompanyAbbrFollowedByTheCustomerName()
        {
            const string COMPANY_NAME = "Test Company";
            const string CUSTOMER_NAME = "Customer";

            SellingCompany sellingCompany = new SellingCompany
            {
                Abbreviation = "Test",
                CompanyName = COMPANY_NAME
            };

            Customer customer = new Customer
            {
                CustomerName = CUSTOMER_NAME
            };

            var customerPathBuilder = new CustomerPathBuilder();

            var result = customerPathBuilder.BuildCustomerPath(sellingCompany, customer);

            Assert.AreEqual(COMPANY_NAME + " &raquo; " + CUSTOMER_NAME, result);
        }
    }
}
