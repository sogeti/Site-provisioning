$(document).ready(function () {

    $(document).on('click', '#bgFile', function (e) {
        $('#bgFileUpload').click();
    });
    $(document).on('click', '#clrFile', function (e) {
        $('#clrFileUpload').click();
    });
    $(document).on('click', '#fontFile', function (e) {
        $('#fontFileUpload').click();
    });
    $(document).on('click', '#logoFile', function (e) {
        $('#logoFileUpload').click();
    });


    $(document).on('click', '#switchDefaultTemplate', function (e) {
        SetFiles();
    });

    SetFiles();

    $(document).on('change', '#bgFileUpload', function (e) {

        $('#bgFile').html('<img src=/Content/Images/01-progress.gif>');

        var _file = $('#bgFileUpload')[0].files[0];
        var form = $('#formId')[0];
        var formData = new FormData(form);
        formData.append('fileType', 'Background.jpg');
        var location = $('#switchDefaultTemplate').val();
        formData.append('location', location);
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/SiteTemplate/SaveExternFile',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (data) {
                if (data.pad.indexOf("http://") == 0 || data.pad.indexOf("https://") == 0) {
                    $("#bgFile").toggleClass('preview-failed preview-success');
                    $('#bgFile').html('<img src=' + data.pad + ' />');
                    $('#FileBgBlobLocation').val(data.pad);

                    $('div.upload-result-failed').text('');
                    if ($('#fontFile').hasClass('preview-success') && $('#logoFile').hasClass('preview-success') && $('#clrFile').hasClass('preview-success')) {
                        $('div.upload-result').html(_file.name + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                        $('div.upload-result-failed').text("");
                        $('.SubmitButton > .ms-Button').removeClass('is-disabled');
                    }
                } else {
                    $('#bgFile').html('<i class="ms-Icon ms-Icon--xCircle"></i>');
                }
            },
            error: function () {
                alert("failed");
            }
        });
    });

    $(document).on('change', '#clrFileUpload', function (e) {
        var _file = $('#clrFileUpload')[0].files[0];
        var form = $('#formId')[0];
        var formData = new FormData(form);
        formData.append('fileType', 'Color.spcolor');
        var location = $('#switchDefaultTemplate').val();
        formData.append('location', location);
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/SiteTemplate/SaveExternFile',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (data) {
                if (data.pad.indexOf("http://") == 0 || data.pad.indexOf("https://") == 0) {
                    $('#clrFile').toggleClass('preview-failed preview-success');
                    $('#clrFile').html('<img src=' + data.pad + ' />');
                    $('#FileClrBlobLocation').val(data.pad);

                    $('div.upload-result-failed').text('');
                    if ($('#fontFile').hasClass('preview-success') && $('#logoFile').hasClass('preview-success') && $('#bgFile').hasClass('preview-success')) {
                        $('div.upload-result').html(_file.name + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                        $('div.upload-result-failed').text("");
                        $('.SubmitButton > .ms-Button').removeClass('is-disabled');
                    }
                } else {
                    $('#clrFile').html('<i class="ms-Icon ms-Icon--fileDocument"></i>');
                }
            },
            error: function () {
                alert("failed");
            }
        });
    });

    $(document).on('change', '#fontFileUpload', function (e) {
        var _file = $('#fontFileUpload')[0].files[0];
        var form = $('#formId')[0];
        var formData = new FormData(form);
        formData.append('fileType', 'Font.spfont');
        var location = $('#switchDefaultTemplate').val();
        formData.append('location', location);
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/SiteTemplate/SaveExternFile',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (data) {
                if (data.pad.indexOf("http://") == 0 || data.pad.indexOf("https://") == 0) {
                    $('#fontFile').toggleClass('preview-failed preview-success');
                    $('#fontFile').html('<img src=' + data.pad + ' />');
                    $('#FontBlobLocation').val(data.pad);

                    $('div.upload-result-failed').text('');
                    if ($('#clrFile').hasClass('preview-success') && $('#logoFile').hasClass('preview-success') && $('#bgFile').hasClass('preview-success')) {
                        $('div.upload-result').html(_file.name + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                        $('div.upload-result-failed').text("");
                        $('.SubmitButton > .ms-Button').removeClass('is-disabled');
                    }
                } else {
                    $('#fontFile').html('<i class="ms-Icon ms-Icon--fontColor"></i>');
                }
            },
            error: function () {
                alert("failed");
            }
        });
    });

    $(document).on('change', '#logoFileUpload', function (e) {
        var _file = $('#logoFileUpload')[0].files[0];
        
        var form = $('#formId')[0];
        var formData = new FormData(form);
        formData.append('fileType', 'Logo.png');
        var location = $('#switchDefaultTemplate').val();
        formData.append('location', location);
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '/SiteTemplate/SaveExternFile',
            data: formData,
            processData: false,
            contentType: false,
            dataType: "json",
            success: function (data) {
                if (data.pad.indexOf("http://") == 0 || data.pad.indexOf("https://") == 0) {
                    $('#logoFile').toggleClass('preview-failed preview-success');
                    $('#logoFile').html('<img src=' + data.pad + ' />');
                    $('#LogoBlobLocation').val(data.pad);

                    $('div.upload-result-failed').text('');
                    if ($('#clrFile').hasClass('preview-success') && $('#fontFile').hasClass('preview-success') && $('#bgFile').hasClass('preview-success')) {
                        $('div.upload-result').html(_file.name + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                        $('div.upload-result-failed').text("");
                        $('.SubmitButton > .ms-Button').removeClass('is-disabled');
                    }
                } else {
                    $('#logoFile').html('<i class="ms-Icon ms-Icon--globe"></i>');
                }
            },
            error: function () {
                alert("failed");
            }
        });
    });


});


function SetFiles() {
    SetFile("#bgFile", "Background.jpg", "#FileBgBlobLocation", "ms-Icon--picture");
    SetFile("#logoFile", "Logo.png", "#LogoBlobLocation", "ms-Icon--picture");
    SetFile("#clrFile", "Color.spcolor", "#FileClrBlobLocation", "ms-Icon--paint");
    SetFile("#fontFile", "Font.spfont", "#FontBlobLocation", "ms-Icon--fontColor");
}

function SetFile( idtoSet,  backgroundFile,  idValue, icon) {
    var defaultLocation = $('#switchDefaultTemplate').is(":checked");
    var fileValue = $(idValue).val();


        $.getJSON('/SiteTemplate/GetFileLocation', {
                folder: $('#SiteTemplateGuid').val(),
                file: backgroundFile,
                defaultloc: defaultLocation
            })
            .done(function(data) {
                if (data !== "") {
                    $(idtoSet).toggleClass('preview-failed preview-success');
                    $(idtoSet).html('<img src=' + data + ' />');
                    $(idValue).val(data);
                } else {
                    $(idtoSet).addClass('preview-failed');
                    $(idtoSet).removeClass('preview-success');
                    $(idtoSet).html('<i class="ms-Icon ' + icon + '"></i>');
                    $(idValue).val();
                }
            })
            .fail(function(jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.log("Request Failed: " + err);
            });

}


(function($) {
    var _listID = '';

    function addUser(listID, name, imgSrc) {
        var html = '<div class="ms-listItem twoPixDown">' +
            '<div class="ms-Persona ms-Persona--xs">' +
            '<div class="ms-Persona-imageArea">' +
            '<i class="ms-Persona-placeholder ms-Icon ms-Icon--person"></i>' +
            '<img class="ms-Persona-image" src="' + imgSrc + '"/>' +
            '</div>' +
            '<div class="ms-Persona-details">' +
            '<div class="ms-Persona-primaryText">' + name + '</div>' +
            '</div>' +
            '</div>';
        $(listID).append(html);
    };

    $('#userDlg').hide();
    $(document).on('click', '#cancelDlgBtn', function(e) {
        $('#userDlg').hide();
        _listID = '';
    });
    $(document).on('click', '#confirmDlgBtn', function(e) {
        //Save Users to model
        $('.ms-PeoplePicker-searchBox > .ms-PeoplePicker-persona').each(function() {
            var selectedName = $(this).find(".ms-Persona-primaryText").html();
            var selectedImg = $(this).find(".ms-Persona-image").attr('src');
            console.log(selectedName + ' + ' + selectedImg);
            addUser(_listID, selectedName, selectedImg);

        });

        $('#userDlg').hide();
        _listID = '';
    });
    $(document).on('click', '.addPersonIcon', function (e) {
        $('.ms-PeoplePicker-searchBox > .ms-PeoplePicker-persona').remove();
        _listID = '#' + $(this).parent().parent().attr('id');
        $('#userDlg').show();
    });
})(jQuery);
