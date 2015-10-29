var connection = $.hubConnection();

var proxy = connection.createHubProxy("ProgressHub");

connection.start();

$.connection.hub.logging = true;
// Create a function that the hub can call to broadcast messages.
proxy.on("updateProgress", function (progressState) {
    $("#status-list").prepend('<div class="ms-Table-row ms-ListItem-tertiaryText">' +
                       '<text class="ms-Table-cell"><i class="ms-fontColor-green ms-Icon ms-Icon--check buttonStyle" id=progress-' + progressState.RequestID + '></i></text>' +
                       '<div class="ms-Table-cell centerAlignTable"><span class="ms-ListItem-tertiaryText" style="top:7px">' + progressState.SiteTemplateName + '</span></div>' +
                       '<div class="ms-Table-cell centerAlignTable"><span class="ms-ListItem-tertiaryText" style="top:7px">' + progressState.StateString + '</span></div>' +
                       '<div class="ms-Table-cell centerAlignTable"><span class="ms-ListItem-tertiaryText" style="top:7px">' + progressState.StringTimeStamp + '</span></div>' +
                   '</div>');

    if (progressState.StateString === "Failed") {
        $("#" + progressState.RequestID).empty();
        $("#" + progressState.RequestID).append("<i class='ms-fontColor-red ms-Icon ms-Icon--xCircle buttonStyle'></i>");

        $('#progress-' + progressState.RequestID).removeClass("ms-fontColor-green ms-Icon ms-Icon--check");
        $("#progress-" + progressState.RequestID).addClass("ms-fontColor-red ms-Icon ms-Icon--xCircle");
    }
});

proxy.on("changePhoto", function (templateName) {
    $("#" + templateName).removeClass('ms-Icon--sortLines');
    $("#" + templateName).addClass('ms-Icon ms-Icon--question');
});