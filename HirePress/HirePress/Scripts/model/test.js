$(document).ready(() => {
    $.ajax({
        url: "/API/GetSkillsTypeData?SkillType=Frontend",
        type: "GET",
        success: function (data) {
            debugger;
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
});