$(document).ready(function () {
    'use strict';

    var viewModels = window.SalesHub.viewModels;
    var customerData = window.SalesHub.customerData;

    kendo.bind($("header"), viewModels.headerViewModel);

    viewModels.headerViewModel.updateSelectedCustomer(customerData.customerId, customerData.customerPath);
});