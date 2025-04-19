
document.addEventListener("DOMContentLoaded", function () {
    let listNav = document.querySelectorAll('.nav-link');

    // Lấy đường dẫn hiện tại của trang web
    let currentPath = window.location.pathname;

    listNav.forEach((link) => {
        link.classList.remove('active');

        // So sánh đường dẫn của từng link với đường dẫn hiện tại
        let home = link.getAttribute("href");
        if (home === currentPath || (home === "/Home/Index" && currentPath === "/")) {
            link.classList.add("active");
        }

        // Thêm sự kiện click để chuyển trạng thái active
        link.addEventListener("click", function () {
            listNav.forEach(nav => nav.classList.remove("active"));
            this.classList.add("active");
        });
    });
});


// dung cho phan tu navbar-brand
document.querySelectorAll('.nav-link').forEach((link) => {
    link.addEventListener('mouseenter', () => {
        document.querySelector('.navbar-brand').classList.add("hover-effect");
    });

    link.addEventListener('mouseleave', () => {
        document.querySelector(".navbar-brand").classList.remove('hover-effect');
    });
});

// chinh navbar doi mau khi truot xuong
window.addEventListener("scroll", () => {
    let navbar = document.querySelector('.navbar');
    if (window.scrollY > 50) {
        navbar.classList.add("scrolled");
    }
    else {
        navbar.classList.remove("scrolled")
    }
});

// layoutpage1 line 112
$(document).ready(function () {
    $(".owl-carousel").owlCarousel({
        items: 4,
        autoplay: true,
        loop: true,
        nav: true,
        dots: true,

    });
});


