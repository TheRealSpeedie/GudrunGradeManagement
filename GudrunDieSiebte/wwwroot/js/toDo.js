
const myToDos = [];
const DateOptions = { year: 'numeric', month: 'short', day: 'numeric' };

function fillTodoList() {
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Students/GetMyToDos",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#toDoList").empty();
                var data = response["data"];
                for (let x = 0; x < data.length; x++) {
                    var todo = new ToDo(data[x])
                    todo.modul = new Modul(data[x]["modul"])
                    todo.teacher = new Teacher(data[x]["teacher"])
                    todo.Done = data[x]["done"];
                    myToDos.push(todo);
                }
            }
        }
    })
}
function getTodo(view = "") {
    var HTML = "";
    var toast = new MyToast("getTodo");
    var DivID = "";
    if (myToDos.length == 0) {
        fillTodoList();
    }
    if (view == "done") {
        DivID = "#doneToDoList";
        myToDos.forEach(function (i) {
            if (i.Done) {
                console.log("done " + i.task);
                var confirm = '<input class="form-check-input mySwitches" type="checkbox" id="' + i.id + '" checked>';
                var upTo = new Date(i.upTo);
                HTML += "<tr><td>" + upTo.toLocaleDateString('de-DE', DateOptions) + "</td><td>" + i.task + "</td><td>" + i.modul.name + "</td><td>" + i.teacher.fullname + "</td><td>" + confirm + "</td></tr>";
            }
        })
    } else if (view == "notDone") {
        DivID = "#notDoneToDoList"
        myToDos.forEach(function (i) {
            if (!i.Done) {
                var confirm = '<input class="form-check-input mySwitches" type="checkbox" id="' + i.id + '">';
                var upTo = new Date(i.upTo);
                HTML += "<tr><td>" + upTo.toLocaleDateString('de-DE', DateOptions) + "</td><td>" + i.task + "</td><td>" + i.modul.name + "</td><td>" + i.teacher.fullname + "</td><td>" + confirm + "</td></tr>";
            }
        })
    } else {
        DivID = "#toDoList"
        myToDos.forEach(function (i) {
            if (i.Done) {
                var confirm = '<input class="form-check-input mySwitches" type="checkbox" id="' + i.id + '" checked>';
            } else {
                var confirm = '<input class="form-check-input mySwitches" type="checkbox" id="' + i.id + '" >';
            }
            var upTo = new Date(i.upTo);
            HTML += "<tr><td>" + upTo.toLocaleDateString('de-DE', DateOptions) + "</td><td>" + i.task + "</td><td>" + i.modul.name + "</td><td>" + i.teacher.fullname + "</td><td>" + confirm + "</td></tr>";
        })
    }
    $(DivID).empty();
    $(DivID).append(HTML);
    var switches = document.querySelectorAll('.mySwitches');
    switches.forEach(function (switchElement) {
        switchElement.addEventListener('change', function () {
            if (switchElement.checked) {
                setDone(switchElement);
            } else {
                resetDone(switchElement);
            }
        });
    })
    toast.showToast("Die Aufgaben wurden erfolgreich geladen");
}

function setDone(switchElement) {
    var toast = new MyToast("setDone");
    var id = switchElement.id
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Students/SetDone/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                toast.showToast("Die Aufgabe wurde auf erledigt gesetzt");
            }
            else {
                toast.showErrorToast("Die Aufgabe konnte nicht auf erledigt gesetzt");
            }
        }
    })
}

function resetDone(switchElement) {
    var toast = new MyToast("resetDone");
    var id = switchElement.id
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Students/ResetDone/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                toast.showToast("Die Aufgabe wurde auf noch nicht erledigt gesetzt");
            }
            else {
                toast.showErrorToast("Die Aufgabe konnte nicht auf noch nicht erledigt gesetzt");
            }
        }
    })
}



