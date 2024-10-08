function toggleTree(string) {
    $("#" + string).click();
}

var modulHTML = "";
function modulview(classID, modulID) {
    modulHTML = "";
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/GetModulbyIDWithClass/" + modulID + "/" + classID,
        data: {},
        success: function (response) {
            const dateoptions = { year: 'numeric', month: 'short', day: 'numeric' };
            if (response["success"]) {
                var data = response["data"];
                var modul = new Modul(data);
                var examlist = [];
                for (let x = 0; x < data["exams"].length; x++) {
                    var exams = new ExamsWithAverage(data["exams"][x]);
                    var examday = new Date(exams.examday);
                    exams.examday = examday.toLocaleDateString('de-DE', dateoptions)
                    exams.classAVG = Math.round(exams.classAVG * 2) / 2;
                    examlist.push(exams);
                }
                var title = "<h1>Das Modul <div class='higlight'>" + modul.name + "</div> hat folgende Pr&#252;fungen</h1>";
                var tablediv = '<div class="tablediv">';
                var table = '<table class="table table-hover">';
                var thead = '<thead class="text-light bg-dark">';
                var tds = '<td>Pr&#252;fung</td><td>Pr&#252;fungstag</td><td>Note</td>';
                var theadEnd = "</thead>";
                var tbody = '<tbody id="examStudentlist" class="text-light">';
                var tbodyInput = "";
                examlist.forEach(function (t) {
                    tbodyInput += "<tr onclick='toggleTree(`" + t.id + "_" + classID + "`)'><td>" + t.name + "</td><td>" + t.examday + "</td><td>" + t.classAVG + "</td></tr>";
                });
                var tbodyEnd = " </tbody>";
                var tableEnd = "</table>";
                var divEnd = "</div>";
                modulHTML += title + tablediv + table + thead + tds + theadEnd + tbody + tbodyInput + tbodyEnd + tableEnd + divEnd
                appendView("modul");
            }
        },
    });
}

var classHTML = "";
function classview(classID) {
    classHTML = "";
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/getModulsFromClassByID/" + classID,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                var c = new Class(data);
                var modulliste = [];
                for (let x = 0; x < data["moduls"].length; x++) {
                    var modul = new Modul(data["moduls"][x]);
                    modulliste.push(modul);
                }
                var title = "<h1>Die Klasse <div class='higlight'>" + c.name + "</div> hat folgende Module:</h1>";
                var tablediv = '<div class="tablediv">';
                var table = '<table class="table table-hover">';
                var thead = '<thead class="text-light bg-dark">';
                var tds = '<td>Modul</td><td>Beschreibung</td>';
                var theadEnd = "</thead>";
                var tbody = '<tbody id="examStudentlist" class="text-light">';
                var tbodyInput = "";
                modulliste.forEach(function (t) {
                    tbodyInput += "<tr onclick='toggleTree(`" + c.id + "_" + t.id + "`)'><td>" + t.name + "</td><td>" + t.description + "</td></tr>";
                });
                var tbodyEnd = " </tbody>";
                var tableEnd = "</table>";
                var divEnd = "</div>";
                classHTML += title + tablediv + table + thead + tds + theadEnd + tbody + tbodyInput + tbodyEnd + tableEnd + divEnd
                appendView("class");
            }
        },
    });
}
var examHTML = "";
function examview(examID, classID) {
    examHTML = "";
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Exams/getExamsFromClass/" + examID + "/" + classID,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                var exam = new Exam(data);
                var students = [];
                var allgrades = [];
                for (let x = 0; x < data["students"].length; x++) {
                    var s = new Student(data["students"][x]);
                    var g = new Grade(data["students"][x]["grades"][0])
                    if (g.grade > 0) {
                        allgrades.push(g);
                    }
                    s.grade = g;
                    students.push(s);
                }
                const dateoptions = { year: 'numeric', month: 'short', day: 'numeric' };
                var examday = new Date(exam.examday);
                var avg = 0.0;
                if (allgrades.length > 0) {
                    for (let x = 0; x < allgrades.length; x++) {
                            avg += allgrades[x].grade;
                    }
                    avg = avg / allgrades.length;
                }
                console.log(avg, allgrades);
                avg = Math.round(avg * 2) / 2;
                var title = "<h1>Die Pr&#252;fung <div class='higlight'>" + exam.name + "</div> hat einen Schnitt von " + avg + "</h1><h2>Pr&#252;fungstag: " + examday.toLocaleDateString('de-DE', dateoptions) + "</h2>";
                var tablediv = '<div class="tablediv">';
                var table = '<table class="table table-hover">';
                var thead = '<thead class="text-light bg-dark">';
                var tds = '<td>Sch&#252;ler</td><td>Note</td>';
                var theadEnd = "</thead>";
                var tbody = '<tbody id="examStudentlist" class="text-light">';
                var tbodyInput = "";
                students.forEach(function (t) {
                    if (t.grade.grade > 0) {
                        var grade = Math.round(t.grade.grade * 2) / 2;
                    } else {
                        var grade = 0;
                    }
                    tbodyInput += "<tr><td>" + t.fullname + "</td><td>" + grade + "</td></tr>";
                })
                var tbodyEnd = " </tbody>";
                var tableEnd = "</table>";
                var divEnd = "</div>";
                var link = "<a class='linkWithout' href='https://" + window.location.host + "/Teacher/AddGrade/" + classID + "_" + examID + "'>Noten hinzuf&#252;gen/Bearbeiten</a>"
                examHTML += title + tablediv + table + thead + tds + theadEnd + tbody + tbodyInput + tbodyEnd + tableEnd+link + divEnd
                appendView("exam");
            }
        },
    });
}
function appendView(thing) {
    var variable = "";
    if (thing == "exam") {
        variable = examHTML;
    } else if (thing == "modul") {
        variable = modulHTML;
    } else if (thing == "AllClasse") {
        variable = AllClassesViewHTML;
    }
    else {
        variable = classHTML
    }
    $("#gradeViews").empty();
    $("#gradeViews").append(variable);
}
var html = "";
function fillTreeGrade() {
    var toast = new MyToast("fillTreeGrade");
    var mylist = [];
    if (html == "") {
        $.ajax({
            method: "GET",
            url: "https://" + window.location.host + "/Classes/getClassesModulExam",
            data: {},
            success: function (response) {
                if (response["success"]) {
                    html = "<ul>";
                    $("#Gradelist").empty();
                    var data = response["data"];
                    for (var x = 0; x < data.length; x++) {
                        var c = new Class(data[x]["classe"]);
                        for (var y = 0; y < data[x]["moduls"].length; y++) {
                            var m = new Modul(data[x]["moduls"][y]);
                            for (var z = 0; z < data[x]["moduls"][y]["exam"].length; z++) {
                                var e = new Exam(data[x]["moduls"][y]["exam"][z])
                                m.exams.push(e);
                            }
                            c.moduls.push(m);
                        }
                        mylist.push(c);
                    }
                    $("#treeGrade").empty();
                    mylist.forEach(function (i) {
                        console.log(i);
                        var modulhtml = "";
                        var examhtml = "";
                        var frontdiv = '<div class="tree-node tree-node-collapsed" id="' + i.id + '">';
                        var enddiv = "</div>"
                        var icon = '<i class="tree-node-icon"></i>';
                        var transparentIcon = '<i class="tree-node-icon transparent"></i>';
                        var frontUL = "<ul>";
                        var endUL = "</ul>";
                        var frontLI = "<li>";
                        var endLI = "</li>";
                        var classhtml = '<div class="tree-node-label" id="classview_' + i.id + '">' + i.name + '</div>';
                        for (var x = 0; x < i.moduls.length; x++) {
                            examhtml = "";
                            for (var y = 0; y < i.moduls[x].exams.length; y++) {
                                frontdiv = '<div class="tree-node tree-node-collapsed" id="' + i.moduls[x].exams[y].id + '_' + i.id + '">'
                                examhtml += frontLI + frontdiv + transparentIcon + '<div class="tree-node-label" id="examview_' + i.moduls[x].exams[y].id + '_' + i.id + '">' + i.moduls[x].exams[y].name + '</div>' + enddiv + endLI;
                            }
                            frontdiv = '<div class="tree-node tree-node-collapsed" id="' + i.id + '_' + i.moduls[x].id + '">'
                            modulhtml += frontLI + frontdiv + icon + '<div class="tree-node-label" id="modulview_' + i.id + '_' + i.moduls[x].id + '">' + i.moduls[x].name + '</div> ' + frontUL + examhtml + endUL + enddiv + endLI;

                        }
                        html += frontLI + frontdiv + icon + classhtml + frontUL + modulhtml + endUL + enddiv + endLI;
                    })
                    html += "</ul>";
                    appendTree(true);
                    toast.showToast("Der Tree wurde erfolgreich geladen");
                }
                else {
                    toast.showErrorToast("Der Tree konnte nicht geladen werden");
                }
            },
            error: function (XMLHttpRequest) {
                toast.showErrorToast("Status: " + XMLHttpRequest.status);
            }
        });
    } else {
        appendTree(firstshownId, true);
    }
}
function appendTree(firsttime) {
    if (firsttime) {
        AllClassesView();
    }
    $("#treeGrade").append(html);
    const treeNodes = document.querySelectorAll(".tree-node");
    treeNodes.forEach((node) => {
        node.addEventListener("click", (event) => {
            node.classList.toggle("tree-node-expanded");
            node.classList.toggle("tree-node-collapsed");

            var labelNode = node.querySelector('.tree-node-label');
            var labelNodeId = labelNode.getAttribute('id');
            var parts = labelNodeId.split('_');
            let functione, firstID, secondID;
            if (parts.length === 2) {
                [functione, firstID] = parts;
            } else if (parts.length === 3) {
                [functione, firstID, secondID] = parts;
            } 
            if (functione == "modulview") {
                modulview(firstID, secondID);
            } else if (functione == "examview") {
                examview(firstID, secondID);
            } else {
                if (node.classList.contains("tree-node-collapsed")) {
                    AllClassesView();
                } else {
                    classview(firstID);
                }
            }
            if (node.classList.contains("tree-node-expanded")) {
                var nodelabel = node.querySelector('.tree-node-label');
                nodelabel.classList.add('higlight');
            }
            if (node.classList.contains("tree-node-collapsed")) {
                var nodelabel = node.querySelector('.tree-node-label');
                nodelabel.classList.remove("higlight");
            }
            event.stopPropagation();
            event.preventDefault();
        });
    });
    firsttime = false;
}

var AllClassesViewHTML = "";
function AllClassesView() {
    AllClassesViewHTML = "";
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/getAllClassesFromTeacher",
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                var classliste = [];
                for (let x = 0; x < data.length; x++) {
                    var c = new Class(data[x]);
                    classliste.push(c)
                }
                var title = "<h1>Das sind Ihre Klassen:</h1>";
                var tablediv = '<div class="tablediv">';
                var table = '<table class="table table-hover">';
                var thead = '<thead class="text-light bg-dark">';
                var tds = '<td>Klasse</td>';
                var theadEnd = "</thead>";
                var tbody = '<tbody id="examStudentlist" class="text-light">';
                var tbodyInput = "";
                classliste.forEach(function (t) {
                    tbodyInput += "<tr onclick='toggleTree(`classview_" + t.id + "`)'><td>" + t.name + "</td></tr>";
                });
                var tbodyEnd = " </tbody>";
                var tableEnd = "</table>";
                var divEnd = "</div>";
                AllClassesViewHTML += title + tablediv + table + thead + tds + theadEnd + tbody + tbodyInput + tbodyEnd + tableEnd + divEnd
                appendView("AllClasse");
            }
        },
    });
}