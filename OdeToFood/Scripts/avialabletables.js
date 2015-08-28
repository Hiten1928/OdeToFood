$(document).ready(function () {
    $(".tables").click(function () {
        var tableNumber = $(this).text().substring(7);
        $("#tableNumber").val(tableNumber);
        $("#timeFrom").val($("#timeForAvialability").val());
    });
});