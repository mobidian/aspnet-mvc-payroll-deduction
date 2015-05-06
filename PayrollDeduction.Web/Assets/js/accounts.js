(function($) {
    var Accounts = {
        commit: function(account, balance, callback) {
            $.post('/Admin/Accounts/Commit', { id: account, balance: balance }, function(result) {
                callback(result);
            });
        }
    }

    $().ready(function() {
        $('#accounts button.commit').click(function() {
            var account = $(this).attr('data-account');
            var balance = $(this).attr('data-balance');

            Accounts.commit(account, balance, function() {
                $('tr[data-account=' + account + ']').hide();
            });
        });

        $('#accounts button.ignore').click(function() {
            var account = $(this).attr('data-account');
            $('tr[data-account=' + account + ']').hide();
        });
    });
})(jQuery);