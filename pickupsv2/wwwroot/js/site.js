// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#login-button").click(function () {
    $("#quick-login").toggle();
    $("#quick-register").hide();
});
$("#register-button").click(function () {
    $("#quick-register").toggle();
    $("#quick-login").hide();
});