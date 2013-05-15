using System;
using System.Linq;
using SalesHub.Core.Models;

namespace SalesHub.Client.Builders
{
    public interface ICustomerPathBuilder
    {
        string BuildCustomerPath(SellingCompany sellingCompany, Customer customer);
    }
}