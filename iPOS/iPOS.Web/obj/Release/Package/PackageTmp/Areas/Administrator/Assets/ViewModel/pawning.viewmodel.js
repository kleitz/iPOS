﻿app.vm = (function () {
    //"use strict";
    var pawnedItemModel = new app.addPawnedItemModel();

    // #region CONTROLS                
    var isPawnedItemListShowed = ko.observable(true);
    var isManagePawnedItemShowed = ko.observable(false);

    var appraisedItem = ko.observableArray();
    var customer = ko.observableArray();

    // #endregion

    // #region BEHAVIORS
    // initializers
    function activate() {
        //setInfiteScrollGetItemList();
        SetInitialDate();
        getAppraisedItem();
        getCustomer();
    }

    function setInfiteScrollGetItemList() {
        isLoadData = "true";
        inifiniteScroll.init({
            url: RootUrl + "/Administrator//?pageSize=100",
            callback: inifiteScrollCallBackGetItemList,
            searchObject: $("#search-item"),
            page: -1
        });
        inifiniteScroll.getData();
    }

    //Get Data
    function inifiteScrollCallBackGetItemList(result) {
        if (isLoadData === "true") {
            recordCountItemList(result.recordCount);
        }
        isLoadData = "false";

        allItems.removeAll();
        inifiniteScroll.resetDefaults();

        var noMoreData = result.noMoreData;
        var data = result.data;
        var temp = allItems();
        data.forEach(function (o) {
            var appraisalModel = new app.pawnedItemModel(
                o.pawneditemid
            );
            temp.push(appraisalModel);
        });
        isViewLoadMoreData(noMoreData);
        allItems.valueHasMutated();

        return allItems();
    }

    function clearControls() {

    }

    function addItem() {
        isPawnedItemListShowed(false);
        isManagePawnedItemShowed(true);
    }

    function viewItem(arg) {
        loaderApp.showPleaseWait();

        loaderApp.hidePleaseWait();
    }

    function backToAppraisedItemList() {
        isPawnedItemListShowed(true);
        isManagePawnedItemShowed(false);
    }

    function saveItem() {
        /*VALIDATIONS -START*/
        //if (appraisedItem.AppraiseDate().trim() === "") {
        //    toastr.error("Date is required.");
        //    appraisedItem.AppraiseDate("");
        //    document.getElementById("AppraiseDate").focus();
        //    return false;
        //}

        /*VALIDATIONS -END*/

        loaderApp.showPleaseWait();
        var param = ko.toJS(appraisedItem);
        var url = RootUrl + "/Administrator//";
        $.ajax({
            type: 'POST',
            url: url,
            data: ko.utils.stringifyJson(param),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.success) {
                    swal("Success", result.message, "success");

                    loaderApp.hidePleaseWait();
                } else {
                    loaderApp.hidePleaseWait();

                    swal("Error", result.message, "error");

                    clearControls();
                }
            }
        });
    }

    function getAppraisedItem() {
        $.getJSON(RootUrl + "/Administrator/Pawning/GetAppraisedItem", function (result) {
            appraisedItem.removeAll();
            appraisedItem(result);
        });
    }

    function getCustomer() {
        $.getJSON(RootUrl + "/Administrator/Pawning/GetCustomer", function (result) {
            customer.removeAll();
            customer(result);
        });
    }

    function getAppraisedItemById() {
        var AppraisedItemId = $('#AppraiseId').val();
        $.getJSON(RootUrl + "/Administrator/Pawning/GetAppraisedItem?AppraisedItemId=" + AppraisedItemId, function (result) {
            app.appraisedItemModel.removeAll();
            app.appraisedItemModel(result);
        });
    }

    function getCustomerById() {
        var CustomerId = $('#CustomerId').val();
        $.getJSON(RootUrl + "/Administrator/Pawning/GetCustomer?CustomerId=" + CustomerId, function (result) {
            app.customerModel.removeAll();
            app.customerModel(result);
        });
    }

    function SetInitialDate() {
        $('.daterange-single').daterangepicker({
            singleDatePicker: true
        });
    }

    // #endregion

    var vm = {
        activate: activate,
        addItem: addItem,
        saveItem: saveItem,
        viewItem: viewItem,

        isPawnedItemListShowed: isPawnedItemListShowed,
        isManagePawnedItemShowed: isManagePawnedItemShowed,
        backToAppraisedItemList,

        appraisedItem,
        customer,

        getAppraisedItemById,
        getCustomerById
    };

    return vm;

})();

$(function () {
    "use strict";

    app.vm.activate();

    // Success
    $(".control-success").uniform({
        radioClass: 'choice',
        wrapperClass: 'border-success-600 text-success-800'
    });

    //Switchery
    var switches = Array.prototype.slice.call(document.querySelectorAll('.switchery'));
    switches.forEach(function (html) {
        var switchery = new Switchery(html, { color: '#4CAF50' });
    });

    ko.applyBindings(app.vm);
});