$(document).ready(function () {

    //setting up the dateTimePicker
    //            $('#timeFrom').datetimepicker({
    //                minDate: '-1970/01/01',
    //                maxDate: '+1970/01/08',
    //                format: 'd.m.Y H:i'
    //            });
    $('#timeFrom').datetimepicker();

    //sublit button click
    $("#submitButton").click(function () {
        event.preventDefault();
        var params = { TableId: $("#tblId").val(), PeopleCount: $("#peopleCount").val(), TimeFrom: $("#timeFrom").val(), TimeTo: $("#timeTo").val() };
        $.ajax({
            url: "@Url.Action("PlaceOrder", "BookTableController")",
            contentType: "application/json",
        data: JSON.stringify(params),
        type: 'POST',
        cache: false,
        async: true,
        success: function (result) {
            jQuery("#validationPost").css("display", "block");
            jQuery('#validationPost').html(result);
        }
    });
    return false;
});

//check is the date chosen in dateTimePicker is avialable
$(".dateTimePicker").blur(function () {
    var params = { tableId: $("#tblId").val(), time: $(".dateTimePicker").val() };
    $.ajax({
        url: "@Url.Action("IsTableAvialable", "BookTableController")",
        contentType: "application/json",
    data: JSON.stringify(params),
    type: 'POST',
    cache: false,
    async: true,
    success: function (result) {
        if (result.toString().toLowerCase() == "true") {
            $(".glyphicon-ok").css({ color: "green" });
        } else if (result.toString().toLowerCase() == "false") {
            $(".glyphicon-ok").css({ color: "red" });
        }
    }
});
});
});