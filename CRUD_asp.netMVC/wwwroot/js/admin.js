//$(function () { // Cap nhat real-time cho Dashboard View (DAU,UV)
//    const connection = new signalR.HubConnectionBuilder()
//        .withUrl("/DashboardHub", {
//            withCredentials: true
//        })
//        .withAutomaticReconnect()
//        .build();

//    connection.start().catch(err => alert(err));

//    connection.on("ReceiveCurrentStatus", (data) => {

//        let s = data.TotalVisits.toLocaleString();
//        $('#uv').text(data.TotalVisits.toLocaleString());
//        let f = $('#dau').text(data.DailyActiveUsers.toLocaleString());
//        alert("In ra dum tao", s);
//    })
//});

// Truyen du lieu san pham, ktra dkien them hoac sua

$(document).on('click', '.feature-dev', function (e) {
    e.preventDefault();
    alert('🚧 Tính năng đang trong quá trình phát triển');
});

$(document).off('click', '#btnAdminProduct').on('click', '#btnAdminProduct', function (e) {

    e.preventDefault();

    const btn = $(this);

    if (btn.prop('disabled')) return;
    btn.prop('disabled', true).text('Đang xử lý...');

    const id = $('#id').val();
    const name = $('#name').val();
    const description = $('#description').val();
    const qty = $('#product-qty').val();
    let feature = $('#feature').val() || '';
    let cate = $('#cate').val() || '';
    let brand = $('#brand').val() || '';
    let gender = $('#gender').val() || '';
    let newPrice = $('#newPrice').val();
    let oldPrice = $('#oldPrice').val();

    const files = $('#picture')[0].files;

    const arrMaterial = $('#Material option:selected').map(function () {
        return $(this).val();
    }).get();

    const arrSeason = $('#Season option:selected').map(function () {
        return $(this).val();
    }).get();

    const arrStyle = $('#Style option:selected').map(function () {
        return $(this).val();
    }).get();

    const arrTag = $('#Tag option:selected').map(function () {
        return $(this).val();
    }).get();

    const formData = new FormData();
    if (id) formData.append('ID', id);
    formData.append('Name', name);
    formData.append('Description', description);
    formData.append('NewPrice', newPrice);
    formData.append('OldPrice', oldPrice);
    formData.append('Quantity', qty);
    formData.append('FeaturedID', feature);
    formData.append('GenderID', gender);
    formData.append('BrandID', brand);
    formData.append('CateID', cate);
    arrMaterial.forEach(val => formData.append('SelectedMaterialID', val));
    arrSeason.forEach(val => formData.append('SelectedSeasonID', val));
    arrStyle.forEach(val => formData.append('SelectedStyleID', val));
    arrTag.forEach(val => formData.append('SelectedTagID', val));
    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

    for (let i = 0; i < files.length; i++) {
        formData.append('Picture', files[i]);
    }

    let nameAction = id ? 'Edit' : 'Create';

    $.ajax({
        url: id ? `/Admin/${nameAction}Product/${id}` : `/Admin/${nameAction}Product`,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {

            if (response.success) {
                //setTimeout(() => {

                //}, 700);
                window.location.href = '/Admin/ProductList';
            } else {

                btn.prop('disabled', false).text(!id ? 'Tạo' : 'Cập nhật');

                if (response.errors) {
                    $('.text-danger').text('');

                    response.errors.forEach(function (err) {
                        const fieldName = err.field;
                        const message = err.errors[0];

                        $(`span[data-valmsg-for="${fieldName}"]`).text(message);
                    });
                }
                return;
            }
        },
        error: function (xhr) {
            console.log("Lỗi AJAX: ", xhr.responseText);
            alert('Đã xảy ra lỗi trong quá trình gửi yêu cầu.');
        }
    });
});

// Xoa thuoc tinh cho san pham (material, season, style, tag)
$(document).off('click', '.btn-delete-val').on('click', '.btn-delete-val', function (e) {

    e.preventDefault();
    const btn = $(this);
    let typeVal = $('#typeVal').val();
    const id = btn.closest('.new-prop-product').find('#id-prop').val();
    const value = btn.closest('.new-prop-product').find('.name-prop').val();

    if (!id) {
        alert(`Mã của ${typeVal} Không tồn tại !`);
        return;
    }

    $.ajax({
        url: '/Admin/DeletePropTValueForProduct',
        type: 'POST',
        data: {
            id: id,
            value: value,
            type: typeVal,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },

        success: function (data) {

            if (!data.success) {
                if (data.errors) {
                    $('.text-danger').text('');

                    data.errors.forEach(function (err) {
                        const message = err.errors[0];

                        btn.closest('.new-prop-product').find(`.alert-error-sameval`).text(message);
                    });
                }
                return;
            }

            $('.display-detail').html(data.html);
            if (data.propList) {

                const $select = $(`#${data.typeVal}`);
                $select.empty();

                $select.append(`<option value="" class="text-center" disabled>Chọn ${data.nameType} </option>`);

                data.propList.forEach(function (m) {
                    $select.append(`<option value="${m.id}">${m.name}</option>`);
                });
            }
        },

        error: function (err) {

            alert("Lỗi xóa thuộc tính sản phẩm !");
        }
    });
});

// Cap nhat thuoc tinh cho san pham (material, season, style, tag)
$(document).off('click', '.btn-update-val').on('click', '.btn-update-val', function (e) {

    e.preventDefault();
    const btn = $(this);

    const id = btn.closest('.new-prop-product').find('#id-prop').val();
    const value = btn.closest('.new-prop-product').find('.name-prop').val();
    let typeVal = $('#typeVal').val();

    if (!id) {
        alert(`Mã của ${typeVal} Không tồn tại !`);
        return;
    }

    $.ajax({
        url: '/Admin/UpdatePropTValueForProduct',
        type: 'POST',
        data: {
            id: id,
            value: value,
            typeVal: typeVal,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },

        success: function (data) {

            if (!data.success) {
                if (data.errors) {
                    $('.text-danger').text('');

                    data.errors.forEach(function (err) {
                        const message = err.errors[0];

                        btn.closest('.new-prop-product').find(`.alert-error-sameval`).text(message);
                    });

                }
                return;
            }

            $('.display-detail').html(data.html);

            if (data.propList) {

                const $select = $(`#${data.typeVal}`);
                $select.empty();

                $select.append(`<option value="" class="text-center" disabled>Chọn ${data.nameType} </option>`);

                data.propList.forEach(function (m) {
                    $select.append(`<option value="${m.id}">${m.name}</option>`);
                });
            }
        },

        error: function (err) {

        }
    });

});

// Them thuoc tinh cua material, season, style, tag
$(document).off('click', '.btn-add-val').on('click', '.btn-add-val', function (e) {

    e.preventDefault();

    let typeVal = $('#typeVal').val();
    let val = $('#new-value-prop').val();

    $.ajax({

        url: '/Admin/AddPropTValueForProduct',
        type: 'POST',
        data: {
            value: val,
            typeVal: typeVal,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },

        success: function (data) {

            if (!data.success) {
                if (data.errors) {
                    $('.text-danger').text('');

                    data.errors.forEach(function (err) {
                        const message = err.errors[0];

                        $(`#new-value-propName`).text(message);
                    });
                }
                return;
            }

            $('.display-detail').html(data.html);

            if (data.propList) {

                const $select = $(`#${data.typeVal}`);
                $select.empty();

                $select.append(`<option value="" class="text-center" disabled>Chọn ${data.nameType} </option>`);

                data.propList.forEach(function (m) {
                    $select.append(`<option value="${m.id}">${m.name}</option>`);
                });
            }
        },

        error: function (err) {

        }
    });
});


// Cap nhat qty, size, color san pham 
$(document).off('click', '.btn-update-proqty').on('click', '.btn-update-proqty', function () {

    GeneralTempProductQty(3, this);
});

// Xoa so luong san pham qua gia tri size, product, color
$(document).off('click', '.btn-delete-proqty').on('click', '.btn-delete-proqty', function () {

    GeneralTempProductQty(2, this);
});

// Them so luong san pham qua gia tri size, product, color
$(document).off('click', '.btn-add-proqty').on('click', '.btn-add-proqty', function (e) {

    e.preventDefault();
    GeneralTempProductQty(1);
});

function GeneralTempProductQty(typeNum, btn) {

    let idExist = $('#id').val();
    let row = $(btn).closest('.selectTemp');
    let keyQtyVal, keySizeVal, keyColorVal;
    let qtyVal, sizeVal, colorVal, oldSizeVal, oldColorVal;

    if (typeNum == 1) {

        qtyVal = parseInt($('#new-qty').val());
        sizeVal = parseInt($('#size-new-qty').val());
        colorVal = parseInt($('#color-new-qty').val());
    }
    else if (typeNum == 2) {

        qtyVal = parseInt(row.find('.exist-qty').val());
        sizeVal = parseInt(row.find('.size-qty').val());
        colorVal = parseInt(row.find('.color-qty').val());
    }
    else {

        qtyVal = parseInt(row.find('.exist-qty').val());
        sizeVal = parseInt(row.find('.size-qty').val());
        colorVal = parseInt(row.find('.color-qty').val());
        oldSizeVal = parseInt(row.find('.size-qty').data('oldsize'));
        oldColorVal = parseInt(row.find('.color-qty').data('oldcolor'));
    }

    if (isNaN(colorVal) || isNaN(sizeVal) || isNaN(qtyVal)) {
        return;
    }

    if (typeNum == 1 || typeNum == 2) {
        keyQtyVal = 'qtyVal';
        keySizeVal = 'sizeVal';
        keyColorVal = 'colorVal';
    }
    else {
        keyQtyVal = 'newQtyVal';
        keySizeVal = 'newSizeVal';
        keyColorVal = 'newColorVal';
    }

    let url = typeNum == 1 ? 'Add' : typeNum == 2 ? 'Delete' : 'Update';

    let formData = new FormData();
    if (typeNum == 3) {
        formData.append('oldSizeVal', oldSizeVal);
        formData.append('oldColorVal', oldColorVal);
    }
    if (idExist) formData.append('id', idExist);
    formData.append(keyQtyVal, qtyVal);
    formData.append(keySizeVal, sizeVal);
    formData.append(keyColorVal, colorVal);
    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: `/Admin/${url}TempProductQty`,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,

        success: function (data) {
            $('.display-detail').html(data.html);

            if (data.qty > 0) {
                $('#product-qty').val(data.qty);
            }
            CheckCountQty();
        },

        error: function (err) {
            console.log('Lỗi:', err);
        }
    });
}

// Hien thi so luong san pham qua color, size   
$(document).off('click', '#update-qty').on('click', '#update-qty', function (e) {

    e.preventDefault();

    let arrColor, arrSize;
    let idExist = $('#id').val();

    if (idExist) {
        arrColor = $('#Color option:selected').map(function () {
            return $(this).val();
        }).get();

        arrSize = $('#Size option:selected').map(function () {
            return $(this).val();
        }).get();
    }

    $.ajax({
        url: '/Admin/DisplayProductQty',
        type: 'GET',
        data: {
            idExist: idExist,
            arrColor: arrColor,
            arrSize: arrSize
        },
        traditional: true,

        success: function (data) {
            $('.display-detail').html(data.html);

            if (data.qty > 0) {
                $('#product-qty').val(data.qty);
            }
        },

        error: function (err) {
            console.log('Lỗi:', err);
        }
    });
});

// lay ops value
$(document).off('click', '.form-select.detail').on('click', '.form-select.detail', function () {

    let arrValue = $(this).find('option:not(:disabled)').map(function () {
        return $(this).val();
    }).get();

    let nameValue = $(this).attr('id');

    $.ajax({
        url: '/Admin/ProductDetailListItem',
        type: 'GET',
        data: {
            opsValue: arrValue,
            nameValue: nameValue
        },
        traditional: true,

        success: function (data) {
            $('.display-detail').html(data);
            $('#displayAlertQty').css('display', 'none');
        },
        error: function (err) {
            alert('Lỗi:', err);
        }
    });
});

$(document).off('change', '#new-qty, .exist-qty, #newPrice').on('change', '#new-qty, .exist-qty, #newPrice', function () {

    let input = $(this);
    let quantity = input.val();

    if (quantity === "" || parseInt(quantity) < 1) {
        return input.val(1);
    }

    return input.val(quantity);
});

// loc ki tu
$(document).on('input paste', '#oldPrice, #newPrice, #qty, #new-qty', function () {

    const $input = $(this);
    let value = $input.val().replace(/[^0-9]/g, '');
    $input.val(value);
});

$('#picture').on('change', function () {

    let countPic = $('#picture').val();
    let typePic = countPic === '' ? 'inline' : 'none';
    $('#displayErrorPicture').css('display', typePic);
});

function CheckCountQty() {

    let countQty = parseInt($('.countQty').val());
    let type = countQty == 0 ? 'inline' : 'none';
    $('#displayAlertQty').css('display', type);
    $('#tempProductQty').css('display', type);

    if (type == 'none') return false;
}

$(window).on('pageshow', function () {


});
