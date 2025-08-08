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

    // Truyen thong tin gmail cua contactUs
    $('.sendMess').on('click', function () {
        const fname = $('#fname').val();
        const lname = $('#lname').val();
        const email = $('#email').val();
        const subject = $('#subject').val();
        const message = $('#message').val();

        $.ajax({
            url: '/Home/Contact',
            type: 'POST',
            data: {
                FirstName: fname,
                LastName: lname,
                Email: email,
                Subject: subject,
                Message: message,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                $('#contactForm').find('.alert').remove();
                if (response.success) {
                    $('#contactForm').prepend('<div class="alert alert-success alert-dismissible show" role="alert">' + response.message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
                    $('#contactForm')[0].reset(); // xoa du lieu da them sau khi gui thanh cong, false se du lai nham tranh mat du lieu khi them that bai
                    setTimeout(function () {
                        $('#contactForm').find('.alert').fadeOut('slow', function () {
                            $(this).remove();
                        });
                    }, 3000);
                } else {
                    $('#contactForm').prepend('<div class="alert alert-danger alert-dismissible show" role="alert">' + response.message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
                    setTimeout(function () {
                        $('#contactForm').find('.alert').fadeOut('slow', function () {
                            $(this).remove();
                        });
                    }, 3000);
                }
            },

            error: function () {
                $('#contactForm').find('.alert').remove();
                $('#contactForm').prepend('<div class="alert alert-danger alert-dismissible show" role="alert">Đã xảy ra lỗi trong quá trình gửi. Vui lòng thử lại.<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
                setTimeout(function () { $('#contactForm').find('.alert').fadeOut('slow', function () { $(this).remove(); }); }, 3000);
            }
        });
    });

    // truyen du lieu dang nhap, dang ky qua pt post
    $('.bn5.login').on('click', function () {
        const btn = $(this);
        const email = $('.email').val();
        const pass = $('.pass').val();
        const check = $('.form-check-input').is(':checked');
        const productID = $('.productID').data('id') || 0;

        let controller = "Auth";
        let ProductID = parseInt(productID) > 0 ? productID : '';
        let action = ProductID > 0 ? "LoginByProductID" : "Login";
        const url = productID > 0 ? `/${controller}/${action}/${productID}` : `/${controller}/${action}`;

        $.ajax({
            url: url,
            type: 'POST',
            data: {
                Email: email,
                Password: pass,
                RememberMe: check,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                if (response.success) {
                    console.log("Đăng nhập thành công: " + response.message);
                    if (response.authenticated == 1) {
                        window.location.href = `/Product/ProductDetail/${response.productID}`;
                    }
                    else {
                        window.location.href = "/Home/Index";
                    }
                }
                else {
                    console.log("Đăng nhập thất bại: " + response.message);

                    $('.text-danger').text('');

                    if (response.errors && response.errors.length > 0) {
                        response.errors.forEach(error => {
                            $('span[data-valmsg-for="InfoGeneral"]').each(function () {
                                if (error.includes($(this).attr('data-valmsg-for')) || error.includes('Email') || error.includes('mật khẩu')) {
                                    $(this).text(error).show();
                                }
                            });
                        });
                    }
                }
            },

            error: function (xhr) {
                console.log('Lỗi AJAX: ', xhr.responseText); // Log chi tiết lỗi
                alert('Đã xảy ra lỗi trong quá trình đăng nhập: ' + (xhr.responseText || 'Không xác định'));
            }
        });
    });

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

            //(line 242 de lay gia tri #selectColor)
            // colorBtn = $('#selectColor').val(); Cach 1 -> gia tri se tu luu vao colorBtn ngay ca khi client click gia tri co active va tat di khi ko co active nhung van lay gia tri 

            sizeBtn = $('.size-option.size-active').data('size');
            colorBtn = $('.color-option.color-active').data('color');
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
                    $('.alertInfo').find('.alert').remove();
                    if (response.success) {
                        console.log("Thêm sản phẩm vào giỏ hàng thành công." + response.message);

                        $('a.cart small.count').text(response.qtyNewCart); // Cập nhật số hiển thị trong navbar
                        $('.countCart').val(response.qtyNewCart);

                        $('.alertInfo').prepend('<div class="alert alert-success alert-dismissible show" role="alert">' + response.message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
                        setTimeout(function () {
                            $('.alertInfo').find('.alert').fadeOut('slow', function () {
                                $(this).remove();
                            });
                        }, 3000);
                        //window.location.href = `/Product/ProductDetail/${itemID}`;
                    }
                    else {
                        console.log("Thêm sản phẩm vào giỏ hàng thất bại." + response.message);
                        if (response.authenticated == 1) {

                            $('.alertInfo').prepend('<div class="alert alert-danger alert-dismissible show" role="alert">' + response.message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
                            setTimeout(function () {
                                $('.alertInfo').find('.alert').fadeOut('slow', function () {
                                    $(this).remove();
                                });
                            }, 3000);
                            //window.location.href = `/Product/ProductDetail/${response.productID}`;
                        }
                        else {
                            window.location.href = `/Auth/LoginByProductID/${response.productID}`;
                        }
                    }
                },

                error: function (response) {
                    console.log("Sản phẩm thêm vào bị lỗi!!!" + response.message || " Không xác định");
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
    $('.buy').on('click', function () {
        const btn = $(this);
        let priceTotal = parseFloat(btn.closest('.payment').find('.price-complete').text().replace(/[^0-9]/g, ''));


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

// cap nhat so luong va gia san pham sau khi check va tang giam so luong cho gia sp
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