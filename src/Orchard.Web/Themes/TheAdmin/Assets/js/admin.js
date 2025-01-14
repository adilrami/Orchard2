﻿$(function () {
    $("body").on("click", "[itemprop~='RemoveUrl']", function () {
        // don't show the confirm dialog if the link is also UnsafeUrl, as it will already be handled in base.js
        if ($(this).filter("[itemprop~='UnsafeUrl']").length == 1) {
            return false;
        }

        // use a custom message if its set in data-message
        var dataMessage = $(this).data('message');
        if (dataMessage === undefined) {
            dataMessage = confirmRemoveMessage;
        }

        return confirm(dataMessage);
    });
})(jQuery);

$(function () {
    var magicToken = $("input[name=__RequestVerificationToken]").first();
    if (magicToken) {
        $("body").on("click", "a[itemprop~=UnsafeUrl], a[data-unsafe-url]", function () {
            var _this = $(this);
            var hrefParts = _this.attr("href").split("?");
            var form = $("<form action=\"" + hrefParts[0] + "\" method=\"POST\" />");
            form.append(magicToken.clone());
            if (hrefParts.length > 1) {
                var queryParts = hrefParts[1].split("&");
                for (var i = 0; i < queryParts.length; i++) {
                    var queryPartKVP = queryParts[i].split("=");
                    //trusting hrefs in the page here
                    form.append($("<input type=\"hidden\" name=\"" + decodeURIComponent(queryPartKVP[0]) + "\" value=\"" + decodeURIComponent(queryPartKVP[1]) + "\" />"));
                }
            }
            form.css({ "position": "absolute", "left": "-9999em" });
            $("body").append(form);

            var unsafeUrlPrompt = _this.data("unsafe-url");

            if (unsafeUrlPrompt && unsafeUrlPrompt.length > 0) {
                if (!confirm(unsafeUrlPrompt)) {
                    return false;
                }
            }

            if (_this.filter("[itemprop~='RemoveUrl']").length == 1) {
                // use a custom message if its set in data-message
                var dataMessage = _this.data('message');
                if (dataMessage === undefined) {
                    dataMessage = confirmRemoveMessage;
                }

                if (!confirm(dataMessage)) {
                    return false;
                }
            }

            form.submit();
            return false;

        });
    }
})(jQuery);