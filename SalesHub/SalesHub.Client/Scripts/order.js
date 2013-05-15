window.SalesHub.OrderDetailsGrid_Save = function(e) {
    e.model.set("TotalAmount", e.model.Units * e.model.PricePerUnitOfWeight);
};

window.SalesHub.OrderDetailsEdit_NetWeight_Change = function (e) {
    var grid = $("#orderDetailsGrid").data("kendoGrid");
    var orderDetails = grid.editable.options.model;
    if (orderDetails.UnitWeight !== 0) {
        orderDetails.set("Units", orderDetails.NetWeight / orderDetails.UnitWeight);
    }
};

window.SalesHub.OrderDetailsEdit_UnitWeight_Change = function (e) {
    var grid = $("#orderDetailsGrid").data("kendoGrid");
    var orderDetails = grid.editable.options.model;
    orderDetails.set("NetWeight", orderDetails.UnitWeight * orderDetails.Units);
};

window.SalesHub.OrderDetails_Error = function (args) {
    if (args.errors) {
        var grid = $("#orderDetailsGrid").data("kendoGrid");
        var validationTemplate = kendo.template($("#orderDetailsValidationMessageTemplate").html());
        grid.one("dataBinding", function (e) {
            e.preventDefault();

            $.each(args.errors, function (propertyName) {
                var renderedTemplate = validationTemplate({ field: propertyName, messages: this.errors });
                grid.editable.element.find(".errors").append(renderedTemplate);
            });
        });
    }
};

window.SalesHub.SuggestedValuesGrid_Changed = function (e) {
    var selectedRow = this.select()[0];

    $("#PaymentTermsOverride").val($(selectedRow).find("td").text());
    $("#suggestedValuesWindow").data("kendoWindow").close();
};

$(document).ready(function () {
    var viewModels = window.SalesHub.viewModels;
    var customerData = window.SalesHub.customerData;

    kendo.bind($("header"), viewModels.headerViewModel);

    viewModels.headerViewModel.updateSelectedCustomer(customerData.customerId, customerData.customerPath);

    $(".actionButton").on("click", function (e) {
        window.location.href = $(e.target).data("action");
    });

    $("#delete").on("click", function() {
        $("#deleteForm").submit();
    });

    $("#suggestedValueButton").on("click", function (e) {
        var suggestedValuesWindow = $("#suggestedValuesWindow").data("kendoWindow");

        suggestedValuesWindow.center();
        suggestedValuesWindow.open();
    });
});