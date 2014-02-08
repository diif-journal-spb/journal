/* journal specific js goes here */

$(function($){
 $('div.board .editor span.email').replaceWith(function() {
    var url = $.trim($(this).text());
    return '<a href="mailto:' + url + '">' + url + '</a>';
 });
});

