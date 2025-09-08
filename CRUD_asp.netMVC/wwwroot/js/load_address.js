import { updateQtyAfterCheck, GetArrIDChecked, LoadView } from './js.js';

$(document).ready(function () {

    LoadView();

    $(document).off('click', '.deleteAddress').on('click', '.deleteAddress', function () {


    });

    // Hien thi cap nhat dia chi
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
        const isDefault = row.find('#sDefaultAddress').val();

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
            console.log("false. ");
            return;
        }

        // Chuyen tu select address -> add address
        const isAddress = true;
        const updateAddress = true;

        //$('.address-container').hide();
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
                    if (data) {
                        console.log("Dữ liệu JSON đã tải");
                    } else {
                        console.log("Không thể tải dữ liệu JSON.");
                    }
                });

                $("#isDefaultAddress").prop('checked', isDefault); // Cập nhật checkbox

                //$("#ward").val(ward);
            },
            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });

    // Loc ky tu nhap so
    $(document).on('input paste', '#phonenumber', () => {
        const $input = $(this);
        let value = $input.val().replace(/[^0-9]/g, ''); // Giu lai so
        $input.val(value);
    });

    // Them, cap nhat dia chi cho user (chung button click)
    $(document).on('click', '.updateAddress', function () {
        // Xóa các lỗi cũ
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
                    console.log("True: " + response.message);

                    // Dieu huong quay ve list address sau khi them thanh cong
                    GeneralAjaxResponse(true, false);

                } else {
                    console.log("False: " + response.message);
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

    // Xóa viền đỏ khi người dùng chỉnh sửa hoặc chọn giá trị mới
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
    // Them dia chi cho user

    // Hien thi them address
    $(document).off('click', '.add-card-content').on('click', '.add-card-content', function () {

        $(".address-container").fadeOut(300);  // an form d/s dia chi
        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            console.log("false. ");
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
                console.log("Hiển thị modal thêm địa chỉ thành công. ");

                $(".modal-left").html(response); // render vào modal
                updateQtyAfterCheck();
                LoadDataAddress(); // hien thi json address VN
            },
            error: function () {
                alert("Lỗi không hiển thị !");
            }
        });
    });

    // Su kien dong list address
    //$(document).on('click', '.btn-close-address, .returnAddress', function () {
    $(document).on('click', '.btn-close-address', function () {

        $(".address-form").fadeOut(300);  // an form dia chi

        let ids = [];
        let ArrChecked = GetArrIDChecked(ids);
        if (!ArrChecked) {
            console.log("false. ");
            return;
        }

        // tra ve lai modalPaymentPatial
        const isAddress = false;
        const updateAddress = false;

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
                $(".modal").html(response); // render vào modal
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
        console.log("false. ");
        return;
    }

    // tra ve lai modalPaymentPatial

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
                    console.log("Tỉnh được chọn:", provinceSelect.getItem(provinceCode.Code)?.textContent || provinceName);

                    // Cập nhật danh sách phường/xã
                    updateWardOptions(provinceCode.Code);

                    // Nếu có wardName, chọn phường/xã
                    if (wardName) {
                        const wardCode = provinceCode.Wards.find(w => w.Name === wardName)?.Code;
                        if (wardCode) {
                            wardSelect.setValue(wardCode);
                            console.log("Phường/xã được chọn:", wardSelect.getItem(wardCode)?.textContent || wardName);
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
}

//Xu ly su kien cho danh sach dia chi
$(function () {

    // Khi trang load: hiện nhãn cho checkbox đang checked (nếu có)
    $('input[name="defaultAddress"]').each(function () {
        if ($(this).is(':checked')) {
            $(this).closest('.checkbox-wrap').find('.default-label').show();
        } else {
            $(this).closest('.checkbox-wrap').find('.default-label').hide();
        }
    });

    // Click vào row: set checkbox = true (không dùng trigger click)
    $(document).on('click', 'tbody tr', function (e) {
        // Nếu click vào checkbox hoặc button thì bỏ qua (để tránh double)
        if ($(e.target).is("input[type='checkbox'], .btn, .btn *")) return;

        const $input = $(this).find('input[name="defaultAddress"]');
        // nếu đã checked thì không làm gì
        if ($input.prop('checked')) return;

        $input.prop('checked', true).trigger('change');
    });

    // Khi checkbox thay đổi
    //$(document).on('change', 'input[name="defaultAddress"]', function () {
    //    // bỏ chọn và ẩn nhãn mọi checkbox khác
    //    $('input[name="defaultAddress"]').not(this).prop('checked', false)
    //        .closest('.checkbox-wrap').find('.default-label').hide();

    //    // hiện nhãn cho checkbox đang checked (nếu checked)
    //    if ($(this).is(':checked')) {
    //        $(this).closest('.checkbox-wrap').find('.default-label').show();
    //    } else {
    //        $(this).closest('.checkbox-wrap').find('.default-label').hide();
    //    }
    //});

});
//Xu ly su kien cho danh sach dia chi