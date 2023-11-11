function validateEmail(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return reg.test(email);
}
function validatePassword(pass) {
    console.log(pass);
    const reg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/;
    return reg.test(pass);
}
function getUrlVars() {

    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    var timespan = window.location.href.slice(window.location.href.indexOf('-') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    for (var i = 0; i < timespan.length; i++) {
        timespan = timespan[i].split('=');
        vars.push(timespan[0]);
        vars[hash[2]] = hash[3];
    }
    return vars;
}

$(document).on('click', '.menu-toggle', function () {
    $('nav').toggleClass('active');
    $('body').css('overflow-y', 'hidden')
});
$(document).on('click', '#closeBtn', function () {

    $('.menu-toggle').trigger("click");
    $('body').css('overflow-y', '')
});
$(window).scroll(function () {
    if ($(this).scrollTop()) {
        $('#back-to-top1').fadeIn();
    } else {
        $('#back-to-top1').fadeOut();
    }
});

$("#back-to-top1").click(function () {
    $("html, body").animate({ scrollTop: 0 }, 1000);
});

$(window).scroll(function () {
    var HeaderHeight = $('header').height();
    if($(window).width() > 992){
        if ($(window).scrollTop() >= 400) {
            $('header').addClass('fixed-header');
    
        }
        else {
            $('header').removeClass('fixed-header');
    
        }
    }
    else{
        if ($(window).scrollTop() >= 200) {
            $('header').addClass('fixed-header');
    
        }
        else {
            $('header').removeClass('fixed-header');
    
        }
    }
    
});

$(document).ready(function () {
    $elems = $('.testimonial .item .title');
    var max_height = 0;
    $elems.each(function (idx, elem) {
        max_height = Math.max(max_height, $(elem).height());
    });
    $elems.closest('.testimonial').height(max_height + 100);

    $('.btn_try_now').click(function (e) {
        e.preventDefault()
        $('input[name="FreeTrialEmail"]').focus();
        var $container = $("html,body");
        var $scrollTo = $('.free_trail');

        $container.animate({ scrollTop: $container.offset().top - 350 }, 300);
    })
    $('input[name="SubFistName"]').keypress(function () {
        $('#firstNameErrorMsg').html('');
    });
    $('input[name="SubEmail"]').keypress(function () {
        $('#emailErrorMsg').html('');
    });
    var HeaderHeight = $('header').height();
    $('a[href*=\\#]').on('click', function () {
        //if ($(window).width() < 991) {
        //    $('.navbar-toggler').click();
        //}
       
        $('html, body').animate({
            scrollTop: $(this.hash).offset().top - HeaderHeight
        }, 500);
    });
    $(window).on('load', function () {
        if (window.location.hash) {

            var hash = window.location.hash;

            $('html, body').animate({
                scrollTop: $(hash).offset().top - HeaderHeight
            }, 500);

        };
    });


    var object = { planName: getUrlVars()["plan"], TimeSpan: getUrlVars()["timespan"] };
    $("#planName").text(object.planName);

    if (object.planName == "TRIAL") {
       $(".packages").not('.trial').remove()
    }
    else if (object.planName == "STANDARD" && object.TimeSpan == "MONTHLY") {
       $(".packages").not('.standard_monthly').remove();

    }
    else if (object.planName == "STANDARD" && object.TimeSpan == "YEARLY") {

       $(".packages").not('.standard_yearly').remove();

    }
    else if (object.planName == "PRO" && object.TimeSpan == "MONTHLY") {
       $(".packages").not('.pro_monthly').remove();

    }
    else if (object.planName == "PRO" && object.TimeSpan == "YEARLY") {
       $(".packages").not('.pro_yearly').remove();

    }
    //Start plan script
    $('input:radio[name="address_check"]').change(
        function () {
           if ($(this).is(':checked') && $(this).val() == 'No') {
              $('.billing_address').slideDown();
           }
           else {
              $('.billing_address').slideUp();
           }
        }
    );

     if ($(window).width() > 991) {
        $('a[data-collapse]').click(function () {
           let collapseId = '#' + $(this).data('collapse');
           $(collapseId).slideToggle();
           $(this).find('i').toggleClass('fa-chevron-up fa-chevron-down')
        });
     }

     if ($(window).width() < 992) {

        $('#gotoInitial').click(function () {
           $('.plan-selected_1').hide(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
           $('.user-info').show(500);
        });
        $('#BacktoPlan').click(function () {
           $('.user-info').hide(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
           $('.plan-selected_1').show(500);
        });
        $('#gotoSecond').click(function () {
           $('.user-info').hide(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
           $('.company-info').show(500);
        });
        $('#BacktoUser').click(function () {
           $('.company-info').hide(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
           $('.user-info').show(500);
        });
        $('#gotoFinal').click(function () {
           $('.company-details-cont').hide(500);
           $('.billing-address-container').show(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
        });
        $('#BacktoCompany').click(function () {
           $('.billing-address-container').hide(500);
           $('.company-details-cont').show(500);
           $("html, body").animate({ scrollTop: 0 }, "fast");
        });
    }

    $('#btn_submit_demo').click(function () {             
        
        //e.preventDefault();
        let list = [
            document.querySelector("input[name='firstname']"),
            document.querySelector("input[name='email']"),
            document.querySelector("select[name='CountyCode']"),
            document.querySelector("input[name='dateandTime']"),
            document.querySelector("input#TimeSlotHidden")
        ];

        if (validateInput(list)) {
            return;
        }

        this.setAttribute('disabled', '');
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
        this.style.cursor = 'not-allowed';
        let ref = this;
        var formData = {
            subject: "VeriDoc Certificates Demo Request",
            firstName: $("input[name='firstname']").val(),
            lastName: $("input[name='lastname']").val(),
            email: $("input[name='email']").val(),
            phoneCode: $("select[name='CountyCode1']").find('option:selected').data("val"),
            phoneNumber: $("input[name='PhoneNo']").val(),
            Companyname: $("input[name='Companyname']").val(),
            country: $("select[name = 'CountyCode']").find('option:selected').data("val"),
            date: $("input#datep").val(),
            time: $("input#TimeSlotHidden").val()
        }

        console.log(formData)

        $.ajax({
            url: apiURL + "/Mail/send-demo-request",
            method: 'POST',
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function (data, textStatus, xhr) {
                ref.style.cursor = 'pointer';
                ref.innerHTML = 'Submit';
                ref.removeAttribute('disabled');
                if (xhr.status == 200) {
                    PopupRequestDemo();
                    ResetPopup();
                    $('#RequestDemoModal').modal('hide');
                   
                } else if (xhr.status == 400 && Array.isArray(xhr.responseJSON)) {
                    let msg = '';
                    xhr.responseJSON.forEach(function (v) { msg += v + ", " });
                    showError(msg);
                }
                var frm = document.getElementById('frm')[0];
                frm.reset();
               
            },
            error: function (xhr, textStatus, errorThrown) {
                ref.style.cursor = 'pointer';
                ref.innerHTML = 'Submit';
                ref.removeAttribute('disabled');
                console.log(xhr);
                if (xhr.status == 500) {
                    popupservererror();
                } else if (xhr.status == 400 && Array.isArray(xhr.responseJSON)) {
                    let msg = '';
                    xhr.responseJSON.forEach(function (v) { msg += v + ", " });
                    showError(msg);
                }
            }
            // error handling
        });
    });
    $('#btnContact').click(function (e) {

        e.preventDefault();
        let list = [
            document.querySelector("input[name='first_name']"),
            document.querySelector("input[name='email_name']"),
            document.querySelector("textarea[name='enter_message']")
        ];
        if (validateInput(list)) {
            return;
        }

        this.setAttribute('disabled', '');
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
        /*this.style.cursor = 'pointer';*/
        let ref = this;
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdWFxoeAAAAAIV5QiMZ1CoLILIMhJ01xhUl5KX1', { action: 'btnContact' }).then(function (recaptchaToken) {
                $('#hdnRecaptcha').val(recaptchaToken);
                var ContactformData = {
                    subject: "VeriDoc Certificates Contact Request",
                    firstName: $("input[name='first_name']").val(),
                    lastName: $("input[name='last_name']").val(),
                    email: $("input[name='email_name']").val(),
                    phoneCode: $("select[name='ContactCountyCode1']").find('option:selected').data("val"),
                    phoneNumber: $("input[name='phone_number']").val(),
                    message: $("textarea[name='enter_message']").val(),
                    reCaptchaToken: $("#hdnRecaptcha").val()
                }
                console.log(ContactformData)
                $.ajax({
                    url: apiURL + "/Mail/send-contact-mail",
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(ContactformData),
                    success: function (data, textStatus, xhr) {
                        ref.innerHTML = 'Submit';
                        ref.removeAttribute('disabled');
                        if (xhr.status == 200) {
                            debugger;
                            PopupRequestDemo();
                            ResetPopup();
                            $('#btnContact').removeAttr('disabled');
                            $('#btnContact').find('span').remove();
                        } else if (xhr.status == 400 && Array.isArray(xhr.responseJSON)) {
                            let msg = '';
                            xhr.responseJSON.forEach(function (v) { msg += v + ", " });
                            showError2(msg);
                        } else if (xhr.status == 417 && xhr.responseJSON.hasOwnProperty('content')) {
                            showError2(xhr.responseJSON.content);
                        }
                        document.getElementsByName('contact-form')[0].reset();
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        debugger;
                        ref.innerHTML = 'Submit';
                        ref.removeAttribute('disabled');
                        console.log(xhr);
                        if (xhr.status == 500) {
                            popupservererror();
                            $('#btnContact').removeAttr('disabled');
                        } else if (xhr.status == 400 && Array.isArray(xhr.responseJSON)) {
                            let msg = '';
                            xhr.responseJSON.forEach(function (v) { msg += v + ", " });
                            showError2(msg);
                            $('#btnContact').removeAttr('disabled');
                        }
                        else if (xhr.status == 417) {
                            showError2(xhr.responseText);
                            $('#btnContact').removeAttr('disabled');
                        }
                    }
                    // error handling
                });


                // error handling

            });

        });

    });

    $('.submit-signup-validation').click(function (e) {

        e.preventDefault();
        let list = [
            document.querySelector("input[name='SubFistName']"),
            document.querySelector("input[name='SubEmail']"),
        ];

        if (validateInput(list)) {
            return;
        }
        $('.information-card').slideUp();
        $('.extra-template').hide();
        $('.sub-heading').text('SUMMARY');

        if ($(".extra-template input").is(":checked")) {
            $('.summary-card .with-template').slideDown();
        } else {
            $('.summary-card .without-template').slideDown();
        }
        $('.summary-card').show();
    });
    
    $(".signactionmain-wtowot").click(function (e) {
        e.preventDefault();
        let list = [
            document.querySelector("input[name='SubFistName']"),
            document.querySelector("input[name='SubEmail']"),
        ];

        if (validateInput(list)) {
            return;
        }
        $(this).attr('disabled', '');
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
        this.style.cursor = "not-allowed";
        const ref = this;

        let SubformData = getSignupdata();
        SubformData.IsTemplate = document.querySelector('input[name="templateselection"]').checked;
        console.log(SubformData);
        $.ajax({
            url: "/Account/SubscribeUser",
            method: 'POST',
            data: JSON.stringify(SubformData),
            contentType: 'application/json',
            success: function (data, textStatus, xhr) {
                ref.innerHTML = "Continue";
                ref.removeAttribute("disabled");
                ref.style.cursor = "pointer";
                if (data.code == 400 && data.msg != '') {
                    showError2(data.msg);
                } else if (data.code == 500) {
                    popupservererror();
                } else if (data.code == 200) {
                    window.location.href = "/Payment";
                }
                console.log(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                ref.innerHTML = "Continue";
                ref.removeAttribute("disabled");
                ref.style.cursor = "pointer";
                console.log(xhr);
                if (xhr.status == 500) {
                    popupservererror();
                }
            }
        });
    });

    $('#blog-slider').owlCarousel({
        autoplay: true,
        loop:true,
        autoPlaySpeed: 5000,
        autoPlayTimeout: 5000,
        autoWidth: false,
        items: 1,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3],
        nav: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        dots: true,
    });
    var $btns = $('.btn-tab').click(function () {
        if (this.id == 'all') {
            $('#parent > div').fadeIn(450);
        } else {
            var $el = $('.' + this.id).fadeIn(450);
            $('#parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    });
 }); 

function getSignupdata() {
    var object = { planName: getUrlVars()["plan"], TimeSpan: getUrlVars()["timespan"] };
    let country = "";
    let billingcCountry = "";
    let countryCode = "";
    let billingCountryCode = "";
    if ($("select[name='SubCompanyCountry']").val() != 'Choose Country') {
        country = $("select[name='SubCompanyCountry']").val();
        countryCode = $("select[name='SubCompanyCountry']").find('option:selected').data("dialcode");
    }
    if ($("select[name='BillingCompanyCountry']").val() != "Choose Country") {
        billingcCountry = $("select[name='BillingCompanyCountry']").val();
        billingCountryCode = $("select[name='BillingCompanyCountry']").find('option:selected').data("dialcode");
    }
    var SubformData = {
        "FirstName": $("input[name='SubFistName']").val(),
        "LastName": $("input[name='SubLastName']").val(),
        "Email": $("input[name='SubEmail']").val(),
        "PhoneCode": $("select[name='SubPhoneCodeNo']").find('option:selected').data("val") == undefined ? "" : $("select[name='SubPhoneCodeNo']").find('option:selected').data("val"),
        "PhoneNumber": $("input[name='SubPhoneNo']").val(),
        "CompanyName": $("input[name='SubCompanyName']").val(),
        "Address": $("input[name='SubCompanyAddress']").val(),
        "City": $("input[name='SubCompanyCity']").val(),
        "State": $("input[name='SubCompanyState']").val(),
        "Country": country,
        "CountryCode": countryCode,
        "Zip": $("input[name='SubCompanyPostcode']").val(),
        "IsBiilingSame": document.getElementById('address_checkyes').checked,
        "BillingCompanyName": document.getElementById('address_checkyes').checked ? $("input[name='SubCompanyName']").val() : $("input[name='BillingCompanyName']").val(),
        "BillingAddress": document.getElementById('address_checkyes').checked ? $("input[name='SubCompanyAddress']").val() : $("input[name='BillingCompanyAddress']").val(),
        "BillingCity": document.getElementById('address_checkyes').checked ? $("input[name='SubCompanyCity']").val() : $("input[name='BillingCompanyCity']").val(),
        "BillingState": document.getElementById('address_checkyes').checked ? $("input[name='SubCompanyState']").val() : $("input[name='BillingCompanyState']").val(),
        "BillingCountry": document.getElementById('address_checkyes').checked ? country : billingcCountry,
        "BillingCountryCode": document.getElementById('address_checkyes').checked ? countryCode : billingCountryCode,
        "BillingZip": document.getElementById('address_checkyes').checked ? $("input[name='SubCompanyPostcode']").val() : $("input[name='BillingZipCode']").val(),
        "Plan": object.planName,
        "PlanTimeSpan": object.TimeSpan
    }
    return SubformData;
}


function validateInput(elementList) {
    let v = false;
    let message = '<p>Please enter all required fields !</p>';
    elementList.forEach(element => {
        if (element.getAttribute('type') == 'email') {
            if (element.value == '') {
                message = '<p>Please enter all required fields !</p>';
                v = true;
                element.style.border = '1px solid red';
            }
            else if (!validateEmail(element.value)) {
                message = '<p>Please enter valid email !</p>';
                element.style.border = '1px solid red';
                v = true;
            }
            else {
                element.style.border = '1px solid #cccccc';
            }
        }
        else if (element.nodeName == "SELECT") {
            if (element.value == 'Choose Country') {
                //message = '<p>Please enter all required fields.</p>';
                element.nextElementSibling.querySelector('span.select2-selection.select2-selection--single').style.border = '1px solid red';
                v = true;
            }
            else {
                element.nextElementSibling.querySelector('span.select2-selection.select2-selection--single').style.border = '1px solid #cccccc';
            }
        }
        else if (element.getAttribute('type') == 'number') {
            if (element.value == '') {
                //message = '<p>Please enter all required fields.</p>';
                element.style.border = '1px solid red';
                v = true;
            }
            else if (element.value.length < 9 || element.value.length > 10) {
                message = '<p>Please enter valid phone number !</p>';
                element.style.border = '1px solid red';
                v = true;
            }
            else {
                element.style.border = '1px solid #cccccc';
            }
        }
        else if (element.getAttribute('type') == 'password') {
            if (!validatePassword(element.value)) {
                //message = '<p>Please enter all required fields.</p>';
                v = true;
                element.style.border = '1px solid red';
            }
            else {
                element.style.border = '1px solid #cccccc';
            }
        }
        else {
            if (element.value == '00:00AM' || element.value == '') {
                //message = '<p>Please enter all required fields.</p>';
                if (element.value == '00:00AM') {
                    element.parentElement.querySelector('#datep').style.border = '1px solid red';
                }
                else {
                    element.style.border = '1px solid red';
                }
                v = true;
            }
            else {
                element.style.border = '1px solid #cccccc';
            }
        }
    });
    if (v) {
        showError2(message);
    }
    return v;
}
function ResetPopup() {
    $("input[name='first_name']").val('');
    $("input[name='last_name']").val('');
    $("input[name='email_name']").val('');
    $("select[name='ContactCountyCode1']").val(null).trigger('change');
    $("select[name='ContactCountyCode1']").val($("select[name='ContactCountyCode1'] option:first").val()).trigger('change');
    $("input[name='phone_number']").val('');
    $("textarea[name='enter_message']").val('');

    $("input[name='firstname']").val('');
    $("input[name='lastname']").val('');
    $("input[name='email']").val('');
    $("select[name='CountyCode1']").prop('selectedIndex', 0);
    $("input[name='PhoneNo']").val('');
    $("input[name='Companyname']").val('');
    $("select[name = 'CountyCode']").prop('selectedIndex', 0);
    $("input#datep").val('');
    $("input#TimeSlotHidden").val('');
    $("#CountyCode").val('');

    $('#datepicker').datepicker('setDate', null);
    $('.timeslot').find('button').removeClass('active');

    $("select[name='CountyCode']").val(null).trigger('change');
    $("select[name='CountyCode']").val($("select[name='CountyCode'] option:first").val()).trigger('change');

    $("select[name='CountyCode1']").val(null).trigger('change');
    $("select[name='CountyCode1']").val($("select[name='CountyCode1'] option:first").val()).trigger('change');

}
function validateDemo() {
    let v = false;
    if ($("input[name='firstname']").val() == "") {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("input[name='firstname']"));
        v = true;
    }
    else {
        document.querySelector("input[name='firstname']").style.border = '1px solid #cccccc';
        v = false;
    }

    if ($("input[name='email']").val() == "") {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("input[name='email']"));
        return false
    }
    else {
        document.querySelector("input[name='email']").style.border = '1px solid #cccccc';
    }

    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (reg.test($("input[name='email']").val()) == false) {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("input[name='email']"));
        return false;
    }
    else {
        document.querySelector("input[name='email']").style.border = '1px solid #cccccc';
    }
    if ($("select[name='CountyCode']").val() == 'Choose Country') {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("select[name='CountyCode']"));
        return false
    }
    else {
        document.querySelector("select[name='CountyCode']").style.border = '1px solid #cccccc';
    }

    if ($("input[name='dateandTime']").val() == "") {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("input[name='dateandTime']"));
        return false
    } else {
        document.querySelector("input[name='dateandTime']").style.border = '1px solid #cccccc';
    }
    if ($("input#TimeSlotHidden").val() == "00:00AM") {
        showError2("<p>Please enter all required fields !</p>", document.querySelector("input#TimeSlotHidden"));
        return false
    } else {
        document.querySelector("input#TimeSlotHidden").style.border = '1px solid #cccccc';
    }
}
function showError2(msg, element) {
    Swal.fire({
        icon: 'error',
        title: 'Error!',
        html: msg,
        customClass: {
            closeButton: 'btn_2',
            confirmButton: 'btn_2',
            cancelButton: 'btn_2',
        }
    }).then(function () {
        if (element != undefined || element != null) {
            //element.style.border = '1px solid red';
        }
    });
}
function PopupRequestDemo() {
    Swal.fire({
        icon: 'success',
        title: 'Successful!',
        text: 'Thank you for your interest. Somebody from your region will be in contact with you soon!',
        button: "Ok",
        customClass: {
            confirmButton: 'btn_2',
        },
    });
}
function popupservererror() {
    showError2('<p>Server error ! Try again later !</p>');
}

