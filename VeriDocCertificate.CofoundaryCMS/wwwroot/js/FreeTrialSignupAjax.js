(function () {
    "use strict";

    $(document).ready(function () {
        const url = new URL(window.location.href);
        let email = url.searchParams.get("email");
        document.getElementById('email_id').value = email;
        document.getElementById('usermailsec').innerHTML = email;

        $('#startFreeTrial').click(function (e) {
            e.preventDefault();
            const ref = this;
            this.innerHTML = this.innerHTML + `<span><svg version="1.1" id="L9" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                        viewBox="0 0 100 100" enable-background="new 0 0 0 0" xml:space="preserve">
                                        <path fill="#fff" stroke="#fff" stroke-width="4" d="M73,50c0-12.7-10.3-23-23-23S27,37.3,27,50 M30.9,50c0-10.5,8.5-19.1,19.1-19.1S69.1,39.5,69.1,50">
                                            <animateTransform 
                                                attributeName="transform" 
                                                attributeType="XML" 
                                                type="rotate"
                                                dur="1s" 
                                                from="0 50 50"
                                                to="360 50 50" 
                                                repeatCount="indefinite" />
                                        </path>
                                        </svg></span>`;
            this.setAttribute("disabled", "");
            const fd = new FormData();
            fd.append("email", $('#email_id').val());
            fd.append("password", $('#pass_log_id').val());
            $.ajax({
                async: true,
                method: "POST",
                url: apiURL + "/Subscribe/free-trial-signup",
                data: fd,
                cache: false,
                processData: false,
                contentType: false,
                success: function (data, status, xhr) {
                    ref.innerHTML = "Start Your Free Trial";
                    ref.removeAttribute("disabled");
                    if (xhr.status == 200) {
                        document.getElementById('PassSetup').style.display = "none";
                        document.getElementById('emailsentview').style.display = "block";
                    }
                },
                error: function (xhr, status, error) {
                    ref.innerHTML = "Start Your Free Trial";
                    ref.removeAttribute("disabled");
                    if (xhr.status == 400 && xhr.responseJSON.hasOwnProperty("errors")) {
                        let msg = "";
                        for (const index in xhr.responseJSON.errors) {
                            msg += xhr.responseJSON.errors[index].join() + ", ";
                        }
                        showError2(`<p>${msg.replace(/.$/, '')}</p>`);
                    } else if (xhr.status == 400 || xhr.responseJSON.statusCode == 400) {
                        showError2(`${xhr.responseJSON.content}`);
                    } else {
                        popupservererror();
                    }
                    console.log(xhr);
                }
            });
        });
        $("body").on('click', '.toggle-password', function () {
            $(this).toggleClass("fa-eye fa-eye-slash ");
            var input = $(this).siblings('input');
            if (input.attr("type") === "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }

        });

        $('.password-setup input').on("cut copy paste contextmenu", function (e) {
            e.preventDefault();
            return false;
        });

        $('#pass_log_id').keyup(function () {
            $(this).css('border', '1px solid #e23c39');
            $(this).siblings('span').css('display', 'block');
            let CheckVal = $(this).val();
            validateCheckPassword(CheckVal);
            var count_elements = $(this).siblings('.red').length;
            if (count_elements == 0) {
                $(this).css('border', '1px solid #24984e');

                if ($(this).siblings('.fa').length == 0) {
                    $(this).after('<i class="fa fa-check"></i>');
                }

                $("#startFreeTrial").attr('disabled', false);

            }
            else if (count_elements >= 1) {
                $(this).siblings('.fa-check').remove();
                $("#startFreeTrial").attr('disabled', true);
            }

            if (CheckVal.length == 0) {
                $(this).siblings('span').css('display', 'none');
            }


        });

        function validateCheckPassword(passval) {
            var upperCase = new RegExp('[A-Z]');
            var lowerCase = new RegExp('[a-z]');
            var numbers = new RegExp('[0-9]');
            if (passval.length > 7) {
                $('.charlength_validate').addClass('green').removeClass('red');

            }
            else {
                $('.charlength_validate').addClass('red').removeClass('green');

            }

            if (passval.match(upperCase)) {
                $('.capital_validate').addClass('green').removeClass('red');

            }
            else {
                $('.capital_validate').addClass('red').removeClass('green');

            }
            if (passval.match(lowerCase)) {
                $('.smallcase_validate').addClass('green').removeClass('red');

            }
            else {
                $('.smallcase_validate').addClass('red').removeClass('green');

            }
            if (passval.match(numbers)) {
                $('.numeric_validate').addClass('green').removeClass('red');

            }
            else {
                $('.numeric_validate').addClass('red').removeClass('green');

            }
            if (/^[a-zA-Z0-9- ]*$/.test(passval) == false) {
                $('.special_validate').addClass('green').removeClass('red');

            }
            else {
                $('.special_validate').addClass('red').removeClass('green');

            }
        }
    });
    
	
}());
