document.addEventListener("DOMContentLoaded", function (event) {

    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)

        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                nav.classList.toggle('show')
                toggle.classList.toggle('bx-x')
                bodypd.classList.toggle('body-pd')
                headerpd.classList.toggle('body-pd')
            })
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')

    /*const linkColor = document.querySelectorAll('.nav_link')*/

    //function colorLink() {
    //    if (linkColor) {
    //        linkColor.forEach(l => l.classList.remove('active'))
    //        this.classList.add('active')
    //    }
    //}
    //linkColor.forEach(l => l.addEventListener('click', colorLink))

});

$(function () {
    global.Ready();
    Page.Ready();
});

// The plugin
$.fn.json_beautify = function () {
    this.each(function () {
        var el = $(this),
            obj = JSON.parse(el.val()),
            pretty = JSON.stringify(obj, undefined, 4);
        el.val(pretty);
    });
};

var global = {

    Ready: function () {
        $(".nav-item").click(function () {
            $(".nav-item").removeClass("active");
            $(this).addClass("active");
        });
    }


}


