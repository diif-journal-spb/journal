/* journal specific js goes here */

$(function($){
 $('div.board .editor span.email').replaceWith(function() {
    var url = $.trim($(this).text());
    return '<a href="mailto:' + url + '">' + url + '</a>';
 });
});

(function() {
    var COOKIE_NAME = 'diffjournalCookieConsent';
    var METRIKA_ID = 70526236;
    var metrikaInitialized = false;

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
            COOKIE_NAME + '=1; max-age=' + maxAge +
            '; SameSite=Lax; path=/';
    }

    function initYandexMetrika() {
        if (metrikaInitialized) {
            return;
        }

        metrikaInitialized = true;

        (function(m,e,t,r,i,k,a){
            m[i]=m[i]||function(){
                (m[i].a=m[i].a||[]).push(arguments);
            };

            m[i].l=1*new Date();

            for (var j = 0; j < document.scripts.length; j++) {
                if (document.scripts[j].src === r) {
                    return;
                }
            }

            k=e.createElement(t);
            a=e.getElementsByTagName(t)[0];
            k.async=1;
            k.src=r;
            a.parentNode.insertBefore(k,a);

        })(window, document, 'script',
            'https://mc.yandex.ru/metrika/tag.js', 'ym');

        ym(METRIKA_ID, 'init', {
            webvisor: true,
            trackHash: true,
            clickmap: true,
            ecommerce: "dataLayer",
            referrer: document.referrer,
            url: location.href,
            accurateTrackBounce: true,
            trackLinks: true
        });
    }

    $(function() {
        var banner = $('#cookieConsentBanner');

        /*
         * Согласие уже было дано при предыдущем посещении.
         * Запускаем Метрику и не показываем баннер.
         */
        if (hasConsent()) {
            initYandexMetrika();

            if (banner.length) {
                banner.hide();
            }

            return;
        }

        /*
         * Согласия еще нет.
         * Метрику НЕ запускаем.
         */
        if (!banner.length) {
            return;
        }

        banner.show();

        $('#cookieConsentAccept').on('click', function() {
            saveConsent();
            banner.hide();

            /*
             * Запускаем Метрику непосредственно после получения согласия.
             */
            initYandexMetrika();
        });
    });
})();



