(function ($) {
    $.fn.paginate = function(options) {
        var pagerSettings    = $.extend({ page: 1, waypoint: 'body' }, options);
        var waypointSettings = { offset: '100%', onlyOnScroll: true };

        var self    = this;
        var loading = $('<div class="alert" style="text-align: center">Loading...</div>');

        self.data('page', pagerSettings['page']);
        self.waypoint(function(event, direction) {
            var page = parseInt(self.data('page'));
            self.waypoint('remove');
            self.append(loading);

            $.get(window.location.href, { page: page })
                .success(function (data) {
                    $(self.data('content')).append(data);
                    self.waypoint(waypointSettings);
                })
                .complete(function () {
                    loading.detach();
                });

            self.data('page', page + 1);
        }, waypointSettings);
    }
})(jQuery);