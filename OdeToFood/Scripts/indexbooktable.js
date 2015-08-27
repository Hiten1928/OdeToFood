jQuery(document).ready(function () {

    var date = new Date();
    $("#timeForAvialability").val(date.dateFormat("Y/m/d") + " " + date.toTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1"));

    $("#timeForAvialability").datetimepicker();
    $("#datePicker").datetimepicker({timepicker:false, format:'d.m.Y'});

    $("#timeForAvialability").blur(function () {
        var model = @Html.Raw(Json.Encode(Model.Id));
        var model = @Html.Raw(Json.Encode(Model.Id));
        var params = { time: $(this).val(), restaurantId : model };
        $.ajax({
            url: "@Url.Action("GetAvialableTables", "BookTableController")",
            contentType: "application/json",
            data: JSON.stringify(params),
            type: 'POST',
            cache: false,
            async: true,
            success: function (result) {
                $('#avialableTableList').html(result);
            }
        });
    });


var tableClickedId;
//on table number click event
$('span.glyph').click(function () {
    tableClickedId = $(this).data("tableid");
    //getting create order form
    $.ajax({
        url: $(this).data('url'),
        type: 'GET',
        cache: false,
        async:true,
        success: function (result) {
            $('.msmqpartial').html(result);
        }
    });

    //getting all the times avialable for the table
    $("#freeTimePanel").css("display", "block");
    var params = { tableId: $(this).data("tableid"), date: new Date() };
    $.ajax({
        url: "@Url.Action("GetAvialableTimes", "BookTableController")",
        contentType: "application/json",
    data: JSON.stringify(params),
    type: 'POST',
    cache: false,
    async:true,
    success: function (result) {
        $("#avialableTimeList").html(result);
    }
});

//all avialable times for the table if the date changes
$("#datePicker").val(new Date().dateFormat("Y.m.d"));
$("#datePicker").blur(function () {
    var params = { tableId: tableClickedId, date: $(this).val() };
    $.ajax({
        url: "@Url.Action("GetAvialableTimes", "BookTableController")",
        contentType: "application/json",
    data: JSON.stringify(params),
    type: 'POST',
    cache: false,
    success: function (result) {
        $("#avialableTimeList").html(result);
    }
});
});
return false;
});



//table click effect
$(".glyph").click(function () {
    $(".glyph").css("color", "#444");
    $(this).css("color", "white");
});

});