using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesHub.Client.ViewModels.Api;
using SalesHub.Core.Repositories;

namespace SalesHub.Client.Api
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CustomerOrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public CustomerOrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public JsonResult GetOrdersForCustomer([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            var orders = _orderRepository.GetAllOrders();

            return Json(orders.ToDataSourceResult(dataSourceRequest, o => new CustomerOrderViewModel
            {
                IsActive = o.IsActive,
                OrderDate = o.OrderDate,
                OrderId = o.OrderId,
                OrderNumber = o.OrderNumber,
                Value = o.ContractAmount,
                Weight = o.ContractWeight
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
