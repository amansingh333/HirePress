
$.ajax({
    url: "/api/alljob",
    type: "GET",
    success: function (data) {
        console.log(data);
        generateJobs(data)
    },
    error: function (err) {
        alert(err.responseText);
    }
});

function generateJobs(data) {
    data.forEach(e => {
        const payload = ` <div class="job-listings">
                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-xs-12">
                                <div class="job-company-logo">
                                    <img src="~/Theme/homepage-assets/assets/img/features/img1.png" alt="">
                                </div>

                            </div>
                            <div class="col-lg-9 col-md-9 col-xs-12 text-left pl-4">
                                <div class="job-details">
                                    <h3>${e.JobTitle}</h3>
                                    <div class="location mt-0">
                                        <i class="lni-map-marker"></i> ${e.Location}
                                    </div>
                                </div>
                                <p>

                                    ${e.Description.slice(0,200)}

                                </p>
                            </div>
                            <div class="col-lg-2 col-md-2 col-xs-12 text-center job-posting-action">
                                <a href="${e.JobURL} class="btn-apply"><span class="btn-apply">View</span> </a><br>
                                <span class="btn-full-time">Not intrested</span>
                            </div>
                        </div>
                    </div>`
        $('.jobs-wrapper').append(payload)
    })
}

$(document).ready(() => { 
    CreateCityDropdown();
});

function CreateCityDropdown() {
    
    $.getJSON('Json/cities.json', function (data) {
        $('#city_dropdown').html('');
        $('#city_dropdown').append('<input type="text" class="form-control" id="txt-search" name="SearchCity" list="city" placeholder="Location: City, State, Zip" autocomplete="off">');
        var datalist = '<datalist id="city">';
        $.each(data, function (key, val) {
            datalist += '<option value="' + val.name + '">';
        });
        datalist += '</datalist>';
        $('#city_dropdown').append(datalist);
    }).fail(function (err) { alert(err.responseText); });
    
}

