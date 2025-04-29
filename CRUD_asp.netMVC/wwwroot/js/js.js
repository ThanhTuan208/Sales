
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

