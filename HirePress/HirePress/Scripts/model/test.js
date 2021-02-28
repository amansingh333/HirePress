$(document).ready(() => {
    $.ajax({
        url: "/api/skills?skilltype=Frontend",
        type: "GET",
        success: function (data) {
            debugger;
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
});