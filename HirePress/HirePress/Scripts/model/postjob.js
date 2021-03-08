var json = {};


//Submit Button handler
$('#submit').click(() => {
    getJson()
});



function getJson() {
    json = {
        JobTitle: $('input[name=JobTitle]').val(),
        Company: $('input[name=Company]').val(),
        Location: $('input[name=Location]').val(),
        Category: $('select[name=Category]').find(':selected').text(),
        Education: $('input[name=Education]').val(),
        JobTags: $('input[name=JobTags]').val(),
        ApplicationEmail: $('input[name=ApplicationEmail]').val(),
        ClosingDate: $('input[name=ClosingDate]').val(),
        CompanyName: $('input[name=CompanyName]').val(),
        Website: $('input[name=Website]').val(),
        Tagline: $('input[name=Tagline]').val(),
        Description: $('.note-editable').html()
    };

}



//for creating previewModal
var div=""
$('#preview').click(() => {
    getJson()

    div += `<div class="modal fade mt-5" id="previewModal" tabindex="-1" aria-labelledby="previewModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg modal-dialog-scrollable">
                <div class="modal-content">
                  <div class="modal-header">
                    <h6  id="previewModalLabel">${json.JobTitle}</h6>
                    
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                    
                  </div>
             
                  <div class="modal-body">
                    
                    <div class="card">
                      <div class="card-header">
                        <h6>${json.Company}</h6>
                      </div>
                      <div class="card-body">
                        
                        <!--<h6>Special title treatment</h6>-->
                        <p class="card-text">Experience   : ${json.Location}</p>
                        <p class="card-text">Stipend      : ${json.Location}</p>
                        <p class="card-text">Location     : ${json.Location}</p>
                        <p class="card-text">Closing Date : ${json.ClosingDate}</p>                 
                      </div>
                    </div>

                     
                    <div class="card">
                      <div class="card-header">
                        Job Description
                      </div>
                      <div class="card-body">
                        <h6 class="card-title">Responsibility</h6>
                        <p class="card-text">${json.Description}</p>
                        <h6 class="card-title">Skills And Qualification</h6>
                        <p class="card-text">${json.Tagline}</p>
                        <h6 class="card-title">Education</h6>
                        <p class="card-text">${json.Education}</p>
                        <h6 class="card-title">Key Skills</h6>
                        <p class="card-text">${json.JobTags}</p>
                      </div>
                    </div>

                    <div class="card">
                      <h6 class="card-header">Company Detail</h6>
                      <div class="card-body">
                        <h6 class="card-title">About Company</h6>
                        <p class="card-text">{about Company Detail}</p>
                        <a href="#" class="btn btn-primary">Go somewhere</a>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" id="close" data-dismiss="modal" >Close</button>
                    
                  </div>
                </div>
              </div>
            </div>`
    $('#pre').append(div);

});


//for enable/disable the previewmodal button
function manage() {
    var bt = document.querySelector('#preview')
    console.log(jobTitleId.value, companyId.value, locationId.value)
    if (((jobTitleId.value != "") && (companyId.value != "")) && (locationId.value != "")) {
        bt.disabled = false;
    } else {
        
        bt.disabled = true;
    }
}





