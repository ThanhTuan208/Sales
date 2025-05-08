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

/*product-detail*/
//const imgs = document.querySelectorAll('.img-select a');
//const imgBtns = [...imgs];
//let imgId = 1;

//imgBtns.forEach((imgItem) => {
//    imgItem.addEventListener('click', (event) => {
//        event.preventDefault();
//        imgId = imgItem.dataset.id;
//        slideImage();
//    });
//});

//function slideImage() {
//    const displayWidth = document.querySelector('.img-showcase img:first-child').clientWidth;

//    document.querySelector('.img-showcase').style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
//}

//window.addEventListener('resize', slideImage);


// Tăng giảm số lượng
$('#increase').click(() => {
    let qty = parseInt($('#quantity').val());
    $('#quantity').val(qty + 1);
});

$('#decrease').click(() => {
    let qty = parseInt($('#quantity').val());
    if (qty > 1) $('#quantity').val(qty - 1);
});

// Chọn size mượt mà
$('.size-option').click(function () {
    $('.size-option').removeClass('size-active');
    $(this).addClass('size-active');
});

$('.color-option').click(function () {
    $('.color-option').removeClass('color-active');
    $(this).addClass('color-active')
}
)
/*product-detail*/


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