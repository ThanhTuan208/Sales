var timerInterval;
var expireSeconds = parseInt($(".qr-expire-time").data("expire-seconds")) || 300;

// Dem nguoc thoi gian het han QR
var remaining = expireSeconds;

export function startCountdown(seconds) {
    remaining = seconds;
    $("#resetQR").hide();

    clearInterval(timerInterval);
    timerInterval = setInterval(function () {
        remaining--;

        var minutes = Math.floor(remaining / 60);
        var secs = remaining % 60;
        $("#timer").text(
            (minutes < 10 ? "0" : "") + minutes + ":" +
            (secs < 10 ? "0" : "") + secs
        );

        if (remaining <= 0) {
            clearInterval(timerInterval);
            $("#resetQR").show();
        }
    }, 1000);
}