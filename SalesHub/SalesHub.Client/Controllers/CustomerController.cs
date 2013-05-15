using System.Web.Mvc;
using SalesHub.Client.Builders;
using SalesHub.Client.ViewModels.Client;
using SalesHub.Core.Models;
using SalesHub.Core.Repositories;

namespace SalesHub.Client.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerPathBuilder _customerPathBuilder;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerPathBuilder customerPathBuilder, ICustomerRepository customerRepository)
        {
            _customerPathBuilder = customerPathBuilder;
            _customerRepository = customerRepository;
        }

        public ViewResult Details(int id)
        {
            Customer customer = _customerRepository.GetCustomerById(id);
            var model = new CustomerDetailsViewModel
            {
                CustomerId = id,
                CustomerPath = _customerPathBuilder.BuildCustomerPath(customer.SellingCompany, customer),
            };
            return View(model);
        }
    }
}
