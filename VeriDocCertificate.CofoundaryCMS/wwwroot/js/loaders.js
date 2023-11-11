var dots = 0;
$(document).ready(function () {
    setInterval(type, 600);
});
function type() {
    if (dots < 3) {
        $('#dots').append('.');
        dots++;
    }
    else {
        $('#dots').html('');
        dots = 0;
    }
}

function countUp(duration) {
    var $this = $('.countPercentage > span'), // <- Don't touch this variable. It's pure magic.
        countTo = $this.attr('data-count');
    ended = $this.attr('ended');

    if (ended != "true") {
        $({ countNum: $this.text() }).animate({
            countNum: countTo
        },
            {
                duration: duration, //duration of counting
                easing: 'swing',
                step: function () {
                    $this.text(Math.floor(this.countNum));
                },
                complete: function () {
                    $this.text(this.countNum);
                }
            });
        $this.attr('ended', 'true');
    }
}

function showLoader(msg, duration, loaduntill) {
    $('.countPercentage > span').text('0');
    $('.custom-text').text(msg);
    $('.spanner').show();

    $('.countPercentage > span').attr('data-count', loaduntill).attr('ended', false);

    countUp(duration)
    $('body').addClass('showLoader');
}

function hideLoader(duration) {
    $('.countPercentage > span').attr('data-count', 100).attr('ended', false);
    countUp(duration)
    setTimeout(function () {
        $('.spanner').hide();
        $('body').removeClass('showLoader');
    }, duration + 500);

}
