$(document).ready(function () {
    showNotification()
    const path = window.location.pathname;
    switch (path) {
        case "/Student":
            getClassmates();
            break;
        case "/Teacher":
            getAllTeachers(updateTeacherTable);
            break;
        case "/Student/Teacherlist":
            getMyTeachers();
            break;
        case "/Teacher/TeachingCompany":
            getTeachingCompany(updateTable);
            break;
        case "/Teacher/Absence":
            getAbsences('visible');
            break;
        case "/Student/Classes":
            getClasses();
            break;
        case "/Admin/TeachingCompany":
            getTeachingCompany(updateAdminTable);
            break;
        case "/Student/Module":
            GetMyModuls();
            break;
        case "/Important/Grades":
            getMyGrades();
            break;
        case "/Important/Testimony":
            getMyTestimoiny();
            break;
        case "/General/Modullist":
            getModulStructure();
            break;
        case "/General/Teacherlist":
            getAllTeachers(updateTeacherTable);
            break;
        case "/General/TeachingCompanylist":
            getTeachingCompany(updateTeachingCompanyTable);
            break;
        case "/Teacher/Grade":
            getGradesModulsFromClass();
            fillTreeGrade();
            break;
        case "/Student/ToDo":
            getTodo("notDone");
            break;
        default:
            break;
    }
    const knownLink = "/General/Modullist/";
    const regexPattern = /M[0-9]{3}/;
    const currentLink = window.location.href;
    if (currentLink.includes(knownLink)) {
        const unknownText = currentLink.split(knownLink)[1];
        if (regexPattern.test(unknownText)) {
            getModulStructure(unknownText);
        } else {
            getModulStructure();
        }
    }
});

function getVersion() {
    var toast = new MyToast("getVersion");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Home/Version",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#version").empty()
                var data = response["data"];
                $("#version").text(data);
            }
            else {
                toast.showErrorToast("Die Version konnten nicht Angeziegt werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}

function showNotification() {
    var toast = new MyToast("showNotification");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/Notifications/GetByclass",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#notifications").empty()
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    let notification = new Notification(data[x]);
                    let modul = new Modul(data[x]["modul"]);
                    let teacher = new Teacher(data[x]["teacher"]);
                    var listItem = $("<a></a>").addClass("list-group-item").addClass("list-group-item-action");
                    var divHeader = $("<div></div>").addClass("d-flex").addClass("w-100").addClass("justify-content-between");
                    var h5 = $("<h5></h5>").addClass("mb-1");
                    h5.text(modul.name);
                    var smallHeader = $("<small></small>").addClass("text-muted");
                    switch (notification.type) {
                        case 1:
                            listItem.addClass("list-group-item-danger")
                            smallHeader.append('<i class="las la-exclamation-triangle icon text-danger"></i>')
                            break;
                        case 2:
                            listItem.addClass("list-group-item-warning")
                            smallHeader.append('<i class="las la-exclamation icon text-warning"></i>')
                            break
                        case 3:
                            listItem.addClass("list-group-item-info")
                            smallHeader.append('<i class="las la-info-circle icon text-info"></i>')
                            break;
                        case 4:
                            listItem.addClass("list-group-item-light")
                            smallHeader.append('<i class="las la-envelope icon text-dark"></i>')
                            break;
                    }
                    divHeader.append(h5);
                    divHeader.append(smallHeader);

                    var p = $("<p></p>").addClass("mb-1");
                    p.text(notification.message)
                    var small = $("<small></small>").addClass("text-muted");
                    let createTime = new Date(notification.createTime);
                    const EndOptions = { year: 'numeric', month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric' }
                    small.text("von " + teacher.fullname + " am " + createTime.toLocaleDateString('de-DE', EndOptions))

                    listItem.append(divHeader);
                    listItem.append(p);
                    listItem.append(small);

                    $("#notifications").append(listItem);
                }
                toast.showToast("Die Nachrichten wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Nachrichten konnte nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function append(word, id, classe = 10) {
    var element = "";
    if (word == "exam") {
        element = "";
        let finishid = "#grade_" + id;
        let resultid = "#modul_" + id;
        let iconid = "#icon_" + id;
        $(resultid).attr("onclick", "untie('exam'," + id + ")");
        $(iconid).attr("class", 'las la-chevron-circle-up');
        Exams.forEach(function (e) {
            console.log(Exams);
            var g = new Grade(e.grades[0])
            g.grade = parseFloat(g.grade).toFixed(2)
            if (g.grade < 4) {
                element = "<td class='red'>" + g.grade + "</td>";
            } else {
                element = "<td>" + g.grade + "</td>";
            }
            if (g.confirm) {
                $(finishid).after("<tr id='g_" + e.id + "'><td><div class='platzhalter'></div>" + e.thema + "</td>" + element + "<td></td><td></td><td id='confstatus_" + e.id + "'>Ja</td>" + '<td><button type="button" class="btn btn-outline-info" onclick="showDetails(' + e.id + ')">Details</button></td></tr>');
            } else {
                $(finishid).after("<tr id='g_" + e.id + "'><td><div class='platzhalter'></div>" + e.thema + "</td>" + element + "<td></td><td></td><td id='confstatus_" + e.id + "'><a href='#' onclick='confirm(" + e.id + ")' class='linkWithout'>Nein</a></td>" + '<td><button type="button" class="btn btn-outline-info" onclick="showDetails(' + e.id + ')">Details</button></td></tr>');
            }
        })
    } else if (word == "grade") {
        $("#btn_" + id).attr("class", 'las la-chevron-circle-up');
        modulsGrades.forEach(function (t) {
            t.average = parseFloat(t.average).toFixed(2);
            element += `<tr id="g_` + id + `"><td></td><td>` + t.modulname + `</td><td>` + t.average + `</td><td><button onclick='showGradeForm(` + id + `,"` + classe + `")' type="button" class="btn btn-primary">Note hinzufügen</button></td></tr>`;
        })
        var finishid = "#tr_" + id;
        $(finishid).after(element);
    }
}
function untie(word, idOnClick, classe = 10) {
    if (word == "exam") {
        Exams.forEach(function (g) {
            let ids = "tbody#gradelist tr#g_" + g.id;
            $(ids).remove();
        })
        let resultid = "#modul_" + idOnClick;
        let iconid = "#icon_" + idOnClick;
        $(resultid).attr("onclick", "append(exam," + idOnClick + ")");
        $(iconid).attr("class", 'las la-chevron-circle-down');

    } else if (word == "grade") {
        $("#btn_" + idOnClick).attr("onclick", 'GetDataForGradeAppend(' + idOnClick + ' , "' + classe + '")');
        $("#btn_" + idOnClick).attr("class", 'las la-chevron-circle-down');
        modulsGrades.forEach(function (t) {
            $("tbody#Gradelist tr#g_" + idOnClick).remove();
        })
    }
}







