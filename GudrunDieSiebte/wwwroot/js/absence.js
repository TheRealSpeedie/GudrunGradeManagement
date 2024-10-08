const absences = [];
var status = "";
function getAbsences(string) {
    var toast = new MyToast("getAbsences");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Absences/GetAbsencesFromTeacher",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var a = new AbsenceVisible(data[x]);
                    var p = new Person(data[x]["person"]);
                    a.person = p;
                    absences[a.id] = a;
                    status = string;
                    showAbsences();
                }
                toast.showToast("Die Absenzen wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Absenzen konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function dontShowAbsence(id) {
    var toast = new MyToast("dontShowAbsence");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Absences/DontShow/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                if (status == "visible") {
                    $("#table_row_" + id + "_" + status).remove();
                } else {
                    $(".visibility_" + id).html("<i class='las la-eye-slash'></i>");
                    $(".visibility_" + id).attr("onclick", "Show(" + id + ")");
                }
                toast.showToast("Die Absenz wird zukünftig Ihnen nicht mehr angezeigt");
            }
            else {
                toast.showErrorToast("Der Sichtbarkeits Status der Absenz konnte nicht geändert werden.");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
const dates = [];
function showDetails(id) {
    var lessonsFromAbsence = [];
    var personsFromAbsence = [];
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Absences/ShowDetails/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                const EndOptions = { hour: 'numeric', minute: 'numeric' };
                var data = response["data"];
                var a = new Absence(data["absence"]);
                var p = new Person(data["absence"]["person"]);
                a.person = p;
                for (let y = 0; y < data["lessons"].length; y++) {
                    var l = new Lesson(data["lessons"][y]);
                    var p = new Modul(data["lessons"][y]["modul"]);
                    var starttime = new Date(l.starttime);
                    var endtime = new Date(l.endtime);
                    var starttime2 = starttime.toLocaleDateString('de-DE', EndOptions)
                    var endtime2 = endtime.toLocaleDateString('de-DE', EndOptions)
                    l.starttime = starttime2;
                    l.endtime = endtime2;
                    l.modul = p;
                    lessonsFromAbsence.push(l)
                }
                for (let y = 0; y < data["persons"].length; y++) {
                    var t = new Person(data["persons"][y]);
                    personsFromAbsence.push(t)
                }
                var lessons = "";
                lessonsFromAbsence.forEach(function (a) {
                    lessons += "Beim Modul " + a.modul.name + " in der Lektion von " + a.starttime + " bis " + a.endtime
                })
                var person = "";
                personsFromAbsence.forEach(function (a) {
                    person += a.fullname
                })
                var lessons2 = new Converter(lessons).toString();

                var title = a.person.fullname;
                var startabsence = new Date(a.starttime);
                var endabsence = new Date(a.endtime);
                var confirm = "Nein";
                if (a.confirm) {
                    confirm = "Ja"
                }
                var body = `<div class="table">
                                <table class="table table-bordered ">
                                    <tbody>
                                        <tr>
                                            <td >Fehlt von</td>
                                            <td>Von `+ startabsence.toLocaleDateString('de-DE') + ` bis ` + endabsence.toLocaleDateString('de-DE') + `</td>
                                        </tr>
                                        <tr>
                                            <td>Grund</td>
                                            <td>`+ a.reason + `</td>
                                        </tr>
                                        <tr>
                                            <td>Bestätigt</td>
                                            <td>`+ confirm + `</td>
                                        </tr>
                                        <tr>
                                            <td class="WriteOneLine">Betroffene Lektionen</td>
                                            <td>`+ lessons2 + `</td>
                                        </tr>
                                        <tr>
                                            <td>Betroffene Personen</td>
                                            <td>`+ person + `</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>`
                var footer = `Gudrun <img src='../Pictures/icon/32/icon.png'>`
                MyModal.showModal(title, body, footer)

            }
        },
    });
}
function Show(id) {
    var toast = new MyToast("Show");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Absences/Show/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                if (status == "invisible") {
                    console.log(id);
                    $("#table_row_" + id + "_" + status).remove();
                } else {
                    $(".visibility_" + id).html("<i class='las la-eye'></i>");
                    $(".visibility_" + id).attr("onclick", "dontShowAbsence(" + id + ")");
                }
                toast.showToast("Die Absenz wird zukünftig Ihnen angezeigt");
            }
            else {
                toast.showErrorToast("Der Sichtbarkeits Status der Absenz konnte nicht geändert werden.");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}

function showAbsences() {
    var variable = [];
    const StartOptions = { year: 'numeric', month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric' };
    const EndOptions = { hour: 'numeric', minute: 'numeric' };
    var DivId = "";
    if (status == "visible") {
        absences.forEach(function (a) {
            if (a.visible) {
                variable.push(a);
            }
        })
        DivId = "#" + status
    } else if (status == "invisible") {
        absences.forEach(function (a, index) {
            if (!a.visible) {
                variable.push(a)
            }
        })
        DivId = "#" + status
    } else {
        variable = absences;
        DivId = "#absencelist"
    }
    $(DivId).empty();
    variable.forEach(function (a) {
        var startdate = new Date(a.starttime);
        var enddate = new Date(a.endtime);
        var button = "";
        var trash = "";
        if (a.confirm) {
            button = "<button class= 'btn btn-success confirm_" + a.id + "' onclick = 'resettickAbsence(" + a.id + ")'><i class='las la-check-circle'></i></button>"
        } else {
            button = "<button  class= 'btn btn-warning confirm_" + a.id + "' onclick = 'tickAbsence(" + a.id + ")'><i class='las la-circle'></i></button>"
        }
        if (a.visible) {
            trash = "<button class='btn btn-danger visibility_" + a.id + "' onclick='dontShowAbsence(" + a.id + ")'><i class='las la-eye'></i></button>";
        } else {
            trash = "<button id='show_" + a.id + "' class='btn btn-danger visibility_" + a.id + "' onclick='Show(" + a.id + ")'><i class='las la-eye-slash'></i></button>";
        }
        $(DivId).append("<tr id='table_row_" + a.id + "_" + status + "'>"
            + "<td><p class='WriteOneLine'>" + a.person.fullname + "</p></td>"
            + "<td><p class='WriteOneLine'>" + startdate.toLocaleDateString('de-DE', StartOptions) + " - " + enddate.toLocaleDateString('de-DE', EndOptions) + "</p></td>"
            + "<td>" + a.reason + "</td>"
            + "<td>" + "<div class='inOneLine'>" + button + "<button class= 'btn btn-primary' onclick = 'showDetails(" + a.id + ")'><i class='las la-info-circle'></i></button>" + trash + "</div></td>"
            + "<tr>");
    })
}
function tickAbsence(id) {
    var toast = new MyToast("tickAbsence");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Absences/Confirm/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                $(".confirm_" + id).html("<i class='las la-check-circle'></i>");
                $(".confirm_" + id).removeClass("btn-warning");
                $(".confirm_" + id).addClass("btn-success");
                $(".confirm_" + id).attr("onclick", "resettickAbsence(" + id + ")");
                toast.showToast("Die Absenz wurde erfolgreich Bestätigt");
            }
            else {
                toast.showErrorToast("Die Absenz konnte nicht Bestätigt werden");

            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function resettickAbsence(id) {
    var toast = new MyToast("resettickAbsence");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Absences/UnConfirm/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                $(".confirm_" + id).html("<i class='las la-circle'></i>");
                $(".confirm_" + id).addClass("btn-warning");
                $(".confirm_" + id).removeClass("btn-success");
                $(".confirm_" + id).attr("onclick", "tickAbsence(" + id + ")");
                toast.showToast("Die Absenz wurde erfolgreich verneint");
            }
            else {
                toast.showErrorToast("Die Absenz konnte nicht Verneint werden");

            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
