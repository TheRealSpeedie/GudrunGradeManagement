
function showTeacherDetails(id) {
    var toast = new MyToast("showTeacherDetails");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Teachers/Details/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                let teacher = new Teacher(data);
                const birthdayOptions = { year: 'numeric', month: 'short', day: 'numeric' };
                var birthdayDate = new Date(teacher.birthday);
                var jetzt = new Date();
                const alter = jetzt.getFullYear() - birthdayDate.getFullYear();
                const EndOptions = { hour: 'numeric', minute: 'numeric' };
                var ListItem1 = "";
                var ListItem2 = "";
                for (let x = 1; x < data["lessons"].length + 1; x++) {
                    let lesson = new Lesson(data["lessons"][x -1])
                    var Starttime = new Date(lesson.starttime);
                    var Endtime = new Date(lesson.endtime);
                    let modul = new Modul(data["lessons"][x - 1]["modul"])
                    let myclass = new Class(data["lessons"][x - 1]["class"])
                    if ((x % 2) == 0) {
                        ListItem2 += `<li> Modul: ` + modul.name + ` <ul><li> Klasse: ` + myclass.name + `</li><li> Wann: ` + Starttime.toLocaleDateString('de-DE', EndOptions) + ` bis ` + Endtime.toLocaleTimeString('de-DE', EndOptions) + `</li></ul></li>`
                    }
                    else {
                        ListItem1 += `<li> Modul: ` + modul.name + ` <ul><li> Klasse: ` + myclass.name + `</li><li> Wann: ` + Starttime.toLocaleDateString('de-DE', EndOptions) + ` bis ` + Endtime.toLocaleTimeString('de-DE', EndOptions) + `</li></ul></li>`
                    }
                }
                var title = data["person"]["fullName"];
                var body = `<div class="container ">
	                            <div class="row">
		                            <div class="col-md-4 ">
                                        <img src='../Pictures/small/default_Teacher.png'>
        	                        </div>
                                    <div class="col-md-8 ">
                                        Persöhnliches:
                                        <ul>
                                                <li>Vorname: ` + teacher.firstname + `</li>
                                                <li>Nachname: ` + teacher.lastname + `</li>
                                                <li>Alter: ` + alter + `</li>
                                                <li>Geburtstag: ` + birthdayDate.toLocaleDateString('de-DE', birthdayOptions) + `</li>
                                                <li>Adresse: ` + teacher.address + `</li>
                                                <li>Stadt: ` + teacher.city + `</li>
                                        </ul>
                                        Besonderes:
                                           <ul>
                                               <li>Hasst Schüler deren Name mit 'W' beginnt</li>
                                        </ul>   
                                        Bestechung:
                                           <ul>
                                               <li>Hand heben während des Unterrichtes</li>
                                        </ul>
         	                        </div>

 	                            </div> 
	                           <div class="row ">
                                    <div class="col-md-6">
                                        Lektionen: 
                                        <ul> ` + ListItem1 + `</ul>
                                    </div>
                                    <div class="col-md-6">
                                         <ul> ` + ListItem2 + `</ul>
                                    </div>
                              </div>
                        </div>`
                var footer = `Gudrun <img src='../Pictures/icon/32/icon.png'>`
                MyModal.showModal(title, body, footer)
            }
            else {
                toast.showErrorToast("Die Details des Lehrers konnten nicht angezeigt werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
const teachers = []
function CreateTeacherSchoolManagement(data, callback) {
    for (var x = 0; x < data.length; x++) {
        let t = new Teacher(data[x]);
        let sm = new SchoolManagement(data[x]["schoolManagement"]);
        t.schoolManagement = sm;
        teachers[t.id] = t;
    }
    callback();
}
function getMyTeachers() {
    var toast = new MyToast("getMyTeachers");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Teachers/GetMyTeachers",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                CreateTeacherSchoolManagement(data, updateTeacherTable)
                toast.showToast("Die Lehrer wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Lehrer konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function getAllTeachers(callback) {
    var toast = new MyToast("getAllTeachers");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Teachers/GetAllTeachers",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                CreateTeacherSchoolManagement(data, callback);
                toast.showToast("Die Lehrer wurden erfolgreich geladen");
            }
            else {
                toast.showErrorToast("Die Lehrer konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}

function updateTeacherTable(admin = false) {
    $("#teacherlist").empty()
    console.log(teachers);
    var adminstuff = "";
    if (admin) {
        adminstuff = "<button type='button' class='btn btn-outline-warning' onclick=''>Edit</button>"
            + "<button type='button' class='btn btn-outline-danger' onclick=''>Delete</button>";
    }
    teachers.forEach(function (t) {
        $("#teacherlist").append("<tr id='table_row_" + t.id + "'>"
            + "<td>" + t.fullname + "</td>"
            + "<td>" + t.schoolManagement.schoolName +"</td>"
            + "<td>" + "<button type='button' class='btn btn-outline-info' onclick='showTeacherDetails(" + t.id + ")'>Details</button>" + "</td>"
            + adminstuff
            + "<tr>"
        );
    })
}