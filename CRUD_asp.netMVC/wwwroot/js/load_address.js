$(document).ready(function () {

    const $provinceEl = $("#province");
    const $wardEl = $("#ward");
    const $fullLocationEl = $("#fullLocation");
    const $streetEl = $("#street");

    console.log("TomSelect không tải.");

    let data = [];
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
            data = json;
            const options = data.map(p => ({ value: p.Code, text: p.Name }));
            provinceSelect.addOptions(options);
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
            const province = data.find(p => p.Code === provinceCode) || { Wards: [] };
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
});