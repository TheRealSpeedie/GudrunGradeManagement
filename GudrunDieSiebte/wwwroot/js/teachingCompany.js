const teachingCompanies = [];
function getTeachingCompany(callback) {
    var toast = new MyToast("getTeachingCompany");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/TeachingCompanies/",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                $("#teachingCompanyList").empty();
                for (var x = 0; x < data.length; x++) {
                    let tc = new TeachingCompany(data[x]);
                    let students = [];
                    for (var y = 0; y < data[x]["students"].length; y++) {
                        let student = new Student(data[x]["students"][y]);
                        students.push(student);
                    }
                    tc.students = students;
                   
                    teachingCompanies[tc.id] = tc;
                }
                callback();
                toast.showToast("Die Lehrbetriebe wurden erfolgreich geladen");
                console.log(teachingCompanies);
            }
            else {
                toast.showErrorToast("Die Lehrbetriebe konnten nicht geladen werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function deleteModal(id, name) {
    var title = "Lehrbetrieb Löschen";
    var body = "Wollen Sie den Lehrbetrieb "+ name + " wirklich löschen?";
    var footer = "<a type='button' class='cancel' data-bs-dismiss='modal' href='#'>Nein</a>"
        + "<a type='button' class='save' href='#' onclick='getStudentCount(" + id +", checkForDeleteTeachingCompany)'>Ja</a>";
    MyModal.showModal(title, body, footer);
}

function checkForDeleteTeachingCompany(id, count) {
    if (count == 0) {
        deleteTeachingCompany(id);
        MyModal.hideModal();
    } else {
        var title = "Lehrbetrieb kann nicht gelöscht werden";
        var body = "Der Lehrbetrieb kann aufgrund enthaltener Schüler nicht gelöscht werden!";
        var footer = "<a type='button' class='save' data-bs-dismiss='modal' href='#'>Verstanden</a>";
        MyModal.showModal(title, body, footer);
    }
}

function deleteTeachingCompany(id) {
    var toast = new MyToast("deleteTeachingCompany");
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/TeachingCompanies/Delete/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#table_row_" + id).remove();
                toast.showToast("Der Lehrbetrieb wurde erfolgreich gelöscht");
            }
            else {
                toast.showErrorToast("Der Lehrbetrieb konnte nicht gelöscht werden");
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function editTeachingCompany(id) {
    let tc = teachingCompanies[id];
    var title = "Lehrbetrieb Bearbeiten";
    var body = `<div class="row g-3 needs-validation" novalidate>
                    <div class="col-md-6">
                        <label for="validationCustom01" class="form-label" id="labelName1">Name</label>
                       `+ "<input type='text' class='form-control' id='inputName1' value=" + tc.name + " >" + `
                            <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                    <div class="col-md-6">
                        <label for="validationCustom01" class="form-label" id="labelAdresse1">Adresse</label>
                        `+ "<input type='text' class='form-control ' id='inputAdresse1' value=" + tc.address + " >" + `
                            <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                    <div class="col-md-6">
                        <label for="validationCustom01" class="form-label" id="labelCity1">Stadt</label>
                        `+ "<input type='text' class='form-control'  id='inputCity1' value=" + tc.city + " >" + `
                           <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                    <div class="col-md-6">
                        <label for="validationCustom01" class="form-label">Anzahl Schüler</label>
                        `+ "<input type='number' class='form-control' id='inputStudent1' value=" + tc.students.length + " readonly>" + `
                    </div>
                  </div>`;

    var footer = "<a type='button' class='cancel' data-bs-dismiss='modal' href='#'>Abbrechen</a>"
        + "<a type='button' class='save'  href='#' onclick='validate(1, " + tc.id + ")'>Speichern</a>";
    MyModal.showModal(title, body, footer);   
}
function addTeachingComany() {
    var title = "Lehrbetrieb Hinzufügen";
    var body = `<div class="row">
                    <div class="col-md-6" id="RowName">
                        <label for="inputName2" class="form-label" id="labelName2">Name</label>
                       <input type='text' class='form-control ' id='inputName2'>
                           <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                     <div class="col-md-6" id="RowAdresse">
                        <label for="inputCity2" class="form-label" id="labelAdresse2">Adresse</label>
                        <input type='text' class='form-control' id='inputAdresse2'>
                           <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                    <div class="col-md-6" id="RowCity">
                        <label for="inputCity2" class="form-label" id="labelCity2">Stadt</label>
                        <input type='text' class='form-control' id='inputCity2'>
                           <div class="invalid-feedback">
                                Ungültiges Zeichen!
                           </div>
                    </div>
                  </div>`;
    var footer = "<a type='button' class='cancel' data-bs-dismiss='modal' href='#'>Abbrechen</a>"
        + "<a type='button' class='save'  href='#' onclick='validate(2)'>Speichern</a>";
    MyModal.showModal(title, body, footer);
}
function validate(number, id) {
    var format = /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~¨]/;
    if (number == 1) {
        var name = document.getElementById('inputName1').value;
        var adresse = document.getElementById('inputAdresse1').value;
        var city = document.getElementById('inputCity1').value;
    } else {
        var name = document.getElementById('inputName2').value;
        var adresse = document.getElementById('inputAdresse2').value;
        var city = document.getElementById('inputCity2').value;
    }
    let inputName = "#inputName" + number;
    let labelName = "#labelName" + number;
    let inputAdress = "#inputAdresse" + number;
    let labelAdress = "#labelAdresse" + number;
    let inputCity = "#inputCity" + number;
    let labelCity = "#labelCity" + number;
    var check = true;
    if (format.test(name)) {
        $("#RowName .invalid-feedback").css("display", "block")
        $(inputName).css("border-color", "red")
        $(labelName).css("color", "red")
        check = false;
    }
    if (format.test(adresse)) {
        $("#RowAdresse .invalid-feedback").css("display", "block")
        $(inputAdress).css("border-color", "red")
        $(labelAdress).css("color", "red")
        check = false;
    }
    if (format.test(city)) {
        $("#RowCity .invalid-feedback").css("display", "block")
        $(inputCity).css("border-color", "red")
        $(labelCity).css("color", "red")
        check = false;
    }
    if (check) {
        MyModal.hideModal();
        if (number == 1) {
            setTeachingCompany(id);
        }
        else {
            SaveTeachingCompany();
        }
    }
    return
}

function setTeachingCompany(id) {
    var toast = new MyToast("setTeachingCompany");
    var name = document.getElementById('inputName1').value;
    var adresse = document.getElementById('inputAdresse1').value;
    var city = document.getElementById('inputCity1').value;
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/TeachingCompanies/Edit/" + id,
        data: {
            "Id": id,
            "Name": name,
            "City": city,
            "Address": adresse
        },
        success: function (response) {
            if (response["success"]) {
                teachingCompanies[response["data"]["id"]] = new TeachingCompany(response["data"]);
                updateAdminTable();
                toast.showToast("Der Lehrbetrieb wurden erfolgreich Aktualisiert");
            }
            else {
                toast.showErrorToast(response["error"]);
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}
function SaveTeachingCompany() {
    var toast = new MyToast("SaveTeachingCompany");
    var name = document.getElementById('inputName2').value;
    var adresse = document.getElementById('inputAdresse2').value;
    var city = document.getElementById('inputCity2').value;
    $.ajax({
        method: "POST",
        url: "https://" + window.location.host + "/TeachingCompanies/Create",
        data: {
            "name": name,
            "city": city,
            "address": adresse
        },
        success: function (response) {
            if (response["success"]) {
                toast.showToast("Der Lehrbetrieb wurden erfolgreich Hinzugefügt");
                var data = response["data"];
                $("#teachingCompanyList").append("<tr id='table_row_" + data["id"] + "'>"
                    + "<td>" + data["name"] + "</td>"
                    + "<td>" + data["address"] + "</td>"
                    + "<td>" + data["city"] + "</td>"
                    + "<td><a href='#' onclick='editTeachingCompany(\"" + data["id"] + "\")'><i class='las la-edit icon'></i></a>"
                    + "<a href = '#' onclick = 'deleteModal(\"" + data["id"] + "\",\"" + data["name"] + "\")' ><i class='las la-trash icon'></i></a ></td > "
                    + "<tr>");
            }
            else {
                toast.showErrorToast(response["error"]);
            }
        },
        error: function (XMLHttpRequest) {
            toast.showErrorToast("Status: " + XMLHttpRequest.status);
        }
    });
}

function updateTeachingCompanyTable(admin = false) {
    var toast = new MyToast("updateTable");
    $("#teachingCompanyList").empty();
    var adminstuff = "";
    if (admin) {
        adminstuff = + "<td><a href='#' onclick='editTeachingCompany(" + tc.id + ")'><i class='las la-edit icon'></i></a>"
            + "<a href = '#' onclick = 'deleteModal(\"" + tc.id + "\",\"" + tc.name + "\")' ><i class='las la-trash icon'></i></a ></td > "
    }
    teachingCompanies.forEach(function (tc) {
        $("#teachingCompanyList").append("<tr id='table_row_" + tc.id + "'>"
            + "<td>" + tc.name + "</td>"
            + "<td>" + tc.address + "</td>"
            + "<td>" + tc.city + "</td>" 
            + adminstuff
            + "<tr>");
    });
    toast.showToast("Die Lehrbetriebe wurden erfolgreich Aktualisiert");
}
function sortTable(wert, admin = false) {
    if (wert == "name") {
        teachingCompanies.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });
    }
    else if (wert == "address") {
        teachingCompanies.sort(function (a, b) {
            return a.address.localeCompare(b.address);
        });
    }
    else if (wert == "city") {
        teachingCompanies.sort(function (a, b) {
            return a.city.localeCompare(b.city);
        });
    }
    if (admin) {
        updateTeachingCompanyTable(true);
    } else {
        updateTeachingCompanyTable();
    }
}
