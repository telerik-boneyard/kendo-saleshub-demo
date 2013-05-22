(function () {
    'use strict';

    var getUrlParams = function () {
        var urlParams = {};
        var match,
            pl = /\+/g, // Regex for replacing addition symbol with a space
            search = /([^&=]+)=?([^&]*)/g,
            decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
            query = window.location.search.substring(1);

        while (match = search.exec(query)) {
            urlParams[decode(match[1])] = decode(match[2]);
        }
        return urlParams;
    };

    var updateGridCustomerFilter = function (customerId) {
        var ordersGrid = $("#ordersGrid").data("kendoGrid");
        ordersGrid.dataSource.filter({ field: "CustomerId", operator: "eq", value: customerId });
    };

    window.SalesHub.CustomerTreeView_Select = function (e) {
        var node = $(e.node);
        var dataItem = e.sender.dataItem(e.node);

        if (!dataItem.hasChildren) {
            window.SalesHub.viewModels.headerViewModel.updateSelectedCustomer(node.data("customer-id"), node.data("path"));
            updateGridCustomerFilter(node.data("customer-id"));
        }
    };

    $(document).ready(function() {
        var selectedCustomerLi, treeView;
        var headerViewModel = window.SalesHub.viewModels.headerViewModel;
        var queryStringParams;

        kendo.bind($("header"), headerViewModel);

        queryStringParams = getUrlParams();

        if ("customerId" in queryStringParams) {
            selectedCustomerLi = $("#customerTreeView li[data-customer-id=" + queryStringParams['customerId'] + "]");
        } else {
            selectedCustomerLi = $("#customerTreeView li[data-customer-id]:first");
        }
        treeView = $("#customerTreeView").data("kendoTreeView");

        $("#createOrderButton").on("click", function() {
            window.location.href = window.SalesHub.createOrderUrl + '/' + headerViewModel.selectedCustomerId;
        });
        treeView.select(selectedCustomerLi);
        treeView.expand(selectedCustomerLi.parent());
        headerViewModel.updateSelectedCustomer(selectedCustomerLi.data("customer-id"), selectedCustomerLi.data("path"));
        updateGridCustomerFilter(selectedCustomerLi.data("customer-id"));
    });
})();