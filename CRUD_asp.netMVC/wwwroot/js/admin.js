// Truyen du lieu san pham, ktra dkien them hoac sua
$(document).off('click', '#btnAdminProduct').on('click', '#btnAdminProduct', function (e) {

    e.preventDefault();

    const btn = $(this);

    // Lấy dữ liệu từ form
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
        formData.append('Picture', files[i]); // Tên 'Picture' phải khớp với tên thuộc tính trong formData
    }

    let nameAction = id ? 'Edit' : 'Create';

    $.ajax({
        url: id ? `/Admin/${nameAction}Product/${id}` : `/Admin/${nameAction}Product`,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {

            if (btn.prop('disabled')) return;
            btn.prop('disabled', true).text('Đang xử lý...');

            if (response.success) {
                setTimeout(() => {

                    window.location.href = '/Admin/Index';
                }, 1600);
            } else {

                btn.prop('disabled', false).text('Tạo');

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

    let keyQtyVal, keySizeVal, keyColorVal;
    let qtyVal, sizeVal, colorVal, oldSizeVal, oldColorVal;
    let row = $(btn).closest('.selectTemp');

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

            CheckCountQty()
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
$('.form-select.detail').on('click', function () {

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