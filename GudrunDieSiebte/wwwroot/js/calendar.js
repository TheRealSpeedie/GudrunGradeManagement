if (window.location.pathname == "/Student/Timetable") {
    document.addEventListener('DOMContentLoaded', function () {
        var toast = new MyToast("CalendarToast")
        var calendarEl = document.getElementById('calendar');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            themeSystem: 'bootstrap5',
            initialView: 'dayGridMonth',
            local: 'de',
            timeZone: 'Europe/Zurich',
            firstDay: 1,
            dayMaxEvents: 1,
            headerToolbar: {
                start: 'prevYear,prev today next,nextYear',
                center: 'title',
                end: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
            },
            customButtons: {
                myCustomButton: {
                    text: 'Kalender herunterladen',
                    click: function () {
                        var toast = new MyToast("getCalendar");
                        $.ajax({
                            method: "GET",
                            url: "https://" + window.location.host + "/Lessons/GetDataForDownload",
                            data: {},
                            success: function (response) {
                                if (response["success"]) {
                                    var data = response["data"];
                                    // Create a new link
                                    const anchor = document.createElement('a');
                                    anchor.href = "https://" + window.location.host + data;
                                    anchor.download = "Kalender.ics";

                                    // Append to the DOM
                                    document.body.appendChild(anchor);

                                    // Trigger `click` event
                                    anchor.click();

                                    // Remove element from DOM
                                    document.body.removeChild(anchor);
                                    toast.showToast("Der Kalender wurde erfolgreich heruntergeladen");
                                }
                                else {
                                    toast.showErrorToast("Der Kalender konnte nicht heruntergeladen werden");
                                }
                            },
                            error: function (XMLHttpRequest) {
                                toast.showErrorToast("Status: " + XMLHttpRequest.status);
                            }
                        });
                    }
                }
            },
            footerToolbar: {
                start: 'myCustomButton',
                center: '',
                end: ''
            },
            buttonText: {
                today: 'Heute',
                month: 'Monat',
                week: 'Woche',
                day: 'Tag',
                list: 'Liste'
            },
            timeFormat: 'HH:mm',
            height: 700,
            aspectRatio: 1,
            windowResize: function (arg) {
                if (screen.width < 500) {
                    calendar.setOption('headerToolbar', {
                        start: 'prev,next today',
                        center: 'title',
                        end: ''
                    });
                    calendar.setOption('footerToolbar', {
                        start: 'myCustomButton',
                        center: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek',
                        end: ''
                    })
                    calendar.setOption('height', 500)
                } else {
                    calendar.setOption('headerToolbar', {
                        start: 'prevYear,prev today next,nextYear',
                        center: 'title',
                        end: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                    });
                    calendar.setOption('footerToolbar', {
                        start: 'myCustomButton',
                        center: '',
                        end: ''
                    })
                    calendar.setOption('height', 700)
                }
            },
            events: {
                url: "https://" + window.location.host + "/Lessons/GetLessonFromUser",
                error: function () {
                    toast.showErrorToast("Hier ist etwas schief gelaufen!");
                },
                sucess: function () {
                    toast.showToast("Der Stundenplan wurde erfolgreich geladen");
                },
                loading: function (bool) {
                    if (bool) {
                        console.log("Lädt");
                    }
                },
            },
        });
        calendar.render();
    });
}