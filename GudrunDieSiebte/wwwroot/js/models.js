class TeachingCompany {
    constructor(data) {
        this.id = data["id"];
        this.name = data["name"];
        this.city = data["city"];
        this.address = data["address"];
    }
}
class Absence {
    constructor(data) {
        this.id = data["id"];
        this.reason = data["reason"]
        this.starttime = data["startTime"];
        this.endtime = data["endTime"];
        this.confirm = data["confirmed"]
    }
}
class AbsenceVisible {
    constructor(data) {
        this.visible = data["isVisible"]
        this.id = data["id"];
        this.reason = data["reason"]
        this.starttime = data["startTime"];
        this.endtime = data["endTime"];
        this.confirm = data["confirmed"]
    }
}
class Appointment {
    constructor(data) {
        this.id = data["id"];
        this.reason = data["reason"]
        this.starttime = data["startTime"];
        this.endtime = data["endTime"];
    }
}
class Class {
    constructor(data) {
        this.id = data["id"];
        this.name = data["name"]
        this.moduls = [];
    }
}
class Classroom {
    constructor(data) {
        this.id = data["id"];
        this.floor = data["floor"];
        this.roomNumber = data["roomNumber"];
    }
}
class Exam {
    constructor(data) {
        this.id = data["id"];
        this.name = data["name"];
        this.highscore = data["highscore"]
        this.thema = data["thema"];
        this.examday = data["examday"];
        this.grades = [];
    }
}
class Grade {
    constructor(data) {
        this.id = data["id"];
        this.score = data["score"]
        this.grade = data["grade"] 
        this.isConfirmed = data["isConfirmed"]
    }
}
class ToDo {
    constructor(data) {
        this.id = data["id"];
        this.task = data["task"]
        this.upTo = data["upTo"]
    }
}
class Lesson {
    constructor(data) {
        this.id = data["id"];
        this.starttime = data["startTime"];
        this.endtime = data["endTime"];
    }
}
class ExamsWithAverage {
    constructor(data) {
        this.id = data["id"];
        this.name = data["name"];
        this.thema = data["thema"];
        this.examday = data["examday"];
        this.classAVG = data["classSpecificAverage"]
    }
}
class Modul {
    constructor(data) {
        this.id = data["id"];
        this.name = data["name"];
        this.description = data["description"];
        this.shedule = data["shedule"]
        this.exams = [];
    }
}
class Notification {
    constructor(data) {
        this.id = data["id"];
        this.message = data["message"];
        this.type = data["type"];
        this.createTime = data["createTime"];
    }
}
class SchoolManagement {
    constructor(data) {
        this.id = data["id"];
        this.schoolName = data["schoolName"];
    }
}
class Setting {
    constructor(data) {
        this.id = data["id"];
    }
}
class Person {
    constructor(data) {
        this.id = data["id"];
        this.fullname = data["fullName"];
        this.firstname = data["firstName"];
        this.lastname = data["lastName"];
        this.birthday = data["birthday"];
        this.address = data["address"];
        this.city = data["city"];
    }
}
class Student {
    constructor(data) {
        this.id = data["id"];
        this.enrollmentDate = data["enrollmentDate"];
        this.fullname = data["person"]["fullName"];
        this.firstname = data["person"]["firstName"];
        this.lastname = data["person"]["lastName"];
        this.birthday = data["person"]["birthday"];
        this.address = data["person"]["address"];
        this.city = data["person"]["city"];
    }
}
class Teacher {
    constructor(data) {
        this.id = data["id"];
        this.hireDate = data["hireDate"];
        this.fullname = data["person"]["fullName"];
        this.firstname = data["person"]["firstName"];
        this.lastname = data["person"]["lastName"];
        this.birthday = data["person"]["birthday"];
        this.address = data["person"]["address"];
        this.city = data["person"]["city"];
    }
}
class Classname {
    constructor(data) {
        this.id = data["id"];
        this.classname = data["classname"];
        this.moduls = [];
    }
}
class ModulsAVG {
    constructor(data) {
        this.modulname = data["modulname"];
        this.average = data["average"]
    }
} 
