'use strict';

function showElements(idShow, idHide, classNames) {
    var show = document.getElementById(idShow);
    var hide = document.getElementById(idHide);

    show.style.display = 'none';
    hide.style.display = 'inline';

    if (classNames) {
        var elements = document.getElementsByClassName(classNames);

        for (var i = 0; i < elements.length; i++) {
            if (elements[i].style.display === 'none') {
                elements[i].style.display = elements[i].style.org_display;
            }
        }
    }
}

function hideElements(idShow, idHide, classNames) {
    var show = document.getElementById(idShow);
    var hide = document.getElementById(idHide);

    show.style.display = 'inline';
    hide.style.display = 'none';

    if (classNames) {
        var elements = document.getElementsByClassName(classNames);

        for (var i = 0; i < elements.length; i++) {
            if (elements[i].style.display !== 'none') {
                elements[i].style.org_display = elements[i].style.display;
                elements[i].style.display = 'none';
            }
        }
    }
}

function showOrHideElement(idToggle, idShow, idHide, classNames) {
    var eToggle = document.getElementById(idToggle);

    if (eToggle.style.display === 'none') {
        eToggle.style.display = 'block';
        showElements(idShow, idHide);
    } else {
        eToggle.style.display = 'none';
        hideElements(idShow, idHide);
    }
}

