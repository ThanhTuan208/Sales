import { updateQtyAfterCheck, GetArrIDChecked, LoadView } from './js.js';
import { startCountdown } from './globalGeneralFunc.js';
$(document).ready(function () {
    LoadView();

    // Xu ly su kien thong bao user dung vi thanh toan san pham
    const $btnPay = $('#btnPay');
    const $modal = $('#paymentModal');
    const $btnCancel = $('#btnCancel');
    let amountReceiveHub = 0;
    let orderData = null;

    // Mở modal với hiệu ứng fadeIn
    $('#triggerModal').on('click', function () {
        $modal.css('display', 'flex').hide().fadeIn(400);
    });

    // Đóng modal (Chỉ cho phép đóng qua nút Hủy)
    $btnCancel.on('click', function () {
        $modal.fadeOut(300);

        let evt = {
            amountReceive: amountReceiveHub,
            order: orderData
        };
        console.log(evt);
        console.log("Json: " + JSON.stringify(evt));

        $.ajax({
            url: `/Payment/PaymentConfirmWallet`,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(evt),
            success: function (res) {
                console.log(res);
                alert(res.message);
            },
            error: function (err) {
                console.error(err);
                alert("Error Send Data" || err.responseJSON.message);
            }
        });
    });

    // Xử lý thanh toán
    $btnPay.on('click', function () {
        // 1. Ngăn người dùng thao tác lần 2
        $(this).prop('disabled', true);
        $btnCancel.prop('disabled', true);

        // 2. Thay đổi nội dung nút (Loading)
        $(this).html('<i class="fas fa-circle-notch fa-spin"></i> Đang xử lý...');

        // 3. Giả lập gọi API thanh toán
        setTimeout(function () {
            alert("Thanh toán thành công qua ví!");

            // 4. Reset và đóng modal
            $modal.fadeOut(300, function () {
                // Reset lại trạng thái ban đầu sau khi hiệu ứng ẩn kết thúc
                $btnPay.prop('disabled', false).text('Thanh toán bằng Ví');
                $btnCancel.prop('disabled', false);
            });
        }, 2500);
    });

    // Gui cau hoi thanh toan trong vi dung cho user  //
    $(function () {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/questionRes")
            .withAutomaticReconnect([0, 2000, 5000, 10000])
            .build();

        // Hàm đăng ký handler - gọi lại mỗi khi connect/reconnect thành công
        function registerPaymentHandler() {
            connection.on("QuestionResquestUser", (orderId, userId, orderAmount, amountReceive, missingAmount, userWallet, paymentMethod, status, transactionCode) => {
                console.log("Nhận tín hiệu thanh toán thành công:", orderId, missingAmount);

                $('.modal-overlay-wallet').css('display', 'flex');
                $('.order-amount').text(formatVND(orderAmount));
                $('.receive-amount').text(formatVND(amountReceive));
                $('.excess-amount').text(formatVND(userWallet));
                $('.missing-amount').text(formatVND(missingAmount));

                amountReceiveHub = amountReceive;

                orderData = {
                    id: orderId,
                    userID: userId,
                    amount: orderAmount,
                    paymentMethod: paymentMethod,
                    status: status,
                    transactionId: transactionCode,
                    missingAmount: missingAmount
                };
            })
        };
        function formatVND(amount) {
            if (amount === undefined || amount === null) return '0 ₫';

            return amount.toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND'
            });
        }

        // Hàm khởi động connection
        function startConnection() {
            connection.start()
                .then(() => {
                    console.log("SignalR connected successfully");
                    registerPaymentHandler(); // Đăng ký lại handler
                })
                .catch(err => {
                    console.error("SignalR connection failed:", err);
                    setTimeout(startConnection, 5000);
                });
        }

        // Xử lý khi connection bị đóng
        connection.onclose(() => {
            console.warn("SignalR connection closed. Reconnecting...");
        });

        // Xử lý khi reconnect thành công (rất quan trọng!)
        connection.onreconnected(() => {
            console.log("SignalR reconnected successfully!");
            registerPaymentHandler(); // Đăng ký lại handler sau reconnect
        });

        // Bắt đầu kết nối
        startConnection();
    });


    // chay timer ban dau
    //startCountdown(expireSeconds);
    // Tao lai QR
    $(document).off('click', '#resetQR').on('click', '#resetQR', function () {

        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            return;
        }

        $.ajax({
            url: "/Cart/ShowQrModalCart",
            type: "GET",
            data: {
                arrID: ids,
                ResetQR: false,
                PaymentMethod: ArrChecked.paymentMethod
            },
            traditional: true,
            success: function (response) {
                $(".modal").html(response);
                updateQtyAfterCheck();
            },
            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });

        $("#resetQR").hide();
        startCountdown(300); // chay lai timer
    });

    $(document).off('click', '#locationQR').on('click', '#locationQR', function () {

        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            return;
        }

        $.ajax({
            url: "/Cart",
            type: "GET",
            data: {
                arrID: ids,
                IsAddress: true,
                UpdateAddress: false,
            },
            traditional: true,

            success: function (response) {
                $(".modal-left").html(response);
            },

            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });

    // Goi alert realtime khi thanh toan thanh cong //
    $(function () {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/paymentHub")
            .withAutomaticReconnect([0, 2000, 5000, 10000]) // Thử lại ngay, sau 2s, 5s, 10s...
            .build();

        // Hàm đăng ký handler - gọi lại mỗi khi connect/reconnect thành công
        function registerPaymentHandler() {
            connection.on("ReceivePaymentStatus", (orderId, transactionCode, isSuccess) => {
                console.log("Nhận tín hiệu thanh toán thành công:", orderId, transactionCode);

                const overlay = $("#overlayStatus");
                const spinner = overlay.find(".spinner");
                const successIcon = overlay.find(".success-icon");
                const successText = overlay.find(".success-text");

                overlay.show();
                spinner.show();
                successIcon.hide();
                successText.hide();

                setTimeout(() => {
                    spinner.fadeOut(300, () => {
                        successIcon.fadeIn(300);
                        successText.fadeIn(300);
                    });

                    setTimeout(() => {
                        overlay.fadeOut();
                        $(".modal-overlay").fadeOut(200);
                        $(".modal").removeClass("active").fadeOut(200);

                        if (isSuccess) {

                            window.location.href = `/Payment/PaymentSuccess?orderID=${encodeURIComponent(orderId)}&transactionCode=${encodeURIComponent(transactionCode)}`;
                        }
                        else window.location.href = `/Payment/PaymentFail?orderID=${encodeURIComponent(orderId)}&transactionCode=${encodeURIComponent(transactionCode)}`;

                    }, 3000);
                }, 2000);
            });
        }

        // Hàm khởi động connection
        function startConnection() {
            connection.start()
                .then(() => {
                    console.log("SignalR connected successfully");
                    registerPaymentHandler(); // Đăng ký lại handler
                })
                .catch(err => {
                    console.error("SignalR connection failed:", err);
                    setTimeout(startConnection, 5000);
                });
        }

        // Xử lý khi connection bị đóng
        connection.onclose(() => {
            console.warn("SignalR connection closed. Reconnecting...");
        });

        // Xử lý khi reconnect thành công (rất quan trọng!)
        connection.onreconnected(() => {
            console.log("SignalR reconnected successfully!");
            registerPaymentHandler(); // Đăng ký lại handler sau reconnect
        });

        // Bắt đầu kết nối
        startConnection();
    });

    // Xu ly chuc nang, giao dien trang thai thanh toan //
    $(function () {
        const $modal = $("#paymentSection");
        const $extra = $("#extraContent");
        const $order = $("#orderDetails");

        function showProductsUnderModal() {
            // add class to slide modal up
            $modal.addClass("at-top");

            setTimeout(() => {
                const modalHeight = $modal.outerHeight(true); // include margin/padding
                $extra.css("margin-top", modalHeight + 20 + "px");

                // reveal extra content with slideDown-like animation
                $extra.attr("aria-hidden", "false").hide().slideDown(450);
                $("html,body").animate({ scrollTop: 0 }, 450);
            }, 250); // 250ms after class add (tunable)
        }

        setTimeout(showProductsUnderModal, 1500);

        $modal.on("click", function (e) {
            if ($(e.target).closest("a,button").length) return;
            if (!$modal.hasClass("at-top")) showProductsUnderModal();
        });

        // Recompute margin-top on window resize if products visible
        $(window).on("resize", function () {
            if ($modal.hasClass("at-top") && $extra.is(":visible")) {
                const modalHeight = $modal.outerHeight(true);
                $extra.css("margin-top", modalHeight + 20 + "px");
            }
        });

        $modal.on(
            "transitionend webkitTransitionEnd oTransitionEnd",
            function (e) {
                if ($modal.hasClass("at-top")) {
                }
            }
        );

    });

    // Xoa dia chi //
    $(document).off('click', '.deleteAddress').on('click', '.deleteAddress', function () {

        const id = $('#id').val();
        const qr = $('.qrCodeString').val();

        if (!id || id === "undefined") {
            return;
        }

        let formData = new FormData();
        formData.append("addressId", id);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        $.ajax({
            url: "/Address/DeleteAddress",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {
                if (response.success) {

                    let ids = [];
                    let ArrChecked = GetArrIDChecked(ids);
                    if (!ArrChecked) {
                        return;
                    }

                    $.ajax({
                        url: "/Cart/CheckAddressData",
                        type: "GET",
                        data: { arrID: ids },
                        traditional: true,

                        success: function (response) {

                            if (!response.success) {
                                $(".modal-right").html(response);
                                $('#reCodeQR').attr('src', qr);

                                //clearInterval(timerInterval);
                                $("#timer").text("Cần thêm địa chỉ mặc định mới.");
                            }
                        },

                        error: function () {
                            alert("Lỗi cập nhật địa chỉ QR !");
                        }
                    })

                    GeneralAjaxResponse(true, false);
                }
                else {
                    if (response.addressData === 'undefined') {
                        clearInterval(timerInterval);
                        $("#timer").text("QR đã hết hạn");
                    }
                }
            },

            error: function (response) {
                alert(`Loi xoa dia chi ${response.message}!!!`);
            }
        });
    });

    // Hien thi cap nhat dia chi //
    $(document).off('click', '.edit-address-exists').on('click', '.edit-address-exists', function () {

        $(".address-container").fadeOut(300);  // an form d/s dia chi

        // lay dataAddress tu hang duoc click cua foreach
        let row = $(this).closest('tr');
        const id = row.find('.id').val();
        const recipientName = row.find('.recipientname').text();
        const phoneNumber = row.find('.phonenumber').text();
        const street = row.find('.street').val();
        const province = row.find('.province').val();
        const ward = row.find('.ward').val();
        const isDefault = row.find('.default-label').is(':visible');

        let formData = new FormData();
        formData.append('ID', id);
        formData.append('RecipientName', recipientName);
        formData.append('PhoneNumber', phoneNumber);
        formData.append('Street', street);
        formData.append('Province', province);
        formData.append('Ward', ward);
        formData.append('IsDefault', isDefault);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            return;
        }

        // Chuyen tu select address -> add address
        const isAddress = true;
        const updateAddress = true;

        $.ajax({
            url: "/Cart",
            type: "GET",
            data: {
                arrID: ids,
                IsAddress: isAddress,
                UpdateAddress: updateAddress
            },
            traditional: true, // bind mảng

            success: function (response) {

                $(".modal-left").html(response); // render vào modal

                $("#id").val(id);
                $("#recipientname").val(recipientName);
                $("#phonenumber").val(phoneNumber);
                $("#street").val(street);

                // Gọi LoadDataAddress và chọn tỉnh, xã
                LoadDataAddress(province, ward, function (data) {
                    if (!data) {
                        console.log("Không thể tải dữ liệu JSON.");
                    }
                });

                if (isDefault) {
                    $("#isdefault").prop('checked', true); // Cập nhật checkbox
                }

            },
            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });

    // Loc ky tu nhap so //
    $(document).on('input paste', '#phonenumber', function () {
        const $input = $(this);
        let value = $input.val().replace(/[^0-9]/g, ''); // Giu lai so
        $input.val(value);
    });

    // Them, cap nhat dia chi cho user (chung button click) //
    $(document).on('click', '.updateAddress', function () {

        $('.form-control, .ts-wrapper').removeClass('error');
        //$('.error-message').remove();

        const id = $('#id').val();
        const recipientName = $('#recipientname').val();
        const phone = $('#phonenumber').val();
        const street = $('#street').val();
        const provinceCode = $('#province').val(); // Lay ma
        const wardCode = $('#ward').val();
        const isDefault = $('#isdefault').is(':checked');

        // Chuyen ma thanh ten tu mang addressData
        let provinceName = "";
        let wardName = "";
        if (addressData.length > 0) {
            const province = addressData.find(p => p.Code === provinceCode);
            if (province) {
                provinceName = province.Name;
                const ward = province.Wards.find(w => w.Code === wardCode);
                if (ward) {
                    wardName = ward.Name;
                }
            }
        }

        let formData = new FormData();
        if (id) formData.append('ID', id);
        formData.append('RecipientName', recipientName);
        formData.append('PhoneNumber', phone);
        formData.append('Street', street);
        formData.append('Province', provinceName);
        formData.append('Ward', wardName);
        formData.append('IsDefault', isDefault);
        formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

        //for (let [key, value] of formData.entries()) {
        //    console.log(`${key}: ${value}`);
        //}

        let url = id ? "UpdateAddress" : "AddAddress";

        $.ajax({
            url: `/Address/${url}`,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    if (response.status === 'unique' || response.isDefault) {
                        let ids = [];
                        let ArrChecked = GetArrIDChecked(ids);
                        if (!ArrChecked) {
                            return;
                        }

                        $.ajax({
                            url: "/Cart/ShowQrModalCart",
                            type: "GET",
                            data: {
                                arrID: ids,
                                resetQR: true,
                                PaymentMethod: ArrChecked.paymentMethod
                            },
                            traditional: true,
                            success: function (response) {
                                $(".modal-right").html(response);

                                startCountdown(300);
                                updateQtyAfterCheck();
                            },
                            error: function () {
                                alert("Lỗi không hiển thị !");
                            }
                        });
                    }
                    // Dieu huong quay ve list address sau khi them thanh cong
                    GeneralAjaxResponse(true, false);

                } else {
                    if (response.errors) {
                        Object.keys(response.errors).forEach(function (field) {
                            const messages = response.errors[field];
                            const $field = field === 'Province' || field === 'Ward'
                                ? $(`#${field.toLowerCase()}-ts-control`).closest('.ts-wrapper') // Kiem tra thuoc tinh trong Devtool Element HTML
                                : $(`#${field.toLowerCase()}`);

                            //Gan loi de hien thi vien do loi
                            $field.addClass('error');

                            //messages.forEach(message => {
                            //    $field.after(`<span class="error-message">${message}</span>`);
                            //});
                        });
                    }
                }
            },
            error: function (response) {
                alert("Lỗi thông tin địa chỉ: " + (response.message || "Không xác định"));
            }
        });
    });

    // Xóa viền đỏ khi người dùng chỉnh sửa hoặc chọn giá trị mới //
    $(document).on('change input', '#province, #ward, .form-control', function () {
        const $element = $(this);
        let $wrapper;
        if ($element.is('select')) {
            $wrapper = $element.next('.ts-wrapper');
        } else if ($element.hasClass('form-control')) {
            $wrapper = $element; // Xử lý trực tiếp trên .form-control
        }
        if ($wrapper && $wrapper.length) {
            $wrapper.removeClass('error');
            $wrapper.next('.error-message').remove();
        }
    });


    // Hien thi them address //
    $(document).off('click', '.add-card-content').on('click', '.add-card-content', function () {

        $(".address-container").fadeOut(300);  // an form d/s dia chi
        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            return;
        }

        const isAddress = true;
        const updateAddress = true;

        $.ajax({
            url: "/Cart",
            type: "GET",
            data: {
                arrID: ids,
                IsAddress: isAddress,
                UpdateAddress: updateAddress
            },
            traditional: true,

            success: function (response) {
                console.log("Hiển thị modal thêm địa chỉ thành công. ");

                $(".modal-left").html(response);
                updateQtyAfterCheck();
                LoadDataAddress(); // hien thi json address VN
            },
            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });


    // Su kien dong list address //
    $(document).on('click', '.btn-close-address', function () {

        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            return;
        }

        $.ajax({
            url: "/Cart/ShowQrModalCart",
            type: "GET",
            data: { arrID: ids },
            traditional: true,
            success: function (response) {
                $(".modal-left").html(response);
                updateQtyAfterCheck();
            },

            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });

    // Quay laai trang danh sach dia chi
    $(document).off('click', '.returnAddressList').on('click', '.returnAddressList', function () {

        $(".address-form").fadeOut(300);  // an form dia chi

        GeneralAjaxResponse(true, false);
    });

});

function GeneralAjaxResponse(isAddress, updateAddress) {

    let ids = [];
    let ArrChecked = GetArrIDChecked(ids);
    if (!ArrChecked) {
        return;
    }

    $.ajax({
        url: "/Cart",
        type: "GET",
        data: {
            arrID: ids,
            IsAddress: isAddress,
            UpdateAddress: updateAddress
        },
        traditional: true, // bind mảng
        success: function (response) {
            $(".modal-left").html(response); // render vào modal
            updateQtyAfterCheck();
        },
        error: function () {
            alert("Lỗi không hiển thị !");
        }
    });
}

let addressData = [];
// Load du lieu json dia chi VietNam
function LoadDataAddress(provinceName, wardName, callback) {
    const $provinceEl = $("#province");
    const $wardEl = $("#ward");
    const $fullLocationEl = $("#fullLocation");
    const $streetEl = $("#street");

    // Nếu partial hiện tại không có form địa chỉ thì thoát
    if ($provinceEl.length === 0 || $wardEl.length === 0) {
        console.log("Khong ton tai select province || ward DOM");
        return;
    }

    let provinceSelect, wardSelect;

    // Khởi tạo TomSelect cho province
    provinceSelect = new TomSelect($provinceEl[0], {
        create: true,
        placeholder: "Tìm hoặc nhập tỉnh/thành phố",
        onItemAdd: function () {
            this.blur();
        },
        onChange: function (value) {
            updateWardOptions(value);
        }
    });

    // Khởi tạo TomSelect cho ward
    wardSelect = new TomSelect($wardEl[0], {
        create: true,
        placeholder: "Tìm hoặc nhập phường/xã",
        onItemAdd: function () {
            this.blur();
        }
    });

    // Load JSON từ server
    $.getJSON("full_json_generated_data_vn_units.json")
        .done(function (json) {
            addressData = json;
            const options = addressData.map(p => ({ value: p.Code, text: p.Name }));
            provinceSelect.addOptions(options);

            // Nếu có provinceName, chọn tỉnh tương ứng
            if (provinceName) {
                const provinceCode = addressData.find(p => p.Name === provinceName);
                if (provinceCode) {
                    provinceSelect.setValue(provinceCode.Code);
                    //console.log("Tỉnh được chọn:", provinceSelect.getItem(provinceCode.Code)?.textContent || provinceName);

                    // Cập nhật danh sách phường/xã
                    updateWardOptions(provinceCode.Code);

                    // Nếu có wardName, chọn phường/xã
                    if (wardName) {
                        const wardCode = provinceCode.Wards.find(w => w.Name === wardName)?.Code;
                        if (wardCode) {
                            wardSelect.setValue(wardCode);
                            //console.log("Phường/xã được chọn:", wardSelect.getItem(wardCode)?.textContent || wardName);
                        } else {
                            console.log("Không tìm thấy phường/xã:", wardName);
                        }
                    }
                } else {
                    console.log("Không tìm thấy tỉnh:", provinceName);
                }
            }

            callback && callback(addressData);
        })
        .fail(function (jqxhr, textStatus, error) {
            console.error("Lỗi khi tải JSON:", textStatus, error);
            alert("Không thể tải dữ liệu địa chỉ. Vui lòng kiểm tra file JSON.");
        });

    // Cập nhật options cho ward dựa trên province
    function updateWardOptions(provinceCode) {
        wardSelect.clear();        // Xóa lựa chọn hiện tại
        wardSelect.clearOptions(); // Xóa tất cả options
        wardSelect.disable();      // Vô hiệu hóa tạm thời

        if (provinceCode) {
            const province = addressData.find(p => p.Code === provinceCode) || { Wards: [] };
            const wardOptions = province.Wards
                ? province.Wards.map(w => ({ value: w.Code, text: w.Name }))
                : [];
            wardSelect.addOptions(wardOptions);
            wardSelect.enable(); // Kích hoạt lại khi có dữ liệu
        }
        updateFullLocation();
    }

    // Cập nhật khi chọn phường hoặc nhập địa chỉ
    $wardEl.on("change", updateFullLocation);
    $streetEl.on("input change", updateFullLocation);

    function updateFullLocation() {
        const provinceValue = provinceSelect.getValue()
            ? { label: provinceSelect.getItem(provinceSelect.getValue()).textContent }
            : "";
        const wardValue = wardSelect.getValue()
            ? { label: wardSelect.getItem(wardSelect.getValue()).textContent }
            : "";

        const provinceName = provinceValue.label || "";
        const wardName = wardValue.label || "";
        const streetValue = $streetEl.val() || "";

        $fullLocationEl.val(
            provinceName && wardName
                ? `${streetValue} ${wardName}, ${provinceName}`.trim()
                : streetValue || "Địa chỉ sẽ hiển thị ở đây"
        );
    }

    $(function () {
        const $modal = $("#paymentSection");
        const $extra = $("#extraContent");
        const $order = $("#orderDetails");

        function showProductsUnderModal() {
            // add class to slide modal up
            $modal.addClass("at-top");

            setTimeout(() => {
                const modalHeight = $modal.outerHeight(true); // include margin/padding
                $extra.css("margin-top", modalHeight + 20 + "px");

                // reveal extra content with slideDown-like animation
                $extra.attr("aria-hidden", "false").hide().slideDown(450);
                $("html,body").animate({ scrollTop: 0 }, 450);
            }, 250); // 250ms after class add (tunable)
        }

        setTimeout(showProductsUnderModal, 1500);

        $modal.on("click", function (e) {
            if ($(e.target).closest("a,button").length) return;
            if (!$modal.hasClass("at-top")) showProductsUnderModal();
        });

        // Recompute margin-top on window resize if products visible
        $(window).on("resize", function () {
            if ($modal.hasClass("at-top") && $extra.is(":visible")) {
                const modalHeight = $modal.outerHeight(true);
                $extra.css("margin-top", modalHeight + 20 + "px");
            }
        });

        $modal.on(
            "transitionend webkitTransitionEnd oTransitionEnd",
            function (e) {
                if ($modal.hasClass("at-top")) {
                    // optionally hide the order details box to keep the header compact:
                    // $order.slideUp(220);
                    // or keep visible — comment/uncomment above
                }
            }
        );

        // Button demo (you'd replace with real links)
        $("#btnHome").on("click", function (e) {
            e.preventDefault();
            // navigate home
            // location.href = '/';
            alert("Redirect to homepage (demo)");
        });
    });
}

