$(document).ready(function () {
    $("#login-submit").click(function () {
        $("#form-res").hide();
        var account = $("#account").val();
        var password = $("#password").val();
        
        $.ajax({
            url: "/Account/Varifyaccount",
            type: "POST",
            data: { account: account, password: password },
            success: function (data) {
                if (data.success) {
                    location.href = "/Home";
                } else {
                    $("span.error").text(data.error);
                    $("#form-res").slideDown();
                }
            }
        });
    });
});