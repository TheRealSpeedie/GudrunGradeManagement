const modulsClassnames = [];
function getGradesModulsFromClass() {
    var toast = new MyToast("getGradesModulsFromClass");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/getGradesModulsClass",
        data: {},
        success: function (response) {
            if (response["success"]) {
                $("#Gradelist").empty();
                var data = response["data"];
                for (var x = 0; x < data.length; x++) {
                    var cname = new Classname(data[x]);
                    for (var y = 0; y < data[x]["moduls"].length; y++) {
                        var e = new ModulsAVG(data[x]["moduls"][y])
                        cname.moduls.push(e);
                    }
                    modulsClassnames.push(cname);
                }
                modulsClassnames.forEach(function (t) {
                    $("#Gradelist").append(
                        `<tr id="tr_` + t.id + `">
                            <td id="class_`+ t.classname + `">` + t.classname + `<i onclick='GetDataForGradeAppend(` + t.id + `, "` + t.classname + `")' id="btn_` + t.id + `" class="las la-chevron-circle-down"></i></td>
                            <td id="mod_`+ t.classname + `"></td>
                            <td id="avg_`+ t.classname + `"></td>
                            <td id="add_`+ t.classname + `"></td>
                        </tr>`
                    );
                })
                toast.showToast("Die Noten wurden erfolgreich geladen");
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
const modulsGrades = [];
function GetDataForGradeAppend(id, classe) {
    $("#btn_" + id).attr("onclick", "untie(`grade`," + id + ",'" + classe + "')");
    $.ajax({
        method: "GET",
        url: "https://" + window.location.host + "/Classes/getModulsByClass/" + id,
        data: {},
        success: function (response) {
            if (response["success"]) {
                var data = response["data"];
                    for (var y = 0; y < data["moduls"].length; y++) {
                        var e = new ModulsAVG(data["moduls"][y])
                        modulsGrades.push(e);
                    }
            }
            append("grade", id, classe);
        }
    })
}


