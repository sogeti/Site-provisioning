Provision.Helpers.globalEvents = (function ($) {
    var setStyling = function () {
        //Set the style of the client web part page to be consistent with the host web.
        //DON'T wait for the document ready. It takes too long
        var hostUrl = '';
        if (document.URL.indexOf('?') != -1) {
            var params = document.URL.split('?')[1].split('&');
            for (var i = 0; i < params.length; i++) {
                var p = decodeURIComponent(params[i]);
                if (/^SPHostUrl=/i.test(p)) {
                    hostUrl = p.split('=')[1];
                    document.write('<link rel="stylesheet" href="' + hostUrl + '/_layouts/15/defaultcss.ashx" />');
                    break;
                }
            }
        }
        if (hostUrl == '') {
            document.write('<link rel="stylesheet" href="/_layouts/15/1033/styles/themable/corev15.css" />');
        }

        //Get the DisplayType from url parameters
        var displayType = decodeURIComponent(getQueryStringParameter('DisplayType'));

        //Get the URI decoded SharePoint site url from the SPHostUrl parameter.
        var spHostUrl = decodeURIComponent(getQueryStringParameter('SPHostUrl'));

        //Build absolute path to the layouts root with the spHostUrl
        var layoutsRoot = spHostUrl + '/_layouts/15/';

        //Execute the correct script based on the displayType
        if (displayType == 'FullScreen') {
            //Load the SP.UI.Controls.js file to render the App Chrome
            $.getScript(layoutsRoot + 'SP.UI.Controls.js', renderSPChrome);
        }
        else { // if (displayType == 'iframe') {
            //Create a Link element for the defaultcss.ashx resource
            var linkElement = document.createElement('link');
            linkElement.setAttribute('rel', 'stylesheet');
            linkElement.setAttribute('href', layoutsRoot + 'defaultcss.ashx');

            //Add the linkElement as a child to the head section of the html
            var headElement = document.getElementsByTagName('head');
            headElement[0].appendChild(linkElement);
        }

        function renderSPChrome() {
            //Get the host site logo url from the SPHostLogoUrl parameter
            var hostlogourl = decodeURIComponent(getQueryStringParameter('SPHostLogo'));
            //Set the chrome options for launching Help, Account, and Contact pages
            var options = {
                'appIconUrl': hostlogourl,
                'appTitle': document.title
            };
            //Load the Chrome Control in the divSPChrome element of the page
            var chromeNavigation = new SP.UI.Controls.Navigation('divSPChrome', options);
            chromeNavigation.setVisible(true);
        };

        function getQueryStringParameter(urlParameterKey) {

            var params = document.URL.split('?');
            if (params.length == 1) return;

            var params = document.URL.split('?')[1].split('&');

            var strParams = '';
            for (var i = 0; i < params.length; i = i + 1) {
                var singleParam = params[i].split('=');
                if (singleParam[0] == urlParameterKey)
                    return singleParam[1];
            }
        }
    };

    setStyling();

    return {
        setStyling: setStyling
    };
})(jQuery);