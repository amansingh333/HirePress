//onclick method on each skill type button
function GetSkills(a) {
    //remove btn-primary class from all and ass btn-border class
    for (let i = 0; i < $('.role-btn').length; i++) {
        $('.role-btn').removeClass('btn-primary').addClass('btn-border');
    }

    //add btn-primary class and remove btn-border class from selected button
    $(a).addClass('btn-primary').removeClass('btn-border');

    //make an ajax call to find selected skill's skill types
    $.ajax({
        url: "/api/skills?skilltype=" + a.innerText,
        type: "GET",
        success: function (data) {
            //empty skills column
            $('#skills_data').html('');

            //add skills using data returned by the api call
            for (let i = 0; i < data.length; i++) {
                $('#skills_data').append('<button type="button" class="btn btn-border btn-pills waves-effect waves-themed skills-btn">' + data[i] + '</button>');
            }

            //show the hidden skills column
            $('.step-2').show();

            //select or unselect multiple skills
            $('.skills-btn').click((a) => {
                if ($(a.currentTarget).hasClass('btn-border'))
                    $(a.currentTarget).addClass('btn-primary').removeClass('btn-border');
                else
                    $(a.currentTarget).removeClass('btn-primary').addClass('btn-border');

                //check if any one skill is selected or not to disable or enable next button
                var flag = false;
                for (let i = 0; i < $('.skills-btn').length; i++) {
                    if ($('.skills-btn').hasClass('btn-primary')) {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    $('.onStep1').prop('disabled', '');
                else
                    $('.onStep1').prop('disabled','disabled');
            });
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}

//goto upload panel
function UploadPanel() {
    $('.step-1').hide();
    $('.step-3').show();
}

//back to skill panel
function SkillPanel() {
    $('.step-1').show();
    $('.step-3').hide();
}

//switch between upload and qualification panel
function QualificationPanel(a) {
    if (a.innerText === "NEXT") {
        $('.step-3').hide();
        $('.step-4').show();
    }
    else {
        $('.step-4').hide();
        $('.step-3').show();
    }
}

//functionality to enable or disable Get Hired button using onchange and onkeyup method
function CheckQualificationFields() {
    if ($('#inputGroupSelect04').find(':selected').val() !== "Select Degree") {
        if ($('#College').val() !== "") {
            if ($('#Specialization').val() !== "") {
                if ($('#Graduation_Year').val() !== "") {
                    $('.onStep3').prop('disabled', '');
                }
                else
                    $('.onStep3').prop('disabled', 'disabled');
            }
            else
                $('.onStep3').prop('disabled', 'disabled');
        }
        else
            $('.onStep3').prop('disabled', 'disabled');
    }
    else
        $('.onStep3').prop('disabled', 'disabled');
}

//create data in json and send to server and successfully redirect to candidate home page 
function SaveInformation() {
    var jobrole = "";
    var skills = [];

    //get job role
    for (let i = 0; i < $('.role-btn').length; i++) {
        if ($($('.role-btn')[i]).hasClass('btn-primary'))
            jobrole = $($('.role-btn')[i]).text();
    }
    //get all skills
    for (let i = 0; i < $('.skills-btn').length; i++) {
        if ($($('.skills-btn')[i]).hasClass('btn-primary'))
            skills.push($($('.skills-btn')[i]).text());
    }
    //create json
    var json = {
        "JobRole": jobrole,
        "Skills": skills,
        "Qualification": {
            "Degree": $('#inputGroupSelect04').find(':selected').text(),
            "College": $('#College').val(),
            "Specialization": $('#Specialization').val(),
            "GraduationYear": $('#Graduation_Year').val()
        }
    };
    //redirect to login page
    location.href = "/Account/Login";
}



