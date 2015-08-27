$(function () {
  
    initMenu();

    $('#wrapper').addClass('toggled-2');
    $('#sidebar-wrapper1').hide();
    $('#sidebar-wrapper2').hide();
    $('#sidebar-wrapper3').hide();

    $('#wrapper').show();
    $('#target').on('click', '.book-container', function (ev) {
        var id = $(this).attr('id');
        hideAll(id);
        var rowId = $('#folder-' + id).attr("data-rowId");
        var top = $(this).position().top;
        var newTop = top + 280;
        var theHeight = $('#folder-' + id).height();
        $('#folder-' + id).css("top", newTop + "px");
        $.each($('.row'), function () {
            var allRowId = $(this).attr('data-rowId');
            if (parseInt(allRowId) > parseInt(rowId - 1)) {
                $(this).css("top", theHeight + parseInt(30) + "px");
            }
        });
        if ($('#folder-' + id).is(':visible')) {
            $('#folder-' + id).hide();
            $.each($('.row'), function () {
                var allRowId = $(this).attr('data-rowId');
                if (parseInt(allRowId) > parseInt(rowId - 1)) {
                    $(this).css("top", "0px");
                }
            });
        } else {
            $('#folder-' + id).show();
        }
    });

    hideAll = function (idToExclude) {
        $.each($('.book-container'), function () {
            var id = $(this).attr('id');
            if (parseInt(id) !== parseInt(idToExclude)) {
                $(this).css("top", "0px");
                $('#folder-' + id).hide();
            }
        });
        $.each($('.row'), function () {
            $(this).css("top", "0px");
        });
    }
});

showXHRError = function (xhr, status, err) {
    alertify.set('notifier', 'position', 'top-right');
    alertify.error(formatErrorMessage(xhr));
}
formatErrorMessage = function(jqXHR) {

    if (jqXHR.status === 0) {
        return ('Not connected.\nPlease verify your network connection.');
    } else if (jqXHR.status == 404) {
        return ('The requested page not found. [404]');
    } else if (jqXHR.status == 500) {
        return ('Internal Server Error [500].');
    } else {
        return ('Uncaught Error.\n' + jqXHR.responseText);
    }
}
