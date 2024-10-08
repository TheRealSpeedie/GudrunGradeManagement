var module = [];
var Exams = [];
function getMyGrades() {
    var toast = new MyToast("getMyGrades");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/GetMyGrades",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var average = "";
                var point = 0;
                $("#gradelist").empty();
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var m = new Modul(data[x]);
                    var e = new Exam(data[x]["exam"]);
                    var grades = [];
                    for (let y = 0; y < data[x]["exam"]["grades"].length; y++) {
                        var g = new Grade(data[x]["exam"]["grades"][y])
                        grades.push(g);
                    }
                    e.grades = grades;
                    Exams.push(e);
                    m.exam = e;
                    module[m.id] = m;
                } 
                module.forEach(function (t) {
                    $("#gradelist").append(`<tr id="grade_` + t.id + `">                                                
                                                <td><a href="#" onclick="append(exam, `+ t.id + `)" class="linkWithout" id="modul_` + t.id + `">` + t.name + `<i id="icon_`+ t.id +`"class="las la-chevron-circle-down"></i></a></td>
                                                <td> </td>
                                                <td id="avg_`+ t.id + `"></td>
                                                <td id="points_`+ t.id+`"></td>
                                                <td id="con_` + t.id + `"></td><td></td>
                                            </tr>`
                    );
                    getAverage(t.id);
                    isConfirm(t.id);
                    getPointsFromModul(t.id);
                },
                    toast.showToast("Die Noten wurden erfolgreich geladen"));
            }
            else {
                toast.showErrorToast("Die Noten konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function getAverage(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/GetAverage/" + id,
        data: {},
        success: function (response) {
            average = response["data"]
            if (average > 0) {
                document.getElementById("avg_" + id).innerHTML = parseFloat(average).toFixed(2);
            }
        },
    });
}
function getPointsFromModul(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/GetPointsFromModul/" + id,
        data: {},
        success: function (response) {
            point = response["data"];
            document.getElementById("points_" + id).innerText = point;
        },
    });
}
function isConfirm(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/IsConfirm/" + id,
        data: {},
        success: function (response) {
            let confirm = response["data"]
            if (confirm != "undefiniert") {
                var sign = "";
                if (confirm == "true") {
                    sign = `<td id="con_` + id + `">Ja</td>`
                }
                else {
                    sign = `<td id="con_` + id + `"><a href="#" onclick="confirmALL(` + id + `)" class="linkWithout">Nein</a></td>`
                }
                let resultid = "#con_" + id;
                $(resultid).html(sign);
            }
        },
    });
}
function confirmALL(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/SetConfirmAll/" + id,
        data: {},
        success: function (response) {
            getMyGrades();
        },
    });
}
function confirm(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/SetConfirm/" + id,
        data: {},
        success: function (response) {
            updateRow(id);
        },
    });
}
function updateRow(id) {
    var finishid = "#confstatus_" + id;
    $(finishid).html("<td id='confstatus_" + id + "'>Ja</td>");
}

function showDetails(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/DetailExam/" + id,
        data: {},
        success: function (response) {
            var data = response["data"];
            var e = new Exam(data);
            console.log(e);
            var confirmStatus = "";
            if (e.confirm) {
                confirmStatus = "Ja";
            } else {
                confirmStatus = "Nein";
            }
            var title = e.thema;
            var body = `<div class="container">
                            <div class="row ">
                                <div class="col-md-6">
                                Erreichte Punkt: `+ e.score +` von `+ e.highscore +`
                                </div>
                                <div class="col-md-6">
                                  Bestätigt: `+ confirmStatus +`
                                </div>
                            </div>
                        </div>`;
            var footer = `Gudrun <img src='../Pictures/icon/32/icon.png'>`;
            MyModal.showModal(title, body, footer)
        },
    });
}



function getMyTestimoiny() {
    var toast = new MyToast("getMyGrades");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/GetMyGrades",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#gradelist").empty();
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var m = new Modul(data[x]);
                    var e = new Exam(data[x]["exam"]);
                    m.exam = e;
                    module[m.id] = m;
                }
                module.forEach(function (t) {
                    $("#gradelist").append(`<tr id="grade_` + t.id + `">                                                
                                                <td> ` + t.name + `</td>
                                                <td id="endgrade_` + t.id + `"></td>
                                                <td id="avg_`+ t.id + `"></td>
                                            </tr>`
                    );
                    getAverageAndGrade(t.id);


                },
                    toast.showToast("Die Noten wurden erfolgreich geladen"));
            }
            else {
                toast.showErrorToast("Die Noten konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function getAverageAndGrade(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/GetAverage/" + id,
        data: {},
        success: function (response) {
            average = response["data"]
            if (average > 0) {
                document.getElementById("avg_" + id).innerHTML = parseFloat(average).toFixed(2);
                average = Math.round(average * 2) / 2;
                document.getElementById("endgrade_" + id).innerHTML = average;
            }
        },
    });
}
function getPDFTestimony() {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/GetPDFTestimony",
        data: {},
        success: function (response) {
           
        },
        error: function (XMLHttpRequest) {
        }
    });
}
function getCSVTestimony() {
    var toast = new MyToast("getCSVTestimony");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/GetCSVTestimony",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                const anchor = document.createElement('a');
                anchor.href = "https://" + window.location.host + data;
                anchor.download = "Zeugnis.csv";
                document.body.appendChild(anchor);
                anchor.click();
                document.body.removeChild(anchor);
                toast.showToast("Das Zeugniss wurde erfolgreich heruntergeladen");
            }
        },
    });
}
