(function toggleReview($) {
    $(".restaurantTr").click(function () {
        //alert(this.nextElementSibling);
        var self = $(this);
        while (self.next().hasClass("reviewsTr")) {
            self.next().toggle(200);
            self = self.next();
        }

    });
})(jQuery);