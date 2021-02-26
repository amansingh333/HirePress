
var currStep = 1;
var lastStep = 1;

var jobSkills = ['Angular','React','Vue']

$('.step-2').hide()
$('.step-3').hide()
$('.step-4').hide()





$(".step-1-next").click(function ($event) {
    lastStep = currStep;
    currStep++
    let hide = '.step-' + lastStep;
    let show = '.step-' + currStep;

    $(hide).hide()
    $(show).show()
});

$(".onBack").click(function ($event) {
    lastStep = currStep;
    currStep--
    let show = '.step-' + currStep;
    let hide = '.step-' + lastStep;

    $(hide).hide()
    $(show).show()
});

$(".role-btn").click(function ($event) {
    console.log($event.target.innerText)
});
