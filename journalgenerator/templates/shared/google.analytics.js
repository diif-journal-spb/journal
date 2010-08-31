
if (document.location.host.indexOf('neva.ru') >= 0) {
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-17795545-2']);
  _gaq.push(['_setDomainName', '.neva.ru']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

  setTimeout(function() { window.location="http://www.math.spbu.ru/diffjournal/j"; }, 300);
}

if (document.location.host.indexOf('spbu.ru') >= 0) {
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-17795545-1']);
  _gaq.push(['_setDomainName', '.neva.ru']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
}

