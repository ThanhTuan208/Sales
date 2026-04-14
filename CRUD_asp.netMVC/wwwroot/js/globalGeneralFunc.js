var timerInterval;
var expireSeconds = parseInt($(".qr-expire-time").data("expire-seconds"));

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

export function initBuyNowQrModal() {

    const dataElement = document.getElementById('buyNowData');

    if (!dataElement) return;

    const buyNowData = {
        productId: parseInt(dataElement.dataset.productId),
        color: dataElement.dataset.color || '',
        size: dataElement.dataset.size || '',
        qty: parseInt(dataElement.dataset.qty) || 1,
    };

    $.ajax({
        url: '/Cart/ShowQrModalCart',
        type: 'GET',
        data: {
            productId: buyNowData.productId,
            color: buyNowData.color,
            size: buyNowData.size,
            qty: buyNowData.qty
        },
        success: function (response) {
            if (response && (response.success || response.html)) {
                $(".modal").html(response.html || response);

                $(".modal-overlay").fadeIn(300);
                $(".modal").addClass("active").fadeIn(300);

                if (typeof startCountdown === "function") startCountdown(300);
                if (typeof updateQtyAfterCheck === "function") updateQtyAfterCheck();
            }
        },
        error: function () {
            console.error("Lỗi hiển thị modal QR cho Buy Now");
        }
    });
}

// cap nhat so luong va gia san pham sau khi check va tang giam so luong cho gia sp
export function updateQtyAfterCheck() {

    let total = 0;
    let qty = 0;
    let price = 0;
    let ship = 0;
    let totalVAT = 0;
    let countItem = 0;

    $('.checkbox.active').each(function () {
        qty = parseInt($(this).closest('tr').find('.quantity-input').val());
        price = parseFloat($(this).closest('tr').find('.price').text().replace(/[^0-9]+/g, ''));
        total += price * qty;
        countItem++;
    });

    if ($('.checkbox').hasClass('active')) {

        ship = 30000;
    }

    let vat = 0;
    if (total > 1000000) {
        ship = 0;
    }
    totalVAT += total + ship + vat;

    $('.countItem').text(`x ` + countItem);
    $('.price-provisional').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
    $('.price-ship').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(ship));
    $('.price-totalVAT').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total) + `* 0.5%`);
    $('.price-complete').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalVAT));
}


// lay mang id san pham da chon trong gio hang
export function GetArrIDChecked(ids) {

    const productChecked = $('.checkbox:checked');
    const paymentMethodChecked = $('input[name="payment"]:checked').data('method');

    if (paymentMethodChecked === "qr" && productChecked.length > 0) {

        $('.checkbox:checked').each(function () {

            ids.push($(this).val());
        });

    }
    else return { success: false, paymentMethod: null };

    return { success: true, paymentMethod: "transfer" };
}

export function LoadView() {
    // Su dung pageshow de load lai trang (ke ca khi go back va return view)
    $(window).on('pageshow', function () {

        $('.checkbox:checked').each(function () {
            $(this).addClass('active');
        });

        updateQtyAfterCheck();
    });
}
