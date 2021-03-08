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