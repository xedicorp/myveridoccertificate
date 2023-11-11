
(function ($) {
    'use strict';

    //Tips
    //MODAL REQUEST DEMO
    //Phone Code
   
    //Reuqesr demo modal
   
   
 // country start
 $('.CountyCode1').each(function (index, element) {
    let PhoneCodedropdown = $(this);

    PhoneCodedropdown.empty();

    PhoneCodedropdown.append('<option selected="true">Choose Country</option>');
    PhoneCodedropdown.prop('selectedIndex', 0);

     const urlPhoneCode =  window.location.protocol+ '/' +'data/codes.json';

    // Populate dropdown with list of provinces
    $.getJSON(urlPhoneCode, function (data) {
        if (PhoneCodedropdown.hasClass("dial")) {
            $.each(data, function (key, entry) {
                PhoneCodedropdown.append($('<option></option>').attr('value', entry.code).attr('data-val', entry.dial_code).html(entry.name + '&nbsp;(' + entry.dial_code + ')'));
            })
        }
        else if (PhoneCodedropdown.hasClass("country")) {
            $.each(data, function (key, entry) {
                PhoneCodedropdown.append($('<option></option>').attr('value', entry.code).attr('data-val', entry.name).attr('data-dialcode', entry.dial_code).html(entry.name));
            })
        }
    });
    PhoneCodedropdown.select2({
        minimumResultsForSearch:1,
        templateResult: formatState,
        formatSelection: formatState,
        dropdownParent: PhoneCodedropdown.parent('div')
    });
     PhoneCodedropdown.on('select2:select', function (e) {
        var data = e.params.data;
         if (data.text == "Choose Country") {
             $(".select2-selection__rendered").attr('style', 'color: #d6d6d6 !important')
         }
         else{
             $(".select2-selection__rendered").attr('style', 'color: #000 !important')
         }
    });

 });

    function formatState (state) {
       
    
        if (!state.id) { return state.text; }
      
        var $state = $(
          `
          <div style="display: flex; align-items: center;">
             <div><img sytle="display: inline-block;" src="https://flagcdn.com/${state.id.toLowerCase()}.svg" style="height: 20px;width: 25px;" /></div>
             <div style="margin-left: 10px;">
                ${state.text}
             </div>
          </div>
          `
        );
        return $state;
    }
    //country end

    
    //Date Time picker
    var $datePicker = $("div#datepicker");

    $datePicker.datepicker({
        inline: true,
        altField: "#datep",
        minDate: 0,
        dateFormat: 'dd/mm/yy'
        
    }).on('change', function (event) {
        event.preventDefault();
        var dateText = this.value;
        setTimeout(function () {
            $datePicker.find('.ui-datepicker').append(`<div class="timeslot">
            <div class="row">
            <div class="col-4">
            <div><button type="button" class="selectTime">8:00AM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">9:00AM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">10:00AM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">11:00AM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">12:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">13:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">14:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">15:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">16:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">17:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">18:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">19:00PM</button></div>
            </div>
            <div class="col-4">
            <div><button type="button" class="selectTime">20:00PM</button></div>
            </div>
            </div>
            </div>`);
            /* Act on the event */


            var d = new Date();

            var month = d.getMonth() + 1;
            var day = d.getDate();

            var output = (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + d.getFullYear();

      


            if (dateText == output) {
           
                $(".timeslot").find('button').each(function () {
                    var dt = new Date();

                    var result = parseInt(dt.getHours());

                  
                    var $prevTime = $(this);

                    var allTimeSlot = parseInt($(this).text());



                    if (result >= allTimeSlot) {
                       
                        $prevTime.attr('disabled', '')
                    }


                });
            }
        });

      
    });
    
    //SlideToggle Datepicker
    $('#datep').click(function () {
        $('#datepicker').slideToggle();
        

    });

    //Set TIme Slot
    $(document).on('click', 'button.selectTime', function () {
        
        $("button.selectTime").not(this).removeClass('active');
        $(this).addClass('active');
        var TimeSlot = $(this).text();
        $('#TimeSlotHidden').val(TimeSlot);
        $('#datepicker').slideUp();
    });
  

    $(document).mouseup(function (e) {
        var container = $("#datepicker");
        var container1 = $("#datep");

        // if the target of the click isn't the container nor a descendant of the container
        if (!container.is(e.target) && container.has(e.target).length === 0 && !container1.is(e.target) && container1.has(e.target).length === 0) {
            container.hide();
        }
    });


    
   
}(jQuery));
 
   
