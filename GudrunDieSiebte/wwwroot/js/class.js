function getClasses() {
    var toast = new MyToast("getClasses");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/getClassesWithStudentTeacherModul",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#classlist").empty()
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var students = "<ul>";
                    var moduls = "<ul>";
                    for (var y = 0; y < data[x]["students"].length; y++) {
                        students += "<li>" + data[x]["students"][y]["studentname"];
                    }
                    for (var y = 0; y < data[x]["moduls"].length; y++) {
                        moduls += "<li>" + data[x]["moduls"][y]["modulname"];
                    }
                    $("#classlist").append("<tr id='table_row_" + data[x]["id"] + "'>"
                        + "<td>" + data[x]["classname"] + "</td>"
                        + "<td>" + data[x]["teachername"] + "</td>"
                        + "<td>" + students + "</td>"
                        + "<td>" + moduls + "</td>"
                        + "<tr>");
                }
                toast.showToast("Die Klassen wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Klassen konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
