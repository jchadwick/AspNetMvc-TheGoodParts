$(function () {
    $('#search .search-query').data('source', function (query, callback) {
        var category;
        category = $('#search [name=category]').val();
        return $.getJSON('/autocomplete', {
            query: query,
            category: category
        }, callback);
    });
});