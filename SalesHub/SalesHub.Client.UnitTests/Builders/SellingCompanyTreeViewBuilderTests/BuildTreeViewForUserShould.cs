using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Telerik.JustMock;
using SalesHub.Client.Builders;
using SalesHub.Core.Models;
using SalesHub.Core.Repositories;

namespace SalesHub.Client.UnitTests.Builders.SellingCompanyTreeViewBuilderTests
{
    [TestFixture]
    public class BuildTreeViewForUserShould
    {
        private User CreateUser()
        {
            return new User
            {
                UserName = "test",
                UserId = 1
            };
        }

        private SellingCompany CreateSellingCompany(User user)
        {
            return new SellingCompany
                {
                    SellingCompanyId = 1,
                    Abbreviation = "Test",
                    CompanyName = "Test Company",
                    ManagedBy = new List<User> {user}
                };
        }

        [TestCase]
        public void ReturnAnEmptyTreeViewRootIfThereArentAnySellingCompaniesForTheUser()
        {
            var user = CreateUser();

            user.SellingCompanies = new List<SellingCompany>();

            var mockCustomerRepository = Mock.Create<ICustomerRepository>();
            var mockCustomerPathBuilder = Mock.Create<ICustomerPathBuilder>();

            Mock.Arrange(() => mockCustomerPathBuilder.BuildCustomerPath(Arg.IsAny<SellingCompany>(), Arg.IsAny<Customer>())).Returns("Test path");

            var sellingCompanyTreeViewBuilder = new SellingCompanyTreeViewBuilder(mockCustomerRepository, mockCustomerPathBuilder);

            var result = sellingCompanyTreeViewBuilder.BuildTreeViewForUser(user);

            Assert.IsEmpty(result.Items);
            Mock.AssertAll(mockCustomerRepository);
        }

        [TestCase]
        public void ReturnATreeViewRootWithOneRootLevelItemThatIsTheCompanyIfThereArentAnyCustomersForTheCompany()
        {
            var user = CreateUser();
            var sellingCompany = CreateSellingCompany(user);
            user.SellingCompanies = new List<SellingCompany> { sellingCompany };

            var mockCustomerRepository = Mock.Create<ICustomerRepository>();
            var mockCustomerPathBuilder = Mock.Create<ICustomerPathBuilder>();

            Mock.Arrange(() => mockCustomerRepository.GetCustomersForSellingCompany(sellingCompany.SellingCompanyId)).Returns(new List<Customer>());
            Mock.Arrange(() => mockCustomerPathBuilder.BuildCustomerPath(Arg.IsAny<SellingCompany>(), Arg.IsAny<Customer>())).OccursNever();

            var builder = new SellingCompanyTreeViewBuilder(mockCustomerRepository, mockCustomerPathBuilder);

            var result = builder.BuildTreeViewForUser(user);

            Mock.AssertAll(mockCustomerRepository);
            Mock.AssertAll(mockCustomerPathBuilder);

            Assert.AreEqual(1, result.Items.Count);

            var companyNode = result.Items.First();

            Assert.AreEqual(sellingCompany.Abbreviation, companyNode.Text);
            Assert.IsEmpty(companyNode.Items);
        }

        [TestCase]
        public void GroupCustomersByTheFirstCharacterOfTheirName()
        {
            var user = CreateUser();
            var sellingCompany = CreateSellingCompany(user);
            user.SellingCompanies = new List<SellingCompany> {sellingCompany};

            var customer = new Customer
                {
                    CustomerId = 1,
                    CustomerName = "A - Customer 1",
                    SellingCompany = sellingCompany
                };
            var customers = new [] {customer};

            var mockCustomerRepository = Mock.Create<ICustomerRepository>();
            var mockCustomerPathBuilder = Mock.Create<ICustomerPathBuilder>();

            Mock.Arrange(() => mockCustomerRepository.GetCustomersForSellingCompany(sellingCompany.SellingCompanyId)).Returns(customers);
            Mock.Arrange(() => mockCustomerPathBuilder.BuildCustomerPath(Arg.IsAny<SellingCompany>(), Arg.IsAny<Customer>())).Returns("Test path");

            var builder = new SellingCompanyTreeViewBuilder(mockCustomerRepository, mockCustomerPathBuilder);

            var result = builder.BuildTreeViewForUser(user);

            Mock.AssertAll(mockCustomerPathBuilder);
            Mock.AssertAll(mockCustomerRepository);

            Assert.AreEqual(1, result.Items.Count);

            var companyNode = result.Items.First();

            Assert.AreEqual(sellingCompany.Abbreviation, companyNode.Text);
            Assert.AreEqual(1, companyNode.Items.Count);

            var grouping = companyNode.Items.First();

            Assert.AreEqual("A", grouping.Text);
            Assert.AreEqual(1, grouping.Items.Count);

            var customerNode = grouping.Items.First();

            Assert.AreEqual(customer.CustomerName, customerNode.Text);
        }

        [TestCase]
        public void SellingCompaniesShouldBeExpanded()
        {
            var user = CreateUser();
            var sellingCompany = CreateSellingCompany(user);

            user.SellingCompanies = new List<SellingCompany> { sellingCompany };

            var customer = new Customer
            {
                CustomerId = 1,
                CustomerName = "A - Customer 1",
                SellingCompany = sellingCompany
            };
            var customers = new [] { customer };

            var mockCustomerRepository = Mock.Create<ICustomerRepository>();
            var mockCustomerPathBuilder = Mock.Create<ICustomerPathBuilder>();

            Mock.Arrange(() => mockCustomerRepository.GetCustomersForSellingCompany(sellingCompany.SellingCompanyId)).Returns(customers);

            var builder = new SellingCompanyTreeViewBuilder(mockCustomerRepository, mockCustomerPathBuilder);

            var result = builder.BuildTreeViewForUser(user);

            Assert.AreEqual(1, result.Items.Count);

            var companyTreeViewItem = result.Items.First();

            Assert.IsTrue(companyTreeViewItem.Expanded);
            Mock.AssertAll(mockCustomerRepository);
        }

        [TestCase]
        public void SortCustomerGroupsAreSortedAlphabetically()
        {
            var user = CreateUser();
            var sellingCompany = CreateSellingCompany(user);

            user.SellingCompanies = new List<SellingCompany>() { sellingCompany };

            var customer = new Customer
            {
                CustomerId = 1,
                CustomerName = "A - Customer 1",
                SellingCompany = sellingCompany
            };

            var customer2 = new Customer
            {
                CustomerId = 2,
                CustomerName = "B - Customer 2",
                SellingCompany = sellingCompany
            };

            var customers = new [] { customer, customer2 };

            var mockCustomerRepository = Mock.Create<ICustomerRepository>();
            var mockCustomerPathBuilder = Mock.Create<ICustomerPathBuilder>();

            Mock.Arrange(() => mockCustomerRepository.GetCustomersForSellingCompany(sellingCompany.SellingCompanyId)).Returns(customers);
            Mock.Arrange(() => mockCustomerPathBuilder.BuildCustomerPath(Arg.IsAny<SellingCompany>(), Arg.IsAny<Customer>())).Returns("Test -> path").Occurs(2);

            var builder = new SellingCompanyTreeViewBuilder(mockCustomerRepository, mockCustomerPathBuilder);

            var result = builder.BuildTreeViewForUser(user);

            Mock.AssertAll(mockCustomerPathBuilder);
            Mock.AssertAll(mockCustomerRepository);

            var groups = result.Items.First().Items.ToList();

            Assert.AreEqual("A", groups[0].Text);
            Assert.AreEqual("B", groups[1].Text);
        }
    }
}
