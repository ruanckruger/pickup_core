// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$("#game-list").change(function () {
    console.log($(this).find("option:selected").attr('value'));
    $('#map-list-container').load("/Match/_MapListPartial?gameId=" + $(this).find("option:selected").attr('value'));
});

$(".admin-features").click(function () {
    let dropDownContainer = $('.dropdown-container');
    if (dropDownContainer.hasClass('open')) {
        dropDownContainer.removeClass('open');
    } else {
        dropDownContainer.addClass('open');
    }
});