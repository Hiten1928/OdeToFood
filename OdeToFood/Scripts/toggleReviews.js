(function toggleReview($) {
    $(".restaurantTr").click(function () {
        //alert(this.nextElementSibling);
        $(this).next().toggle(200);

    });
})(jQuery);