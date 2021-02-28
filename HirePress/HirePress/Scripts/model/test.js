$(document).ready(() => {


    $.ajax({
        url: "API/?GetSkillsTypeData=Frontend",
        type: "GET",
        success: function (data) {
            return data
        },
        error: function (err) {
            alert(err.responseText);
        }
    });

})