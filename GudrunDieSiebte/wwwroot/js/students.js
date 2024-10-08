function getClassmates() {
    var toast = new MyToast("getStudent");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Students/GetClassmates",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#studentlist").empty()
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var student = new Student(data[x]);
                    var cl = new Class(data[x]["class"]);
                    $("#studentlist").append("<tr id='table_row_" + data[x]["id"] + "'>"
                        + "<td>" + cl.name + "</td>"
                        + "<td> " + student.firstname + "</td>"
                        + "<td>" + student.lastname + "</td>"
                        + "<tr>");
                }
                toast.showToast("Die Schüler wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Schüler konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function deleteStudent(id) {
    var toast = new MyToast("deleteStudent");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Students/Delete/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#table_row_" + id).remove();
                toast.showToast("Der Schüler wurde erfolgreich gelöscht");
            }
            else {
                toast.showErrorToast("Der Schüler konnte nicht gelöscht werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}