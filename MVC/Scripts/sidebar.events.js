$("#menu-toggle").click(function(e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});
$("#menu-toggle-2").click(function(e) {
    e.preventDefault();
    toggleMenu("sidebar-wrapper1");
});

$("#menu-toggle-3").click(function(e) {
    e.preventDefault();
    toggleMenu("sidebar-wrapper2");
});

$("#menu-toggle-4").click(function(e) {
    e.preventDefault();
    toggleMenu("sidebar-wrapper3");
});

function initMenu() {
    $("#menu1 ul").hide();
    $("#menu1 ul").children(".current").parent().show();

    $("#menu2 ul").hide();
    $("#menu2 ul").children(".current").parent().show();

    $("#menu3 ul").hide();
    $("#menu3 ul").children(".current").parent().show();

    $("#menu1 li a").click(
        function() {
            var checkElement = $(this).next();
            if ((checkElement.is("ul")) && (checkElement.is(":visible"))) {
                return false;
            }
            if ((checkElement.is("ul")) && (!checkElement.is(":visible"))) {
                $("#menu1 ul:visible").slideUp("normal");
                checkElement.slideDown("normal");
                return false;
            }
            return false;
        }
    );
    $("#menu2 li a").click(
        function() {
            var checkElement = $(this).next();
            if ((checkElement.is("ul")) && (checkElement.is(":visible"))) {
                return false;
            }
            if ((checkElement.is("ul")) && (!checkElement.is(":visible"))) {
                $("#menu2 ul:visible").slideUp("normal");
                checkElement.slideDown("normal");
                return false;
            }
            return false;
        }
    );
    $("#menu3 li a").click(
        function() {
            var checkElement = $(this).next();
            if ((checkElement.is("ul")) && (checkElement.is(":visible"))) {
                return false;
            }
            if ((checkElement.is("ul")) && (!checkElement.is(":visible"))) {
                $("#menu3 ul:visible").slideUp("normal");
                checkElement.slideDown("normal");
                return false;
            }
            return false;
        }
    );
}

function toggleMenu(id) {
    isMenuOpen(id);
}

function isMenuOpen(id) {
    var transitionEnd = "transitionend webkitTransitionEnd oTransitionEnd otransitionend";
    //check if menu already open, if so close and open the right one
    if (!$("#wrapper").hasClass("toggled-2") && !$("#" + id).is(":visible")) {
        $("#wrapper").addClass("toggled-2").one(transitionEnd, function() {
            $.each($(".sidebar-wrapper"), function() {
                var thisId = $(this).attr("id");
                if (id !== thisId) {
                    if ($("#" + thisId).is(":visible")) {
                        $("#" + thisId).hide();
                    }
                }
            });
            openMenu(id);
            return false;
        });
    } else {
        if (!closeMenu(id)) {
            openMenu(id);
        }
    }
}

function openMenu(id) {
    //if no menu open - open the one we want
    if ($("#wrapper").hasClass("toggled-2")) {
        $.fx.off = true;
        $("#" + id).show("0", function() {
            $.fx.off = false;
            $("#wrapper").removeClass("toggled-2");
            //	$('#menu1 ul').show();
        });
        return;
    }
}

function closeMenu(id) {
    var transitionEnd = "transitionend webkitTransitionEnd oTransitionEnd otransitionend";
    ////Close if open
    if (!$("#wrapper").hasClass("toggled-2")) {
        $("#wrapper").addClass("toggled-2").one(transitionEnd, function() {
            $("#" + id).hide();
            //$('#menu1 ul').hide();
        });
        return true;
    }
    return false;
}