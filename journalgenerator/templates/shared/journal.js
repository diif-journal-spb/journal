/* journal specific js goes here */

$(function($){
 $('div.board .editor span.email').replaceWith(function() {
    var url = $.trim($(this).text());
    return '<a href="mailto:' + url + '">' + url + '</a>';
 });
});

(function() {

    var COOKIE_NAME = 'diffjournalCookieConsent';

    function hasConsent() {
        var cookies = document.cookie ? document.cookie.split(';') : [];

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i].replace(/^\s+|\s+$/g, '');

            if (cookie.indexOf(COOKIE_NAME + '=') === 0) {
                return true;
            }
        }

        return false;
    }

    function saveConsent() {
        var maxAge = 60 * 60 * 24 * 365;

        document.cookie =
            COOKIE_NAME +
            '=1; max-age=' +
            maxAge +
            '; SameSite=Lax; path=/';
    }

    $(function() {

        var banner = $('#cookieConsentBanner');

        if (!banner.length) {
            return;
        }

        if (hasConsent()) {
            banner.hide();
            return;
        }

        banner.show();

        $('#cookieConsentAccept').on('click', function() {
            saveConsent();
            banner.hide();
        });

    });

})();

