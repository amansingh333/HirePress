function AddSkill() {
    var skill = $('#skill').val();
    var oldskills = $('#textarea').val();
    var skills = oldskills + skill + ',';
    $('#textarea').val(skills);
    $('#skill').val('');
}
function SaveSkillsTypeData() {
    var skillTypeData = $('textarea').val().slice(0, -1);
    $.ajax({
        url: "/API/SetSkillsTypeData?SkillType=" + $('#skill_type').find(':selected').val() + "&SkillTypeData=" + skillTypeData,
        type: "POST",
        success: function (data) {
            if (data) {
                Swal.fire('Skill Data for (' + $('#skill_type').find(':selected').val() + ') Updated Successfully!');
                $('textarea').val('');
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}