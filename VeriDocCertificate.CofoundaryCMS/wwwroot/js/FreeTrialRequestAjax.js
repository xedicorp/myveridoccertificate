(function () {
    "use strict";

    $(document).ready(function () {
        var exists;

        //function to call inside ajax callback 
        function set_exists(x) {
            exists = x;
        }

        function validateTrialEmail() {

            var valueEmail = $('input[name=FreeTrialEmail]').val();
            var emailPattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            var re = emailPattern.test(valueEmail);
            var wordCount = valueEmail.length;
            if (!re && wordCount >= 1) {
                $('#errorInvalid').show();
                $('#errorExist').hide();
                $('#errorIcon').show();
                if ($('input[name=FreeTrialEmail]').siblings('#errorIcon').children('i').hasClass('fa-circle-check') && !re) {
                    $('#errorIcon').children('i').toggleClass('fa-circle-check fa-circle-xmark');
                    /*$('#errorIcon').css('background-color', 'red');*/
                }
                $('input[name=FreeTrialEmail]').css('border', '1px solid #e23c39');

                // Disable the button
                document.getElementById("freetrialsubmit").disabled = true;
                return false;
            }
            else if (wordCount < 1) {
                $('#errorInvalid').hide();
                $('#errorExist').hide();
                $('#errorIcon').hide();
                $('input[name=FreeTrialEmail]').css('border', '1px solid #CCCCCC').val('');
            }
            else {
                // Disable the button
                document.getElementById("freetrialsubmit").disabled = false;
                const fd = new FormData();
                fd.append("email", $('input[name="FreeTrialEmail"]').val());
                console.log(fd)
                $.ajax({
                    url: apiURL + "/Subscribe/free-trial-checkuser",
                    method: 'POST',
                    data: fd,
                    cache: false,
                    processData: false,
                    async: false,
                    contentType: false,
                    success: function (data, textStatus, xhr) {
                        
                        if (xhr.status == 200 && data) {
                            $('#errorInvalid').hide();
                            $('#errorExist').show();
                            $('#errorIcon').show();
                            if ($('input[name=FreeTrialEmail]').siblings('#errorIcon').children('i').hasClass('fa-circle-check') ) {
                                $('#errorIcon').children('i').toggleClass('fa-circle-check fa-circle-xmark');
                                /*$('#errorIcon').css('background-color', 'red');*/
                            }
                            $('input[name=FreeTrialEmail]').css('border', '1px solid red');
                            set_exists(true);
                        }
                        else {
                            $('#errorInvalid').hide();
                            $('#errorExist').hide();
                            $('#errorIcon').show();
                            $('#errorIcon').children('i').removeClass('fa-circle-xmark').addClass('fa-circle-check');
                            /*$('#errorIcon').css('background-color', '#24984E');*/
                            $('input[name=FreeTrialEmail]').css('border', '1px solid #24984E');
                            set_exists(false);
                            
                        }
                    }

                });
            }
        }
        function checkTrialEmail() {

            var valueEmail = $('input[name=FreeTrialEmail]').val();
            //var emailPattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

                const fd = new FormData();
                fd.append("email", $('input[name="FreeTrialEmail"]').val());
                console.log(fd)
                $.ajax({
                    url: apiURL + "/Subscribe/free-trial-checkuser",
                    method: 'POST',
                    data: fd,
                    cache: false,
                    processData: false,
                    async: false,
                    contentType: false,
                    success: function (data, textStatus, xhr) {

                        if (xhr.status == 200 && data) {
                            $('#errorInvalid').hide();
                            $('#errorExist').show();
                            $('#errorIcon').show();
                            if ($('input[name=FreeTrialEmail]').siblings('#errorIcon').children('i').hasClass('fa-circle-check')) {
                                $('#errorIcon').children('i').toggleClass('fa-circle-check fa-circle-xmark');
                                /*$('#errorIcon').css('background-color', 'red');*/
                            }
                            $('input[name=FreeTrialEmail]').css('border', '1px solid #e23c39');
                            set_exists(true);

                            // Disable the button
                            document.getElementById("freetrialsubmit").disabled = true;
                        }
                        else {
                            $('#errorInvalid').hide();
                            $('#errorExist').hide();
                            $('#errorIcon').show();
                            $('#errorIcon').children('i').removeClass('fa-circle-xmark').addClass('fa-circle-check');
                            /*$('#errorIcon').css('background-color', '#24984E');*/
                            $('input[name=FreeTrialEmail]').css('border', '1px solid #24984E');
                            set_exists(false);
                            validateTrialEmail();
                        }


                    }

                });
        }
        $('#freetrialsubmit').on('click', function (event) {
            event.preventDefault();
            validateTrialEmail();
            if (exists == false) {
                $('#freetrialform').submit();
            }
            else {
                return false;
            }
        });
        
        $('input[name=FreeTrialEmail]').on('keyup', function (event) {
            event.preventDefault();
            validateTrialEmail();
        }).keypress(function (event) {
            if ((event.keyCode || event.which) == 13) {
                validateTrialEmail();
                if (exists == false) {
                    $('#freetrialform').submit();
                }
                else {
                    return false;
                }

            }
            else if ((event.keyCode || event.which) == 8) {
                validateTrialEmail();
            }
        });
        $('#errorIcon').on('click', 'i.fa-circle-xmark', function () {
            
            $('#errorInvalid').hide();
            $('#errorExist').hide();
            $('#errorIcon').hide();
            $('input[name=FreeTrialEmail]').css('border', '1px solid #CCCCCC').val('');

        });
    });
    
	
}());