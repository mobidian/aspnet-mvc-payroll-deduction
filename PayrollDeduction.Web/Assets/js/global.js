(function($) {
    $().ready(function() {
        $('a[data-method=post]').live('click', function() {
            var form = document.createElement('form');
            form.style.display = 'none';
            form.method = 'post';
            form.action = this.href;
            document.body.appendChild(form);
            form.submit();
            return false;
        });

        $('.pagination').paginate();
    });
})(jQuery);