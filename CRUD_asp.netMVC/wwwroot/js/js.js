$(document).ready(function () {
    $('.owl-clients').owlCarousel({
        loop: true, // Vòng lặp carousel
        margin: 10, // Khoảng cách giữa các phần tử
        /*nav: true, // Hiển thị nút điều hướng*/
        dots: true, // Hiển thị chấm điều hướng
        autoplay: true, // Tự động chạy
        autoplayTimeout: 3000, // Thời gian giữa các slide (3 giây)
        responsive: {
            0: {
                items: 1 // Màn hình nhỏ (0px - 576px): 1 phần tử
            },
            576: {
                items: 2 // Màn hình trung bình (576px - 767px): 2 phần tử
            },
            768: {
                items: 3 // Màn hình tablet (768px - 991px): 3 phần tử
            },
            992: {
                items: 4 // Màn hình lớn (992px trở lên): 4 phần tử
            },
            1200: {
                items: 5 // Màn hình rất lớn (1200px trở lên): 5 phần tử
            }
        }
    });
});

$(document).ready(function () {

    function updateQtyAfterCheck() {

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

            ship = 50000;
        }
        let vat = total * 0.005;
        totalVAT += total + ship + vat;

        $('.countItem').text(`x ` + countItem);
        $('.price-provisional').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
        $('.price-ship').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(ship));
        $('.price-totalVAT').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total) + ' * 0.5%');
        $('.price-complete').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalVAT));
    }

    // Tang giam so luong trong gio hang (tranh bi luu lich su trang web khong nhu su dung method post)
    $('.quantity-btn').on('click', function () {
        const itemID = $(this).data('id');
        let operation = $(this).data('opera');
        let quantityInput = $(this).siblings('.quantity-input');
        let CurrentQty = parseInt(quantityInput.val());
        let NewQty = operation === '+' ? CurrentQty + 1 : CurrentQty - 1;

        if (NewQty < 1) {
            NewQty = 1;
        }

        /*gan bien cho $(this) ben ngoai ajax vi ko con la pham tu DOM($('.quantity-btn') <=> $(this)) ma chuyen sang thanh ngu canh khac*/
        /*day la this cu truoc khi truyen vao ajax, neu su dung trong ajax thi no la 1 this khac nen ko gan truc tiep se lam thay doi du lieu this truoc va sau no*/
        var $btn = $(this);
        $.ajax({
            url: 'Cart/UpdateToCart',
            type: 'POST',
            data: {
                id: itemID,
                qty: NewQty,
                opera: operation,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                if (response.success) {
                    quantityInput.val(NewQty);
                    let price = parseFloat($btn.closest('tr').find('.price').text().replace(/[^0-9]+/g, ''));
                    var totalPrice = price * NewQty;
                    $btn.closest('tr').find('.priceTotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalPrice));
                    /*$('.payment').find('.priceTotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalPrice));*/
                    updateQtyAfterCheck();
                }
                else {
                    alert('Cập nhật số lượng không thành công.' + response.message);
                }
            },
            error: function (response) {
                alert('Đã xảy ra lỗi trong quá trình cập nhật số lượng.');
            }
        });
    });

    /*Tăng giảm số lượng (Chi ap dung duoc 1 tang giam so luong)*/
    $('#increase').click(() => {
        let qty = parseInt($('#quantity').val());
        $('#quantity').val(qty + 1);
    });

    $('#decrease').click(() => {
        let qty = parseInt($('#quantity').val());
        if (qty > 1) $('#quantity').val(qty - 1);
    });


    // page product detail de them san pham vao gio hang
    $('.btn-option').on('click', function () {
        let btn = $(this);
        const itemID = $('.productID').data('id');
        let qtyInputNumber = parseInt($('#quantity').val()) || 1;

        let colorBtn = null;
        let sizeBtn = null;
        if ($('.color-option').hasClass('color-active') || $('.size-option').hasClass('size-active')) {

            // colorBtn = $('#selectColor').val(); Cach 1 (line 242 de lay gia tri #selectColor)
            sizeBtn = $('#selectSize').val();
            colorBtn = $('.color-option.color-active').data('color'); // Cach 2
        }

        let optionMethod = btn.data('method');

        if (optionMethod == 'cart') {

            $.ajax({
                url: '/Cart/AddToCart',
                type: 'POST',
                data: {
                    productID: itemID,
                    qty: qtyInputNumber,
                    size: sizeBtn,
                    color: colorBtn,
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                },

                success: function (response) {
                    if (response.success) {
                        console.log("Thêm sản phẩm vào giỏ hàng thành công." + response.message);
                        window.location.href = `/Product/ProductDetail/${itemID}`;
                    }
                    else {
                        console.log("Thêm sản phẩm vào giỏ hàng thất bại." + response.message);
                        window.location.href = `/Product/ProductDetail/${itemID}`;
                    }
                },

                error: function (response) {
                    console.log("Sản phẩm thêm vào bị lỗi!!!" + response.message);
                    alert('Sản phẩm thêm vào bị lỗi !!!');
                }
            });
        }
        else {
            // xu li khi mua truc tiep
        }
    });

    // Xoa san pham trong gio hang
    $('.close').on('click', function () {
        const itemID = $(this).data('id') ?? 0;

        $.ajax({
            url: '/Cart/DeleteToCart',
            type: 'POST',
            data: {
                id: itemID,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                if (response.success) {
                    console.log("Xóa sản phẩm thành công." + response.message);
                    window.location.href = `/Cart`;
                }
                else {
                    console.log("Xóa sản phẩm thất bại." + response.message);
                    window.location.href = `/Cart`;
                }
            },

            error: function (response) {
                console.log("Sản phẩm xóa bị lỗi!!!" + response.message);
                alert('Sản phẩm xóa bị lỗi !!!');
            }
        });
    });

    $('.checkbox').on('click', function () {

        if ($(this).hasClass('active')) {

            $(this).removeClass('active');
        }
        else {
            $(this).addClass('active');
        }

        updateQtyAfterCheck();
    });

    // Tinh phan thanh toan san pham da chon (Cart/index)
    $('#buy').on('click', function () {
        const btn = $(this);
        let itemID = $('.productID').data('id');

    });

    /*chon active cho size va color*/
    $('.size-option').click(function () {
        if ($(this).hasClass('size-active')) {

            $(this).removeClass('size-active');
        }
        else {
            $('.size-option').removeClass('size-active');
            $(this).addClass('size-active');
        }
    });

    $('.color-option').click(function () {

        if ($(this).hasClass('color-active')) {

            $(this).removeClass('color-active')
        }
        else {
            $('.color-option').removeClass('color-active');
            $(this).addClass('color-active')
        }
    });

});

// dung cho hieu ung hinh anh qua lai (product detail)
var owl;
$(document).ready(function () {
    owl = $('.owl-carousel-fullwidth').owlCarousel({
        items: 1,
        loop: true,
        dots: false,
        nav: true
    });
});

function changeImage(el) {
    const index = $(el).index();
    owl.trigger('to.owl.carousel', [index, 700]);

    $('.thumbnail-img').removeClass('image-active');
    $(el).addClass('image-active');
}

// Thoi gian thong bao them thanh cong hoac that bai
$(document).ready(function () {
    setTimeout(() => {
        $('.alert').alert('close')
    }, 5000);
})

/* Lay gia tri color -> product detail*/
function selectColor(el) {
    const color = $(el).data('color');
    $('#selectColor').val(color);
}

/*Lay gia tri size -> product detail*/
function selectSize(el) {
    const size = $(el).data('size');
    $('#selectSize').val(size);
}
/*product-detail*/
