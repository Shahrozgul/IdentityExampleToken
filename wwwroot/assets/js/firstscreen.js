$(".divCategory").click(function () {
    $('.divCategory').removeClass('divCategoryActive');
    var divId = $(this).attr('id');
    $("#divProduct").load("ProductPartial?categoryId=" + divId);
    $(this).addClass('divCategoryActive');
});
$(function () {
    //The passed argument has to be at least a empty object or a object with your desired options
    //$("body").overlayScrollbars({ });
    $("#items").height(552);
    $("#items").overlayScrollbars({
        overflowBehavior: {
            x: "hidden",
            y: "scroll"
        }
    });
    $("#cart").height(445);
    $("#cart").overlayScrollbars({});
});
function deleteRow(elem) {
    var row = elem.parentNode.parentNode;
    row.parentNode.removeChild(row);
    calculateTable();
}
function addRow(productId, productName, price) {
    var uniqueElementId = PseudoGuid.GetNew();
    $('#myTable tbody').prepend("<tr id='" + uniqueElementId + "'><td class='text-center align-middle'><figure class='media'><input id='hdnProductId_" + uniqueElementId + "' type='hidden' value='" + productId + "' /><figcaption class='media-body'> <h6 class='title text-truncate'>" + productName + "</h6> </figcaption></figure></td><td class='text-center align-middle'><div class='m-btn-group m-btn-group--pill btn-group mr-2' style='width:100%;'> <button type='button' class='m-btn btn btn-default ' style='width:100%;' id='btnMinus_" + uniqueElementId + "' onclick='minusQuantity(this)'><i class='fa fa-minus'></i></button><input type='number' min='1' step='any' class='m-btn btn btn-default' id='txtQty_" + uniqueElementId + "' value='1' style='width:100px;' onblur='ClearZero(this)' onfocusout='PutZero(this)' /><button type='button' class='m-btn btn btn-default' id='btnAdd_" + uniqueElementId + "' onclick='addQuantity(this)' style='width:100%;'><i class='fa fa-plus '></i></button> </div> </td> <td class='text-center align-middle'> <div class='price-wrap price' id='divRate_" + uniqueElementId + "'>" + price + "</div></td><td class='text-center align-middle'><div class='price-wrap price' id='divTotal_" + uniqueElementId + "'>" + price + "</div></td><td class='text-center align-middle'><button onclick='deleteRow(this)' class= 'btn btn-outline-danger btn-round'><i class='fa fa-trash' ></i></button></td ></tr >");
    calculateTable();
}
function reset() {
    $("#myTable tr").remove();
    calculateTable();
}
function submitBill(status) {
    var rowCount = $('#myTable TBODY TR').length;
    if (rowCount !== 0) {
        var listBillDetails = new Array();
        $("#myTable TBODY TR").each(function () {
            var rowId = $(this).attr('id');
            var i = {};
            i.productId = document.getElementById('hdnProductId_' + rowId).value;
            i.productRate = document.getElementById('divRate_' + rowId).innerText;
            i.productQty = document.getElementById('txtQty_' + rowId).value;
            i.paymentStatus = status;
            i.status = status;
            i.taxPercentage = document.getElementById('ddTaxPercentage').textContent.replace('%', '');
            i.discountPercentage = document.getElementById('ddDiscountPercentage').textContent.replace('%', '');
            listBillDetails.push(i);
        });
        $.ajax({
            url: '/api/FirstScreenApi/pos/SubmitBill',
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(listBillDetails),
            success: function (data, status, xhr) {
                console.log(JSON.stringify(data));
                showBill(data);
                reset();
                //console.log(status);
                //console.log(xhr);                              
            },
            error: function (jqXhr, textStatus, errorMessage) {
                //console.log(jqXhr);
                //console.log(textStatus);
                //console.log(errorMessage);
            }
        });
    }
}
function showBill(data) {
    document.getElementById("spanTokenNumber").textContent = data.billHeader.billNumber;
    document.getElementById("spanDate").textContent = new Date(data.billHeader.createdDate).toLocaleString();
    $("#billTBody tr").remove();
    $.each(data.billHeader.billDetails, function (i, item) {
        $('#billTBody').append('<tr><td class="text-center align-middle">' + item.product.productName + ' - ' + item.product.category.categoryName + '</td><td class="text-center align-middle">' + item.productQty + '</td><td class="text-center align-middle">' + item.productRate + '</td><td class="text-center align-middle">' + item.productQty * item.productRate + '</td><td class="text-center align-middle">' + item.paymentStatus + '</td></tr>');
    });
    $("#receipt").modal();
}
function printBill(billNumber) {
    if (billNumber === 0) {
        billNumber = 0;
    } else if (billNumber === -1) {
        billNumber = parseInt(document.getElementById("spanTokenNumber").textContent) - 1;
    }
    else if (billNumber === 1) {
        billNumber = parseInt(document.getElementById("spanTokenNumber").textContent) + 1;
    }
    if (billNumber >= 0) {
        $.ajax({
            url: '/api/FirstScreenApi/pos/GetBill?billNumber=' + billNumber,
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
            /*data: JSON.stringify(parseInt(billNumber)),*/
            success: function (data, status, xhr) {
                console.log(JSON.stringify(data));
                showBill(data);
                //console.log(status);
                //console.log(xhr);                              
            },
            error: function (jqXhr, textStatus, errorMessage) {
                //console.log(jqXhr);
                //console.log(textStatus);
                //console.log(errorMessage);
            }
        });
    }
}
function addAllColumnHeaders(myList) {
    var columnSet = [];
    var headerTr$ = $('<tr/>');
    for (var i = 0; i < myList.length; i++) {
        var rowHash = myList[i];
        alert(rowHash);
        for (var key in rowHash) {
            if ($.inArray(key, columnSet) == -1) {
                columnSet.push(key);
                headerTr$.append($('<th/>').html(key));
            }
        }
    }
    $("#av").append(headerTr$);
    return columnSet;
}
function minusQuantity(o) {
    var id = o.id.replace('btnMinus_', '');
    setQuantity(id, -1)
}
function addQuantity(o) {
    var id = o.id.replace('btnAdd_', '');
    setQuantity(id, 1)
}
function setQuantity(id, number) {
    var txtQty = document.getElementById('txtQty_' + id);
    var currentValue = parseFloat(txtQty.value);
    if (currentValue + number != 0) {
        txtQty.value = currentValue + number;
    }
    calculateTable();
}
function calculateTable() {
    var subTotal = 0.00;
    $("#myTable TBODY TR").each(function () {
        var rowId = $(this).attr('id');
        var total = 0.00;
        var txtQty = document.getElementById('txtQty_' + rowId);
        if (txtQty.value) {
            var divRate = document.getElementById('divRate_' + rowId);
            var divTotal = document.getElementById('divTotal_' + rowId);
            total = (parseFloat(txtQty.value) * parseFloat(divRate.innerText));
            divTotal.innerText = parseFloat(total).toFixed(2);
            subTotal += total;
        }
    });
    var ddSubTotal = document.getElementById('ddSubTotal');
    ddSubTotal.innerText = parseFloat(subTotal).toFixed(2);
    var ddTaxPercentage = document.getElementById('ddTaxPercentage');
    var ddTaxAmount = document.getElementById('ddTaxAmount');
    ddTaxAmount.innerText = parseFloat((subTotal * ddTaxPercentage.innerText.replace('%', '')) / 100).toFixed(2);
    var ddDiscountPercentage = document.getElementById('ddDiscountPercentage');
    var ddDiscountTotal = document.getElementById('ddDiscountTotal');
    ddDiscountTotal.innerText = parseFloat((subTotal * ddDiscountPercentage.innerText.replace('%', '')) / 100).toFixed(2);
    var ddGrandTotal = document.getElementById('ddGrandTotal');
    ddGrandTotal.innerText = parseFloat((parseFloat(subTotal) + parseFloat(ddTaxAmount.innerText)) - parseFloat(ddDiscountTotal.innerText)).toFixed(2);
}