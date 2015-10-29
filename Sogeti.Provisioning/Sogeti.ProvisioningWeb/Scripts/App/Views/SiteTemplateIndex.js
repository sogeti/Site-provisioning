
    $(document).ready(function () {
        $('.ms-Dialog').hide();

        $(document).on('click', '#deleteTemplateBtn', function (e) {
            $('.ms-Dialog').fadeIn();
        });
        $(document).on('click', '.cancelModal', function (e) {
            $('.ms-Dialog').fadeOut();
            e.preventDefault();
        });
        $(document).on('click', '.confirmModal', function (e) {
            var _id = $('.confirmModal').attr("id");
            var form = $('#formId')[0];
            var formData = new FormData(form);
            formData.append('id', _id);
            $.ajax({
                cache: false,
                async: true,
                type: "POST",
                url: '/SiteTemplate/Delete',
                data: formData,
                processData: false,
                contentType: false,
                dataType: "json",
                success: function (data) {
                    alert("Template " + _id + " is deleted.")
                    $('.ms-Dialog').fadeOut();
                    e.preventDefault();
                },
                error: function () {
                    alert("failed");
                }
            });
            //var identifier = $(this).attr("id")
            //$.post("/sitetemplate/delete?@SharePointContext.CloneQueryString(ViewContext.HttpContext.Request)", identifier)
            //    .done(function(){
            //        $('.ms-Dialog').fadeOut();
            //    })
            //    .fail(function(){
            //        $('.ms-Dialog').fadeOut();
            //    })
        });
    });

