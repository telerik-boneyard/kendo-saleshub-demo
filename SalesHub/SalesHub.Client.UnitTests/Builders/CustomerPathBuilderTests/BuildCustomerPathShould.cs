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
            SellingCompany sellingCompany = new SellingCompany
            {
                Abbreviation = "Test"
            };

            Customer customer = new Customer
            {
                CustomerName = "Customer"
            };

            var customerPathBuilder = new CustomerPathBuilder();

            var result = customerPathBuilder.BuildCustomerPath(sellingCompany, customer);

            Assert.AreEqual("Test -> Customer", result);
        }
    }
}
