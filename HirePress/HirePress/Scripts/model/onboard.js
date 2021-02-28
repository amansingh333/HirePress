

$(document).ready(() => {


    $.ajax({
        url: "API/TestAPI?GetSkillsTypeData=Frontend",
        type: "GET",
        success: function (data) {
            return data
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
  
var payload = {
    step1: {
        job_role: '',
        skills: []
    },
    step2: {
        cv: ''
    }
}


localStorage.setItem('curr_step', 1);
localStorage.setItem('payload', JSON.stringify(payload));

var currStep = 1;
var lastStep = 1;

    var Frontend = ['Angular', 'React', 'Vue', 'Javascript', 'Html', 'Css', 'Scss', 'Bootstrap', 'Foundation', 'Typescript' ]
    var Backend = ['Python', 'NodeJS', 'Java', 'PHP']


$('.step-2').hide()
$('.step-3').hide()
$('.step-4').hide()
$('.step-1-next').addClass('disabled')


$(".onStep1").click(function ($event) {
    console.log($event)
    $('.step-1').hide()
    $('.step-2').hide()
    $('.step-3').show()
})



$(".onStep2").click(function ($event) {
    console.log($event)
    $('.step-3').hide()
    $('.step-4').show()

});

$(".backToStep1").click(function ($event) {
    $('.step-3').hide()
    $('.step-1').show()
    $('.step-2').show()

});

$(".backToStep2").click(function ($event) {
    console.log($event)
    $('.step-4').hide()
    $('.step-3').show()

});

// On cv upload

$(".input-file").click(function ($event) {
    console.log($event)
    var myFile = $('#fileinput').prop('files');
    console.log(myFile)
});

  

$(document).on('click', '.skills-btn', function ($event) {
        if (($event.currentTarget).classList.contains('selected-btn')) {
            ($event.currentTarget).classList.remove('selected-btn')
            payload.step1.skills.forEach((e,index) => {
                console.log(e)
                if (e == $event.target.innerText) {
                    payload.step1.skills.splice(index, 1)
                    localStorage.setItem('payload', JSON.stringify(payload));
                }
            })
        } else {
            $($event.currentTarget).addClass('selected-btn');
            payload.step1.skills.push($event.target.innerText);
            localStorage.setItem('payload', JSON.stringify(payload));
        }

    $('.step-1-next').removeClass('disabled')

    console.log(payload)
});

$(".role-btn").click(($event) => {
    document.querySelectorAll(".role-btn").forEach(e => {
        
        if (e.classList.contains('selected-btn')) {
            console.log(e)
            $('#skills_data').empty()
            e.classList.remove('selected-btn');
        }
    })



    fetch('https://localhost:44371/API/TestAPI?GetSkillsTypeData=Frontend')
        .then(data => data.json())
        .then(res => {
            console.log(res)
        })

    $($event.currentTarget).addClass('selected-btn');
    payload.step1.job_role = $event.target.innerText;
    localStorage.setItem('payload', JSON.stringify(payload));
    for (let i = 0; i < Frontend.length; i++) {
        $('#skills_data').append('<button type="button" class="btn btn-light btn-pills waves-effect waves-themed skills-btn">' + Frontend[i] + '</button>');
    }

    currStep++
    $('.step-2').show();


    console.log(payload)
});




});


