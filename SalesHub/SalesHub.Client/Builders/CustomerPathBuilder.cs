using System;
using SalesHub.Core.Models;

namespace SalesHub.Client.Builders
{
    public class CustomerPathBuilder : ICustomerPathBuilder
    {
        private const string PATH_FORMAT = "{0} -> {1}";

        public string BuildCustomerPath(SellingCompany sellingCompany, Customer customer)
        {
            return String.Format(PATH_FORMAT, sellingCompany.Abbreviation, customer.CustomerName);
        }
    }
}