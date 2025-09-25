//const { regions } = require("modernizr");

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

    $(function () { // Loading screen cho thay doi email ho so
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/changeEmailProfile")
            .build();

        connection.start()
            .catch(err => console.error(err));

        connection.on("ChangeEmailProfile", () => {
            const overlay = $("#loadingOverlay");
            const spinner = overlay.find(".spinner-container");

            overlay.show();   // bật overlay phủ hết màn hình
            spinner.show();

            // Ẩn modal và overlay sau 3s
            setTimeout(() => {

                overlay.fadeOut();
                window.location.href = `/Home/MyProfile`;

            }, 3000);

        })
    });

    $(function () { // Loading screen cho thay doi chung
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/lazyLoad")
            .build();

        connection.start()
            .catch(err => console.error(err));

        connection.on("LazyLoad", () => {
            const overlay = $("#loadingOverlay");
            const spinner = overlay.find(".spinner-container");

            overlay.show();   // bật overlay phủ hết màn hình
            spinner.show();

            // Ẩn modal và overlay sau 3s
            setTimeout(() => {

                overlay.fadeOut(300);
                spinner.fadeOut(300);

            }, 3000);

        })
    });

    // Xóa viền đỏ khi người dùng chỉnh sửa hoặc chọn giá trị mới //
    $(document).on('change input', '#username, #phone', function () {

        const $field = $(this);
        $($field).removeClass('error');
        $($field).siblings('.error-message').remove();
    });

    // Cap nhat form ho so
    $(document).off('click', '.btn-save-profile').on('click', '.btn-save-profile', function (e) {

        e.preventDefault();
        $('input[type=text]').removeClass('error');
        $('.error-message').remove();

        const username = $('#username').val();
        const phone = $('#phone').val();
        const date = $('#date').val();
        const avatar = $('#avatarInput')[0].files[0];
        const gender = $('input[name=gender]:checked').val();

        let formData = new FormData();
        formData.append('UserName', username);
        formData.append('PhoneNumber', phone);
        formData.append('DateOfBirth', date);
        formData.append('ProfileImage', avatar);
        formData.append('Gender', gender);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
        console.log(phone);
        $.ajax({
            url: "/Home/UpdateProfile",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {
                if (response.success) {
                    window.location.href = `/Home/MyProfile`;
                }
                else {
                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        let $field = $(`#${field.toLowerCase()}`);
                        $field.addClass('error');
                        messages.forEach(message => {
                            $field.after(`<span class="error-message text-danger">${message}</span>`);
                        });
                    });
                }
            },

            error: function () {
                alert("Lỗi cập nhật hồ sơ !");
            }
        });
    });


    // Ngan ky tu khac ngoai ky tu so khi nhap sdt //
    $(document).on('input paste', '#phone', function () {
        const $input = $(this);
        let value = $input.val().replace(/[^0-9]/g, ''); // Giu lai so
        $input.val(value);
    });

    // Xu ly quay ve ho so //
    $(document).off('click', '.btn-return-profile').on('click', '.btn-return-profile', function (e) {

        e.preventDefault();
        window.location.href = "/Home/MyProfile";
    });

    // Kiem tra ma otp cua gmail ho so //
    function OPTCodeEmail(callback) {

        $("#confirmotpcode").on("input", function () {

            $('.error-message').remove();

            let value = $(this).val();

            // chỉ cho phép số
            if (!/^\d*$/.test(value)) {
                $(this).val(value.replace(/\D/g, "")); // loại bỏ ký tự không phải số
            }

            if (value.length === 4) {
                callback(value);
            }
        });
    }

    // Cập nhật email trong profile //
    $(document).off('click', '.btn-auth-profile').on('click', '.btn-auth-profile', function (e) {

        e.preventDefault();

        $('#emailnew').removeClass('error');
        $('.error-message').remove();

        let email = $('#emailnew').val();

        $.ajax({
            url: "/Home/UpdateEmailProfile",
            type: "POST",
            data: {
                Email: email,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {

                // thong bao chung
                if (response.errors) {
                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];

                        let $field = $(`#${field.toLowerCase()}`);

                        //Gan loi de hien thi vien do loi

                        if (response.success) {

                            messages.forEach(message => {
                                $field.after(`<span class="error-message text-success">${message}</span>`);
                            });

                            $('.otp').fadeIn(500);

                            // goi OPTCodeEmail duoc gan gia tri trong ham va tra ve otp
                            OPTCodeEmail(function (otpCode) {

                                $.ajax({
                                    url: "/Home/ConfirmEmail",
                                    type: "POST",
                                    data: {
                                        NewEmail: response.email,
                                        UserID: response.userid,
                                        OTPCode: otpCode,
                                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                                    },

                                    success: function (response) {
                                        if (response.errors) {
                                            Object.keys(response.errors).forEach(function (field) {
                                                const messages = response.errors[field];

                                                let $field = $(`#${field.toLowerCase()}`);

                                                //Gan loi de hien thi vien do loi

                                                if (!response.success) {
                                                    $field.addClass('error');
                                                    messages.forEach(message => {
                                                        $('.otp').after(`<span class="error-message text-danger text-center">${message}</span>`);
                                                    });
                                                }
                                            });
                                        }
                                    },

                                    error: function () {
                                        alert("Lỗi không hiển thị !");
                                    }
                                });
                            })
                        }

                        else {
                            $field.addClass('error');
                            messages.forEach(message => {
                                $field.after(`<span class="error-message text-danger">${message}</span>`);
                            });
                        }
                    });
                }
            },

            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });


    // Goi giao dien modal change email profile // 
    $(document).off('click', '#changeemail').on('click', '#changeemail', function (e) {

        e.preventDefault();

        $.ajax({
            url: "/Home/RedirecToEmailProfile",
            type: "POST",

            success: function (response) {
                $(".profile-content").fadeOut(300, function () {
                    $(this).html(response).fadeIn(700);
                });
            },

            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });


    $(function () { // Them hinh cho profile //
        $(document).off('click', '.btn-select-image').on('click', '.btn-select-image', function () {

            $('#avatarInput').click(); // lay anh


        });

        $('#avatarInput').on('change', function (event) {
            const file = event.target.files[0];

            if (file) {
                const render = new FileReader();
                render.onload = function (e) {
                    $('#avatarpreview').attr('src', e.target.result);
                };

                render.readAsDataURL(file);
            }
        });
    })

    // Chon dia chi gio hang //
    $(document).off('click', '.select-address').on('click', '.select-address', function (e) {
        e.preventDefault();

        let ids = [];
        GetArrIDChecked(ids);
        const isAddress = true;

        $.ajax({
            url: "/Cart",
            type: "GET",
            data: { arrID: ids, IsAddress: isAddress },
            traditional: true, // bind mảng
            success: function (response) {
                $(".modal-left").html(response); // render vào modal// Cập nhật hiển thị của <span class="default-label">

                console.log("Mở modal địa chỉ thành công. ");
            },
            error: function () {
                alert("Lỗi không hiển thị modal địa chỉ !");
            }
        });
    });

    // Xu ly button checkout
    $(document).off('click', '.buy.bn54').on('click', '.buy.bn54', function (e) {
        e.preventDefault(); // ngan button type submit

        let ids = [];
        const IsGetArr = GetArrIDChecked(ids);

        if (!IsGetArr.success || ids === null) {
            return;
        }
        console.log(ids);

        $.ajax({
            url: "/Cart/ShowQrModalCart",
            type: "GET",
            data: {
                arrID: ids,
                PaymentMethod: IsGetArr.paymentMethod,
            },
            traditional: true, // quan trọng để bind mảng
            success: function (response) {
                $(".modal").html(response); // render vào modal Index

                // Cap nhat gia tong
                $(".modal-overlay").fadeIn(300);
                $(".modal").addClass("active").fadeIn(300);

                updateQtyAfterCheck();
            },
            error: function (response) {
                alert("Lỗi load sản phẩm lên modal: " + response.message || "khong xac dinh");
            }
        });
    });

    // Đóng modal khi click nút đóng / dung off, on de tranh luu tru connsole vao DOM
    $(document).off('click', '.btn-close').on('click', '.btn-close', function () {
        $(".modal").removeClass("active").fadeOut(200);
        $(".modal-overlay").fadeOut(200);
    });

    // Cap nhat brand
    $('#btnBrandEdit').on('click', function () {

        const id = $('#id').val();
        const name = $('#name').val();
        const image = $('#image')[0].files[0];
        const picturePath = $('#picturePath').val();
        const description = $('#description').val();

        console.log(image);
        let formData = new FormData();
        formData.append("ID", id);
        formData.append("Name", name);
        formData.append("Picture", image);
        formData.append("PicturePath", picturePath);
        formData.append("Description", description);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        $.ajax({
            url: "/Admin/EditBrand",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {
                if (response.success) {
                    console.log("Thông báo: " + response.message);
                    window.location.href = "/Admin/BrandList";
                }
                else {
                    console.log("Thông báo: " + response.message || "KHong xac dinh");
                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`).text(messages);
                    });
                }
            },

            error: function (response) {
                console.log("Lỗi: " + response.message || "Khong xac dinh");
                alert("Lỗi thương hiệu: " + response.message || "Khong xac dinh")
            }
        });
    });

    // Them thuong hieu cho brand
    $('#btnBrandCreate').on('click', function () {

        const name = $('#name').val();
        const image = $('#image')[0].files[0];
        const description = $('#description').val();

        console.log(image);
        let formData = new FormData();
        formData.append("Name", name);
        formData.append("Picture", image);
        formData.append("Description", description);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        $.ajax({
            url: "/Admin/CreateBrand",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {
                if (response.success) {
                    console.log("Thông báo: " + response.message);
                    window.location.href = "/Admin/BrandList";
                }
                else {
                    console.log("Thông báo: " + response.message || "KHong xac dinh");
                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`).text(messages);
                    });
                }
            },

            error: function (response) {
                console.log("Lỗi: " + response.message || "Khong xac dinh");
                alert("Lỗi thương hiệu: " + response.message || "Khong xac dinh")
            }
        });

    });

    // Them hinh cho createBrand
    $("#image").on("change", function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $("#previewImage").attr("src", e.target.result).show();
            }
            reader.readAsDataURL(file);
        }
    });

    // Toggle sidebar on mobile
    document.addEventListener("DOMContentLoaded", function () {
        const sidebar = document.querySelector('.sidebar');
        const toggleBtn = document.createElement('button');
        toggleBtn.className = 'btn btn-dark d-block d-md-none position-absolute top-0 end-0 m-2';
        toggleBtn.innerHTML = '<i class="bi bi-list"></i>';
        document.body.appendChild(toggleBtn);

        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('active');
        });

        // Collapse sidebar on desktop (optional)
        let isCollapsed = false;
        sidebar.addEventListener('dblclick', function () {
            sidebar.classList.toggle('collapsed');
            isCollapsed = !isCollapsed;
        });
    });

    // Truyen du lieu san pham, ktra dkien them hoac sua
    //$('#btnAdminProduct').on('click', function () {
    //    // Lấy dữ liệu từ form
    //    const id = $('#id').val();
    //    const name = $('#name').val();
    //    const description = $('#description').val();
    //    const feature = $('#feature').val() || 0;
    //    const cate = $('#cate').val() || 0;
    //    const brand = $('#brand').val() || 0;
    //    const gender = $('#gender').val() || 0;
    //    const newPrice = $('#newPrice').val();
    //    const oldPrice = $('#oldPrice').val();
    //    const qty = $('#qty').val();
    //    const material = $('#material').val() || [];
    //    const color = $('#color').val() || [];
    //    const size = $('#size').val() || [];
    //    const season = $('#season').val() || [];
    //    const style = $('#style').val() || [];
    //    const tag = $('#tag').val() || [];
    //    const files = $('#picture')[0].files;

    //    // Tạo formData để gửi dữ liệu và file
    //    const formData = new FormData();
    //    if (id) formData.append('ID', id);
    //    formData.append('Name', name);
    //    formData.append('Description', description);
    //    formData.append('NewPrice', newPrice);
    //    formData.append('OldPrice', oldPrice);
    //    formData.append('Quantity', qty);
    //    formData.append('FeaturedID', feature);
    //    formData.append('GenderID', gender);
    //    formData.append('BrandID', brand);
    //    formData.append('CateID', cate);

    //    // Append mảng các giá trị cho các trường select multiple
    //    if (Array.isArray(material)) {
    //        material.forEach(val => formData.append('SelectedMaterialID', val));
    //    } else {
    //        formData.append('SelectedMaterialID', material);
    //    }
    //    if (Array.isArray(color)) {
    //        color.forEach(val => formData.append('SelectedColorID', val));
    //    } else {
    //        formData.append('SelectedColorID', color);
    //    }
    //    if (Array.isArray(size)) {
    //        size.forEach(val => formData.append('SelectedSizeID', val));
    //    } else {
    //        formData.append('SelectedSizeID', size);
    //    }
    //    if (Array.isArray(season)) {
    //        season.forEach(val => formData.append('SelectedSeasonID', val));
    //    } else {
    //        formData.append('SelectedSeasonID', season);
    //    }
    //    if (Array.isArray(style)) {
    //        style.forEach(val => formData.append('SelectedStyleID', val));
    //    } else {
    //        formData.append('SelectedStyleID', style);
    //    }
    //    if (Array.isArray(tag)) {
    //        tag.forEach(val => formData.append('SelectedTagID', val));
    //    } else {
    //        formData.append('SelectedTagID', tag);
    //    }
    //    //Thêm các file vào formData
    //    for (let i = 0; i < files.length; i++) {
    //        formData.append('Picture', files[i]); // Tên 'Picture' phải khớp với tên thuộc tính trong formData
    //    }
    //    console.log(color);
    //    for (let [key, value] of formData.entries()) {
    //        console.log(`${key}: ${value}`);
    //    }

    //    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

    //    //Gửi AJAX request
    //    $.ajax({
    //        url: id ? `/Admin/Edit/${id}` : '/Admin/Create',
    //        type: 'POST',
    //        data: formData,
    //        processData: false, // Không xử lý dữ liệu (cần cho formData)
    //        contentType: false, // Không đặt contentType (formData tự xử lý)
    //        success: function (response) {
    //            if (response.success) {
    //                console.log("Thành công: " + response.message);
    //                window.location.href = '/Admin/Index';
    //            } else {
    //                console.log("Lỗi: " + response.message);
    //                if (response.errors) {
    //                    $('.text-danger').text('');

    //                    Object.keys(response.errors).forEach(function (field) {
    //                        const messages = response.errors[field];
    //                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
    //                        console.log(field);
    //                        console.log(messages);
    //                        $(`span[data-valmsg-for="${correctField}"]`).text(messages);
    //                    });
    //                }
    //            }
    //        },
    //        error: function (xhr) {
    //            console.log("Lỗi AJAX: ", xhr.responseText);
    //            alert('Đã xảy ra lỗi trong quá trình gửi yêu cầu.');
    //        }
    //    });
    //});

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

    // Thiet lap mat khau moi khi gui code mail
    $('#newPassForget').on('click', function () {
        const email = $('#email').val();
        const code = $('#code').val();
        const newPass = $('#newPass').val();

        $.ajax({
            url: "/Auth/ForgotPassword",
            type: "POST",
            data: {
                Email: email,
                Code: code,
                NewPass: newPass,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                if (response.success) {
                    console.log("Thông báo sucess true: " + response.message || "Không xác định");
                    window.location.href = "/Auth/Login";
                }
                else {
                    console.log("Thông báo sucess false: " + response.message || "Không xác định");
                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`).text(messages);
                    });
                }
            },

            error: function (response) {
                console.log('Lỗi AJAX: ', response.message);
                alert('Đã xảy ra lỗi trong quá trình gửi mã OTP Code: ' + (response.message || 'Không xác định'));
            }
        });
    });

    // Truyen input email@gmail.com de gui code mail
    $('#sendMailForget').on('click', function () {
        const email = $('#email').val();

        $.ajax({
            url: "/Auth/SendOTPCodeMail",
            type: "POST",
            data: {
                Email: email,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                if (response.success) {
                    console.log("Thông báo success true: " + response.message || "Không xác định");
                    $('.text-danger').text('');

                    Object.keys(response.corrects).forEach(function (field) {
                        const messages = response.corrects[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`)
                            .text(messages)
                            .removeClass('text-danger')
                            .addClass('text-success');
                    });
                }
                else {
                    console.log("Thông báo success false: " + response.message || "Không xác định");
                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`)
                            .text(messages)
                            .removeClass('text-success')
                            .addClass('text-danger');;

                    });

                }
            },

            error: function (response) {
                console.log('Lỗi AJAX: ', response.message);
                alert('Đã xảy ra lỗi trong quá trình gửi mã OTP Code: ' + (response.message || 'Không xác định'));
            }
        });
    });

    // Truyen du lieu dang ki tai khoan
    $(document).off('.bn5.register').on('click', '.bn5.register', function () {
        const fname = $('#fname').val();
        const lname = $('#lname').val();
        const uname = $('#uname').val();
        const email = $('#email').val();
        const role = $('#role').val();
        const phone = $('#phone').val();
        const pass = $('#pass').val();
        const confirm = $('#confirm').val();

        $.ajax({
            url: "/Auth/Register",
            type: "POST",
            data: {
                FirstName: fname,
                LastName: lname,
                UserName: uname,
                Email: email,
                Phone: phone,
                RoleID: role,
                Password: pass,
                ConfirmPassword: confirm,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },

            success: function (response) {
                if (response.success) {

                    console.log("Thông báo Email: " + response.message);

                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`)
                            .text(messages)
                            .removeClass('text-danger')
                            .addClass('text-success');
                    });
                }
                else {
                    console.log("Thông báo lỗi: " + response.message);

                    $('.text-danger').text('');
                    if (response.errors !== null || response.errors !== "undefined")
                        Object.keys(response.errors).forEach(function (field) {
                            const messages = response.errors[field];
                            const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                            $(`span[data-valmsg-for="${correctField}"]`)
                                .text(messages)
                                .removeClass('text-success')
                                .addClass('text-danger');
                        });
                    else {
                        alert('Lỗi đăng ký trả về không xác định: ' + response.message);
                    }
                }
            },

            error: function (response) {
                console.log('Lỗi AJAX: ', response.message);
                alert('Đã xảy ra lỗi trong quá trình đăng ký: ' + (response.message || 'Không xác định'));
            }
        });
    });

    // truyen du lieu dang nhap qua pt post
    $('.bn5.login').on('click', function () {
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
                        if (response.role === "Admin") {
                            window.location.href = "/Admin/Index";
                        }
                        else {
                            window.location.href = "/Home/Index";
                        }
                    }
                }
                else {
                    console.log("Đăng nhập thất bại: " + response.message);

                    $('.text-danger').text('');

                    Object.keys(response.errors).forEach(function (field) {
                        const messages = response.errors[field];
                        const correctField = field.charAt(0).toUpperCase() + field.slice(1);
                        $(`span[data-valmsg-for="${correctField}"]`).text(messages);
                    });
                }
            },

            error: function (response) {
                console.log('Lỗi AJAX: ', response.message); // Log chi tiết lỗi
                alert('Đã xảy ra lỗi trong quá trình đăng nhập: ' + (response.message || 'Không xác định'));
            }
        });
    });

    // Tang giam so luong trong gio hang (tranh bi luu lich su trang web khong nhu su dung method post)
    $('.quantity-btn').on('click', function () {
        var $btn = $(this);
        const itemID = $btn.data('id');
        let operation = $btn.data('opera');
        let quantityInput = $btn.siblings('.quantity-input');
        let CurrentQty = parseInt(quantityInput.val());
        let NewQty = operation === '+' ? CurrentQty + 1 : CurrentQty - 1;

        if (NewQty < 1) {
            NewQty = 1;
        }

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
                    //$btn.closest('tr').find('.priceTotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalPrice));
                    /*$('.payment').find('.priceTotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(totalPrice));*/
                    updateQtyAfterCheck();
                }
                else {
                    //alert('Cập nhật số lượng không thành công.' + response.message);
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

    $(document).off('click', '.checkbox').on('click', '.checkbox', function () {

        if ($(this).hasClass('active')) {

            $(this).removeClass('active');
        }
        else {
            $(this).addClass('active');
        }

        updateQtyAfterCheck();
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

    // Thêm animation khi tải trang
    document.addEventListener("DOMContentLoaded", function () {
        const cards = document.querySelectorAll('.product-card');
        cards.forEach(card => {
            card.classList.add('animate-fade-in');
        });
    });

    // Hiệu ứng hover cho nút
    const buttons = document.querySelectorAll('.btn-custom');
    buttons.forEach(button => {
        button.addEventListener('mouseover', () => {
            button.style.transform = 'scale(1.05)';
        });
        button.addEventListener('mouseout', () => {
            button.style.transform = 'scale(1)';
        });
    });

    // dung cho hieu ung hinh anh qua lai (product detail)

});

var owl;
$(document).ready(function () {
    owl = $('.owl-carousel-fullwidth').owlCarousel({
        items: 1,
        loop: true,
        dots: false,
        nav: true
    });
});

$('.thumbnail-img').on('click', function () {
    const index = $(this).index();
    owl.trigger('to.owl.carousel', [index, 700]);

    $('.thumbnail-img').removeClass('image-active');
    $(this).addClass('image-active');
});

// Thoi gian thong bao them thanh cong hoac that bai
//$ (document).ready(function () {
//    setTimeout(() => {
//        $('.alert').alert('close')
//    }, 5000);
//})

/* Lay gia tri color -> product detail*/
$('.color-option').on('click', function () {

    const color = $(this).data('color');
    $('#selectColor').val(color);
});


/*Lay gia tri size -> product detail*/
$('.size-option').on('click', function () {
    const size = $(this).data('size');
    $('#selectSize').val(size);
});

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

LoadView();
// Xu li khi go back va return lai, o check van checked nhung gia ko hien thi 
export function LoadView() {
    // Su dung pageshow de load lai trang (ke ca khi go back va return view)
    $(window).on('pageshow', function () {

        $('.checkbox:checked').each(function () {
            $(this).addClass('active');
        });
        updateQtyAfterCheck();
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
    //if (total > 50000000) {
    //    vat = total * 0.005;
    //}
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
