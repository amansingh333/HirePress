$.ajax({
    url: "/api/alljob",
    type: "GET",
    success: function (data) {
        console.log(data);
    },
    error: function (err) {
        alert(err.responseText);
    }
});

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

