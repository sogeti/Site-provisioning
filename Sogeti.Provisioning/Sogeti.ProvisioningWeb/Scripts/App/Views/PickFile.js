
    $(document).ready(function () {
        $('.SubmitButton > .ms-Button').addClass('is-disabled');

        $(document).on('click', '#templateFileUploadBtn', function (e) {
            $('#PnpTemplateFileUpload').click();
        });
    });

$(document).on('change', '#PnpTemplateFileUpload', function (e) {
    var file = $('#PnpTemplateFileUpload')[0].files[0];
    var form = $('#formId')[0];
    var formData = new FormData(form);
    ValidateTempl(file, formData);
});


function ValidateTempl(file, formData) {
    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: '/SiteTemplate/ValidateTemplate',
        data: formData,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (data) {
            console.log(data.Result + "  " + data.Message);
            if (data.Result === "OK") {
                $('div.upload-result').html("Selected template: " + file.name + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                $('div.upload-result-failed').text('');
                $('.SubmitButton > .ms-Button').removeClass('is-disabled');
            }
            else {
                $('div.upload-result').html(file.name + ' <i class=\'ms-Icon ms-Icon--xCircle ms-fontColor-red\'></i>');
                $('div.upload-result-failed').html(data.Message);
                $('.SubmitButton > .ms-Button').addClass('is-disabled');
            }
        },
        error: function () {
            alert("failed");
        }
    });
}
 

// https://github.com/OfficeDev/Office-UI-Fabric/blob/master/dist/components/Dropdown/Jquery.Dropdown.js
// <start>copy from Office UI Fabric source...


// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See LICENSE in the project root for license information.

    
    (function ($) {
        $.fn.Dropdown = function () {

            /** Go through each dropdown we've been given. */
            return this.each(function () {

                var $dropdownWrapper = $(this),
                    $originalDropdown = $dropdownWrapper.children('.ms-Dropdown-select'),
                    $originalDropdownOptions = $originalDropdown.children('option'),
                    originalDropdownID = this.id,
                    newDropdownTitle = '',
                    newDropdownItems = '',
                    newDropdownSource = '';

                /** Go through the options to fill up newDropdownTitle and newDropdownItems. */
                $originalDropdownOptions.each(function (index, option) {

                    /** If the option is selected, it should be the new dropdown's title. */
                    if (option.selected) {
                        newDropdownTitle = option.text;
                    }

                    /** Add this option to the list of items. */
                    newDropdownItems += '<li class="ms-Dropdown-item' + ((option.disabled) ? ' is-disabled"' : '"') + '>' + option.text + '</li>';

                });

                /** Insert the replacement dropdown. */
                newDropdownSource = '<span class="ms-Dropdown-title">' + newDropdownTitle + '</span><ul class="ms-Dropdown-items">' + newDropdownItems + '</ul>';
                $dropdownWrapper.append(newDropdownSource);

                function _openDropdown(evt) {
                    if (!$dropdownWrapper.hasClass('is-disabled')) {

                        /** First, let's close any open dropdowns on this page. */
                        $dropdownWrapper.find('.is-open').removeClass('is-open');

                        /** Stop the click event from propagating, which would just close the dropdown immediately. */
                        evt.stopPropagation();

                        /** Before opening, size the items list to match the dropdown. */
                        var dropdownWidth = $(this).parents(".ms-Dropdown").width();
                        $(this).next(".ms-Dropdown-items").css('width', dropdownWidth + 'px');

                        /** Go ahead and open that dropdown. */
                        $dropdownWrapper.toggleClass('is-open');

                        /** Temporarily bind an event to the document that will close this dropdown when clicking anywhere. */
                        $(document).bind("click.dropdown", function (event) {
                            $dropdownWrapper.removeClass('is-open');
                            $(document).unbind('click.dropdown');
                        });
                    }
                };

                /** Toggle open/closed state of the dropdown when clicking its title. */
                $dropdownWrapper.on('click', '.ms-Dropdown-title', function (event) {
                    _openDropdown(event);
                });

                /** Keyboard accessibility */
                $dropdownWrapper.on('keyup', function (event) {
                    var keyCode = event.keyCode || event.which;
                    // Open dropdown on enter or arrow up or arrow down and focus on first option
                    if (!$(this).hasClass('is-open')) {
                        if (keyCode === 13 || keyCode === 38 || keyCode === 40) {
                            _openDropdown(event);
                            if (!$(this).find('.ms-Dropdown-item').hasClass('is-selected')) {
                                $(this).find('.ms-Dropdown-item:first').addClass('is-selected');
                            }
                        }
                    }
                    else if ($(this).hasClass('is-open')) {
                        // Up arrow focuses previous option
                        if (keyCode === 38) {
                            if ($(this).find('.ms-Dropdown-item.is-selected').prev().siblings().size() > 0) {
                                $(this).find('.ms-Dropdown-item.is-selected').removeClass('is-selected').prev().addClass('is-selected');
                            }
                        }
                        // Down arrow focuses next option
                        if (keyCode === 40) {
                            if ($(this).find('.ms-Dropdown-item.is-selected').next().siblings().size() > 0) {
                                $(this).find('.ms-Dropdown-item.is-selected').removeClass('is-selected').next().addClass('is-selected');
                            }
                        }
                        // Enter to select item
                        if (keyCode === 13) {
                            if (!$dropdownWrapper.hasClass('is-disabled')) {

                                // Item text
                                var selectedItemText = $(this).find('.ms-Dropdown-item.is-selected').text()

                                $(this).find('.ms-Dropdown-title').html(selectedItemText);

                                /** Update the original dropdown. */
                                $originalDropdown.find("option").each(function (key, value) {
                                    if (value.text === selectedItemText) {
                                        $(this).prop('selected', true);
                                    } else {
                                        $(this).prop('selected', false);
                                    }
                                });
                                $originalDropdown.change();

                                $(this).removeClass('is-open');
                            }
                        }
                    }

                    // Close dropdown on esc
                    if (keyCode === 27) {
                        $(this).removeClass('is-open');
                    }
                });

                /** Select an option from the dropdown. */
                $dropdownWrapper.on('click', '.ms-Dropdown-item', function () {
                    if (!$dropdownWrapper.hasClass('is-disabled')) {

                        /** Deselect all items and select this one. */
                        $(this).siblings('.ms-Dropdown-item').removeClass('is-selected')
                        $(this).addClass('is-selected');

                        /** Update the replacement dropdown's title. */
                        $(this).parents().siblings('.ms-Dropdown-title').html($(this).text());

                        /** Update the original dropdown. */
                        var selectedItemText = $(this).text();
                        $originalDropdown.find("option").each(function (key, value) {
                            if (value.text === selectedItemText) {
                                $(this).prop('selected', true);
                            } else {
                                $(this).prop('selected', false);
                            }
                        });
                        $originalDropdown.change

                        if (selectedItemText == '[Choose template or upload a new one]') {
                            $('div.upload-result').html('');
                            $('.SubmitButton > .ms-Button').addClass('is-disabled');
                        } else {
                            $('div.upload-result').html("Selected template: " + selectedItemText + ' <i class=\'ms-Icon ms-Icon--check ms-fontColor-green\'></i>');
                            $('.SubmitButton > .ms-Button').removeClass('is-disabled');
                        }

                    }
                });

            });
        };
    })(jQuery);
// <stop>copy from Office UI Fabric source...

if ($.fn.Dropdown) {
    $('.ms-Dropdown').Dropdown();
}
