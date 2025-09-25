

// Xu ly logic lazy load san pham da mua //
let offset = 0;
let limit = 5;
let isLoading = false;

function initLazyLoad(initOffsetLoad) {
    offset = initOffsetLoad;

    $(window).on("scroll", function () {
        if (isLoading) return;

        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadMoreOrders();
        }
    });
}
function loadMoreOrders() {

    isLoading = true;
    $("#loading").fadeIn(400);

    setTimeout(() => {
        $("#loading").hide();

        $.ajax({
            url: "/Home/LoadMoreOrders",
            type: "GET",
            data: {
                offset: offset,
                limit: limit,
            },
            success: function (data) {
                if (data.trim().length > 0) {
                    $("#orderContainer").append(data);
                    offset += limit;
                } else {
                    $(window).off("scroll"); // hết dữ liệu
                }
            },
            error: function () {
                alert("Có lỗi xảy ra khi tải dữ liệu!");
            },
            complete: function () {
                isLoading = false; // reset cờ
                $("#loading").hide(); // đảm bảo spinner tắt
            },
        });
    }, 2000); // 2s
}
// Xu ly logic lazy load san pham da mua //

//logic theo doi tien trinh cap nhat don hang
const $timeline = $("#tracking-timeline");
const $circles = $timeline.find(".circle");

function updateProgress() {
    setTimeout(() => {
        let activeIndex = -1;

        $circles.each(function (index) {
            if ($(this).hasClass("active")) {
                activeIndex = index;
                return false;
            }
        });

        if (activeIndex === -1) {
            const lastDone = $circles.filter(".done").last();
            activeIndex = lastDone.length ? lastDone.index() : 0;
        }

        const totalSteps = $circles.length;
        if (totalSteps <= 1) {
            $timeline.css("--track-progress", "0px");
            return;
        }

        if ($(window).width() > 768) {
            const lineWidth = $timeline.width() - 140;
            const progress = (activeIndex / (totalSteps - 1)) * lineWidth;
            $timeline.css("--track-progress", Math.max(0, progress) + "px");
        } else {
            const lineHeight = $timeline.height() - 60;
            const progress = (activeIndex / (totalSteps - 1)) * lineHeight;
            $timeline.css("--track-progress", Math.max(0, progress) + "px");
        }
    }, 100);
}

updateProgress();
$(window).on("resize", function () {
    updateProgress();
});