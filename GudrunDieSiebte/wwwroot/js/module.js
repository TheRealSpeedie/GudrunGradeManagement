
function GetMyModuls() {
    var toast = new MyToast("GetMyModuls");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/GetMyModuls",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                $("#module").empty();
                var ListItem1 = "";
                var ListItem2 = "";
                for (var x = 1; x < data.length + 1; x++) {
                    let m = new Modul(data[x - 1]);
                    var html = ` <ul class="list-group modullist">
                                            <li class="list-group-item list-group-item-primary"><a href="#" onclick="openShedule(` + m.id + `)" class="linkWithout changeColor">` + m.name + `</a></li>
                                            <li class="list-group-item">` + m.description + `</li>
                                       </ul>`;
                    if ((x % 2) == 0) {
                        ListItem2 += html;
                    }
                    else {
                        ListItem1 += html;
                    }
                }
                $("#module").append(`
            <div class="container">
                <div class="row">
                    <div class="col-lg-4 col-md-12">
                   `+ ListItem1 + `
                    </div>
                    <div class="col-lg-4 col-md-6">
                  `+ ListItem2 + `
                    </div>
                </div>
            </div>`);
                toast.showToast("Die Module wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Module konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
const SheduleLessons = []
const Sheduleexam = []
function openShedule(id) {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/GetShedule/" + id,
        data: {},
        success: function (response) {
            var data = response["data"];
            var m = new Modul(data[0]);
            for (let a = 0; a < data[0]["lessons"].length; a++) {
                var l = new Lesson(data[0]["lessons"][a])
                if (!SheduleLessons.includes(l)) {
                    SheduleLessons[l.id] = l;
                }
            }
            for (let a = 0; a < data[0]["exam"].length; a++) {
                var e = new Exam(data[0]["exam"][a])
                if (!Sheduleexam.includes(e)) {
                    Sheduleexam[e.id] = e;
                }
            }
            SheduleLessons.sort(function compare(a, b) {
                var dateA = new Date(a.starttime);
                var dateB = new Date(b.starttime);
                return dateA - dateB;
            });
            Sheduleexam.sort(function compare(a, b) {
                var dateA = new Date(a.examday);
                var dateB = new Date(b.examday);
                return dateA - dateB;
            });
            var gridlesson = "";
            var gridexam = "";
            const EndOptions = { hour: 'numeric', minute: 'numeric' };
            const birthdayOptions = { year: 'numeric', month: 'short', day: 'numeric' }
            SheduleLessons.forEach(function (l) {
                var Starttime = new Date(l.starttime);
                var Endtime = new Date(l.endtime);
                gridlesson += ` <li>` + "Am " + Starttime.toLocaleDateString('de-DE', EndOptions) + ` bis ` + Endtime.toLocaleTimeString('de-DE', EndOptions) + `</li>`;
            })
            Sheduleexam.forEach(function (l) {
                var birthdayDate = new Date(l.examday);
                gridexam += ` <li>` + birthdayDate.toLocaleDateString('de-DE', birthdayOptions) + `</li>`;
            })
            var container = `<div class="container">`
            var divend = "</div>"
            var solorow = `<div class="row gx-1 ">`
            var duocol = `<div class="col-md-6">`
            var col = `<div class="col-12">`
            var sheduletxt = solorow + col + m.shedule + divend + divend;
            var sheduls = solorow + duocol + "Lektionen: " + gridlesson + divend + duocol + "Prüfungen: " + gridexam + divend + divend;
            var title = m.name
            var body = container + sheduletxt + sheduls + divend;
            var footer = `Gudrun <img src='../Pictures/icon/32/icon.png'>`;
            MyModal.showModal(title, body, footer)
        },
    });
}

function getModuleByName(modulname) {
    console.log(modulname);
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/getModuleByName/" + modulname,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var DivId = "#" + modulname + "_body";
                $(DivId).empty();
                var data = response["data"];
                var m = new Modul(data);
                var td = '<td id="' + m.name + '_description">' + m.description + '</td>'
                $(DivId).append("<tr>" + td + "</tr>");
            }
        },
    });
}

function getModulStructure(specific = "") {
    var toast = new MyToast("getModulStructure");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Moduls/getAllModule",
        data: {},
        success: function (response) {
            var moduls = []
            if (response["success"]) {
                var data = response["data"];
                for (let x = 0; x < data.length; x++) {
                    var m = new Modul(data[x]);
                    moduls.push(m);
                }
                var ulopen = '<ul class="nav nav-tabs" id="myTab" role="tablist">'
                var ulclose = "</ul>"
                var liItems = "";
                var seconddivs = ""
                var firstmodul = "";
                for (let y = 0; y < moduls.length; y++) {
                    var liopen = '<li class="nav-item" role="presentation">'
                    var liclose = "</li>"
                    console.log(moduls[y]);
                    var name = moduls[y].name;
                    if (y == 0 && specific == "" || specific == name) {
                        var liinput = `<button onclick='getModuleByName("` + name + `")' class="nav-link active" id="` + name + `-tab" data-bs-toggle="tab" data-bs-target="#` + name + `" type="button" role="tab" aria-controls="` + name + `" aria-selected="true">` + name + `</button>`;
                        var seconddiv = '<div class="tab-pane fade show active" id="' + name + '" role="tabpanel" aria-labelledby="' + name + '-tab">'
                        firstmodul = name;
                    } else {
                        var liinput = `<button onclick='getModuleByName("` + name + `")' class="nav-link" id="` + name + `-tab" data-bs-toggle="tab" data-bs-target="#` + name + `" type="button" role="tab" aria-controls="` + name + `" aria-selected="false">` + name + `</button>`;
                        var seconddiv = '<div class="tab-pane fade" id="' + name + '" role="tabpanel" aria-labelledby="' + name + '-tab">'
                    }
                    var table = `<div class="tablediv">
			                        <table class="table table-hover">
				                        <thead class="text-light bg-dark">
				                            <td>Beschreibung</td>
                                        </thead>
				                        <tbody id="` + name + `_body" class="text-light">
				                        </tbody>
			                    </table>
		                    </div>`
                    liItems += liopen + liinput + liclose;
                    seconddivs += seconddiv + table + "</div>";
                    y++
                }
                var firstdiv = '<div class="tab-content" id="myTabContent">'
                var divend = "</div>"
                var html = ulopen + liItems + ulclose + firstdiv + seconddivs + divend;
                $("#modulview").html(html);
                toast.showToast("Die Module wurden erfolgreich geladen");
                getModuleByName(firstmodul);
            }
            else {
                toast.showErrorToast("Die Module konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}


