
const targetNode = document.getElementById('toastPlacement_bottomRight');
const config = { attributes: false, childList: true, subtree: false };

const callback = function (mutationList, observer) {
    for (const mutation of mutationList) {
        if (mutation.type === 'childList') {
            if (mutation.target.children.length > 3) {
                console.log(mutation.target)
                console.log(mutation.target.children.length)
                mutation.target.innerHTML = "";
            }
        }
    }
};
const observer = new MutationObserver(callback);
observer.observe(targetNode, config);
var x = 0;
class MyToast {

    constructor(id) {
        x += 1;
        this.toast = $("<div>", { id: "toast_" + id +"_"+ x, "class": "toast text-dark", "role": "alert", "aria-live": "assertive", "aria-atomic": "true" });
        this.toastHeader = $("<div>", { "class": "toast-header text-primary"});
        this.toastIcon = $("<i>", { "class": "las la-info icon" });
        this.toastdiv = $("<div>", { "class": "me-auto" });
        this.textBeforeMsg = $("<strong>");
        this.location = $("<div>");
        this.toastBtn = $("<button>", { "class": "btn-close", "type": "button", "data-bs-dismiss": "toast", "aria-label": "Close" });
        this.toastBody = $("<div>", { "class": "toast-body" });
        this.toastdiv.append(this.textBeforeMsg, this.location);
        this.toastHeader.append(this.toastIcon, this.toastdiv, this.toastBtn);
        this.toast.append(this.toastHeader, this.toastBody);
        $("#toastPlacement_bottomRight").append(this.toast);
        this.toast.on('hidden.bs.toast', function () {
            $("#toast_" + id + "_" + x).remove();
        })
    }
    resetToast() {
        this.toastIcon.removeClass('la-exclamation');
        this.toastHeader.removeClass('text-danger');
        this.toastIcon.removeClass('la-info');
        this.toastHeader.removeClass('text-primary');
    }
    setLocation() {
        var currentlocation = window.location.pathname;
        var showedlocation = currentlocation.replace("/", "")
        this.location.text(showedlocation);
    }

    showToast(message) {
        this.resetToast();
        this.toastIcon.addClass('la-info');
        this.toastHeader.addClass('text-primary');
        this.toastBody.text(message);
        this.setLocation();
        this.textBeforeMsg.text("Information zu ");
        this.toast.toast("show");
    }

    showErrorToast(message) {
        this.resetToast();
        this.toastIcon.addClass('la-exclamation');
        this.toastHeader.addClass('text-danger');
        this.toastBody.text(message);
        this.setLocation();
        this.textBeforeMsg.text("Fehlermeldung in ");
        this.toast.toast("show");
    }
}


class MyModal {

    static modal;
    static modalTitle;
    static modalBody;
    static modalFooter;

    static {
        this.modal = bootstrap.Modal.getOrCreateInstance($("#Modal")) 
        this.modalTitle = $("#Modal .modal-title");
        this.modalBody = $("#Modal .modal-body");
        this.modalFooter = $("#Modal .modal-footer");
    }

    static showModal(title, body, footer) {
        this.modalTitle.text(title);
        this.modalBody.html(body);
        this.modalFooter.html(footer);
       
        if (!$("#Modal").hasClass('show')) {
            this.modal.show();
        }
    }
    static hideModal() {
        this.modal.hide();
    }
    
}                  
class Converter {
    constructor(text) {
        this.text = text;
        this.convertModulLink();
    }
    convertModulLink() {
        let count = (this.text.match(new RegExp(/M[0-9]{3}/, "g")) || []).length;
        for (let x = 0; x < count; x++) {
            var pos = this.text.search(/[^>]M[0-9]{3}/);
            var modul = this.text.substr(pos + 1, 4)
            var modulnummer = modul.slice(1);
            var modullink = "<a href='https://localhost:7138/General/Modullist/" + modulnummer + "' class='modullink'>" + modul + "</a>"
            this.text = this.text.replace(modul, modullink);
        }
    }
    toString() {
        return this.text;
    }
}