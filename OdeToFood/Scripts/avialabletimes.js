$(document).ready(function () {
    $(".avialableTime").click(function () {
        $(".dateTimePicker").val($(this).text());
    });
});