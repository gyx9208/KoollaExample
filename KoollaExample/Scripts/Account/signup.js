$(document).ready(function () {
    $("#signup-submit").click(function () {
        $("#form-res").hide();
        var account = $("#account").val();
        var password = $("#password").val();
        var password2 = $("#password2").val();
        if (password != password2) {
            $("span.error").text("两次密码不相同");
            $("#form-res").slideDown();
        }
        else {
            $.ajax({
                url: "/Account/Signaccount",
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
        }
    });
});