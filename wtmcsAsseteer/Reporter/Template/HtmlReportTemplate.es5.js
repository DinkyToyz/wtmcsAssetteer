'use strict';

var logToConsole = true;

function quotedParameterString(parameter, unused) {
    if (unused || typeof parameter === 'undefined' || parameter === null) {
        return 'null';
    } else {
        return "'" + parameter.replace("'", "\\'") + "'";
    }
}

function setDisplayProperty(element, displayProperty, useOriginalDisplayProperty) {
    if (!(element.style.display === displayProperty)) {
        if (displayProperty === 'none') {
            if (useOriginalDisplayProperty && element.style.display && element.style.display !== 'none') {
                element.style.original_display_property = element.style.display;
            }

            element.style.display = displayProperty;
        } else if (useOriginalDisplayProperty && element.style.original_display_property) {
            element.style.display = element.style.original_display_property;
        } else {
            element.style.display = displayProperty;
        }
    }
}

function setDisplayPropertyOnElement(idElement, displayProperty, useOriginalDisplayProperty) {
    if (idElement) {
        var element = document.getElementById(idElement);

        if (element) {
            setDisplayProperty(element, displayProperty, useOriginalDisplayProperty);
        }
    }
}

function setDisplayPropertyOnElements(classNamesElements, displayProperty, useOriginalDisplayProperty) {
    if (classNamesElements) {
        var classNames = classNamesElements.split(/[\s,;]+/);

        for (var j = 0; j < classNames.length; j++) {
            if (logToConsole) console.log('SDPOEs', classNames[j], displayProperty, useOriginalDisplayProperty);

            var elements = document.getElementsByClassName(classNames[j]);

            if (elements) {
                for (var i = 0; i < elements.length; i++) {
                    if (logToConsole) console.log('sDPOEs', elements[i]);

                    if (elements[i]) {
                        setDisplayProperty(elements[i], displayProperty, useOriginalDisplayProperty);
                    }
                }
            }
        }
    }
}

function showAndHideElements(idShowButton, idHideButton, idShowElement, idHideElement, classNamesShowElements, classNamesHideElements) {
    if (logToConsole) console.log('SAHEs', idShowButton, idHideButton, classNamesShowElements, classNamesHideElements);

    setDisplayPropertyOnElement(idShowButton, "none", false);
    setDisplayPropertyOnElement(idHideButton, 'inline', false);

    setDisplayPropertyOnElement(idShowElement, 'inline', true);
    setDisplayPropertyOnElement(idHideElement, 'none', true);

    setDisplayPropertyOnElements(classNamesShowElements, 'inline', true);
    setDisplayPropertyOnElements(classNamesHideElements, 'none', true);
}

function showElements(idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements) {
    if (logToConsole) console.log('SE', idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements);

    showAndHideElements(idShowButton, idHideButton, idHideable, null, classNamesShowElements, classNamesHideElements);
}

function hideElements(idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements) {
    if (logToConsole) console.log('HE', idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements);

    showAndHideElements(idHideButton, idShowButton, null, idHideable, classNamesHideElements, classNamesShowElements);
}

function showOrHideElement(idShowButton, idHideButton, idHideableElement, classNamesShowElements, classNamesHideElements) {
    if (idHideableElement) {
        var element = document.getElementById(idHideableElement);

        if (element) {
            if (element.style.display === 'none') {
                setDisplayProperty(element, 'block', true);
                showElements(idShowButton, idHideButton, null, classNamesShowElements, classNamesHideElements);
            } else {
                setDisplayProperty(element, 'none', true);
                hideElements(idShowButton, idHideButton, null, classNamesShowElements, classNamesHideElements);
            }
        }
    }
}

function initializeSingleShowHideToggleButtons(hideOnInit, autoHideButtons, idToggleButton, idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements) {
    if (idToggleButton) {
        var button = document.getElementById(idToggleButton);

        if (button) {
            button.href = 'javascript:showOrHideElement(' + quotedParameterString(idShowButton, !autoHideButtons) + ',' + quotedParameterString(idHideButton, !autoHideButtons) + ',' + quotedParameterString(idHideable) + ',' + quotedParameterString(classNamesShowElements) + ',' + quotedParameterString(classNamesHideElements) + ')';

            if (logToConsole) console.log('ISSHB', idShowButton, button.href);

            if (hideOnInit) {
                hideElements(idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements);
            } else if (autoHideButtons) {
                showElements(idShowButton, idHideButton);
            }
        }
    }
}

function initializeSingleShowHideButtons(hideOnInit, autoHideButtons, idShowButton, idHideButton, idHideable, classNamesShowElements, classNamesHideElements) {
    var button;

    if (idShowButton) {
        button = document.getElementById(idShowButton);
        if (button) {
            button.href = 'javascript:showElements(' + quotedParameterString(idShowButton, !autoHideButtons) + ',' + quotedParameterString(idHideButton, !autoHideButtons) + ',' + quotedParameterString(idHideable) + ',' + quotedParameterString(classNamesShowElements) + ',' + quotedParameterString(classNamesHideElements) + ')';

            if (autoHideButtons && !hideOnInit) {
                buttons.style.display = 'none';
            }
        }
    }

    if (idHideButton) {
        button = document.getElementById(idHideButton);
        if (button) {
            button.href = 'javascript:hideElements(' + quotedParameterString(idShowButton, !autoHideButtons) + ',' + quotedParameterString(idHideButton, !autoHideButtons) + ',' + quotedParameterString(idHideable) + ',' + quotedParameterString(classNamesShowElements) + ',' + quotedParameterString(classNamesHideElements) + ')';

            if (hideOnInit) {
                button.onclick();
            }
        }
    }
}

function initializeMultiShowHideToggleButtons(hideOnInit, autoHideButtons, idAttriute, showButtonPrefix, hideButtonPrefix, hideableElementPrefix, classNamesShowButtons, classNamesHideButtons) {
    if (logToConsole) console.log('IMSHTB', hideOnInit, autoHideButtons, idAttriute, showButtonPrefix, hideButtonPrefix, hideableElementPrefix, classNamesShowButtons, classNamesHideButtons);

    var i, j;
    var id;
    var buttons;
    var classNames;

    if (classNamesShowButtons) {
        if (logToConsole) console.log('IMSHTB', classNamesShowButtons);

        classNames = classNamesShowButtons.split(/[\s,;]+/);

        for (j = 0; j < classNames.length; j++) {
            buttons = document.getElementsByClassName(classNames[j]);

            if (buttons) {
                for (i = 0; i < buttons.length; i++) {
                    if (buttons[i]) {
                        id = buttons[i].getAttribute(idAttriute);
                        if (id) {

                            buttons[i].href = 'javascript:showElements(' + quotedParameterString(showButtonPrefix + id, !autoHideButtons) + ',' + quotedParameterString(hideButtonPrefix + id, !autoHideButtons) + ',' + quotedParameterString(hideableElementPrefix + id) + ')';

                            if (logToConsole) console.log('IMSHTB', classNames[j], id, buttons[i].href);

                            if (autoHideButtons && !hideOnInit) {
                                buttons[i].style.display = 'none';
                            }
                        }
                    }
                }
            }
        }
    }

    if (classNamesHideButtons) {
        if (logToConsole) console.log('IMSHTB', classNamesShowButtons);

        classNames = classNamesHideButtons.split(/[\s,;]+/);

        for (j = 0; j < classNames.length; j++) {
            buttons = document.getElementsByClassName(classNames[j]);

            if (buttons) {
                for (i = 0; i < buttons.length; i++) {
                    if (buttons[i]) {
                        id = buttons[i].getAttribute(idAttriute);
                        if (id) {
                            var url = 'hideElements(' + quotedParameterString(showButtonPrefix + id, !autoHideButtons) + ',' + quotedParameterString(hideButtonPrefix + id, !autoHideButtons) + ',' + quotedParameterString(hideableElementPrefix + id) + ')';

                            buttons[i].href = 'javascript:' + url;
                            if (logToConsole) console.log('IMSHTB', classNames[j], id, buttons[i].href);

                            if (hideOnInit) {
                                eval(url);
                            }
                        }
                    }
                }
            }
        }
    }
}

window.onload = function () {
    initializeSingleShowHideToggleButtons(false, true, 'top-table-showhide', 'top-table-show', 'top-table-hide', 'top-table-hideable');
    initializeSingleShowHideButtons(false, false, 'dep-text-show', 'dep-text-hide', null, 'deptxt dephide', 'depshow');
    initializeSingleShowHideButtons(false, false, 'ref-text-show', 'ref-text-hide', null, 'reftxt refhide', 'refshow');
    initializeMultiShowHideToggleButtons(true, true, 'shbid', 'show', 'hide', 'txt', 'depshow', 'dephide');
    initializeMultiShowHideToggleButtons(true, true, 'shbid', 'show', 'hide', 'txt', 'refshow', 'refhide');
};

