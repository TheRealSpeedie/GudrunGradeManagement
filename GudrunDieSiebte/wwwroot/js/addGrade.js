$(document).ready(function () {
    const knownLink = "/Teacher/AddGrade/";
    const regexPattern = /[1-9]+_[1-9]+/;
    const currentLink = window.location.href;
    if (currentLink.includes(knownLink)) {
        var Text = currentLink.split("/");
        var finalText = Text[5].split("_");
        var classID = finalText[0];
        var examID = finalText[1];
        if (regexPattern.test(Text[5])) {
            showGradeForm(classID, examID);
        } else {
            history.back();
        }
    }
})
var changes = false;
var changedData = [];
function showGradeForm(classID, examID) {
    var highscore = 0;
    var myHtml = "";
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/getAllStudentsWithExam/" + examID + "/" + classID,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                var students = [];
                for (let x = 0; x < data["myStudents"].length; x++) {
                    var s = new Student(data["myStudents"][x]);
                    var g = new Grade(data["myStudents"][x]["grades"][0])
                    s.grade = g;
                    students.push(s);
                }
                var tablediv = '<div class="tablediv">';
                var table = '<table class="table table-hover">';
                var thead = '<thead class="text-light bg-dark">';
                var tds = '<td>Name</td><td>Punkte</td><td>Highscore</td><td>Note</td>';
                var theadEnd = "</thead>";
                var tbody = '<tbody id="addGradeTable" class="text-light">';
                var tbodyInput = "";
                students.forEach(function (t) {
                    var score = 0;
                    highscore = data["highscore"];
                    var grade = 0;
                    if (t.grade != null) {
                        score = t.grade.score;
                        grade = Math.round(t.grade.grade * 2) / 2;
                    }
                    tbodyInput += "<tr><td>" + t.fullname + "</td><td><input type='number' class='ScoreInputs' id='scoreInput_" + t.id + "' name='scoreInputField' min='0' max=" + highscore + " value=" + score + "></td><td>" + highscore + "</td><td id='myGradeField_" + t.id + "'>" + grade + "</td></tr>";
                })
                var tbodyEnd = " </tbody>";
                var tableEnd = "</table>";
                var divEnd = "</div>";
                var link = "<div onclick='checkChanges(" + classID + ", " + examID + ")'>Zurückkehren</div>";
                myHtml += tablediv + table + thead + tds + theadEnd + tbody + tbodyInput + tbodyEnd + tableEnd  + divEnd + link
                $("#AddGradeView").empty();
                $("#AddGradeView").append(myHtml);
                const inputFields = document.querySelectorAll(".ScoreInputs");
                inputFields.forEach((inputField) => {
                    const inputId = inputField.id.split('_')[1];
                    var gradefield = document.querySelector('#myGradeField_' + inputId);
                    var value = inputField.value;
                    inputField.addEventListener('input', () => {
                        if (inputField.value !== value) {
                            changes = true;
                            let currentValue = inputField.value;
                            if (currentValue > 0) {
                                let grade = currentValue / highscore * 5 + 1;
                                grade = Math.round(grade * 4) / 4;
                                let existingIndex = -1;
                                for (let i = 0; i < changedData.length; i++) {
                                    if (changedData[i].fk_Student === inputId) {
                                        existingIndex = i;
                                        break;
                                    }
                                }
                                if (existingIndex !== -1) {
                                    changedData.splice(existingIndex, 1);
                                }
                                changedData.push({
                                    fk_Student: inputId,
                                    score: currentValue,
                                    grade: grade
                                });
                                gradefield.textContent = grade;
                            } else {
                                gradefield.textContent = 0;
                            }
                        }
                    });
                }
                )
            }
        }
    })
}
function checkChanges(classID, examID) {
    if (changes) {
        var title = "Noten Speichern";
        var body = "Wollen Sie die Änderungen speichern?";
        var footer = "<a type='button' class='cancel' data-bs-dismiss='modal' onclick='GoBack()' href='#'>Nein</a>"
            + "<a type='button' class='save' href='#' onclick='GoBack(" + true + ", " + classID + ", " + examID + ")'>Ja</a>";
        MyModal.showModal(title, body, footer);
    } else {
        location.href = "https://" + window.location.host + "/Teacher/Grade";
    }
}

function GoBack(bool = false, classID = 0, examID = 0) {
    if (bool) {
        console.log(changedData);
        $.ajax({
            method: "POST",
            url: "https://" + window.location.host + "/Exams/saveGrades",
            data: {
                classID: classID,
                examID: examID,
                grades: JSON.stringify(changedData)
            },
            success: function (response) {
                if (response["success"]) {

                }
            }
        })
    }
    location.href = "https://" + window.location.host + "/Teacher/Grade";
}