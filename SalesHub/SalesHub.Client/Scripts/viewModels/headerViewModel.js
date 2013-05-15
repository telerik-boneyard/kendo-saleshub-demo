(function ($, kendo) {
    'use strict';

    var customerUrlFormat = "/Customer/Details/";

    var viewModel = kendo.observable({
        selectedCustomerId: null,
        selectedCustomerUrl: "#",
        customerHierarchicalText: "No customer selected",
        updateSelectedCustomer: function (customerId, customerPath) {
            this.set("selectedCustomerId", customerId);
            this.set("selectedCustomerUrl", customerUrlFormat + customerId);
            this.set("customerHierarchicalText", customerPath);
        }
    });

    window.SalesHub.viewModels.headerViewModel = viewModel;
})($, kendo);