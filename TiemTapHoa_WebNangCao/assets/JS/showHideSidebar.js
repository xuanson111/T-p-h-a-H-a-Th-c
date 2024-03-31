// property
let content = document.querySelector('#content');
let sidebarItems = document.querySelectorAll('#sidebar .sidebar__item a');
let sidebarToggle = document.querySelector('.sidebar-toggle');
let sidebar = document.querySelector('#sidebar');
let showCateBtn = document.querySelector('.sidebar-toggle .show');
let hideCateBtn = document.querySelector('.sidebar-toggle .hide');
let deck = document.querySelector('.deck');
let btnShowHide = document.querySelector('.btnShowHide');

// function
function handleShowHideCate(show) {
    if (show) {
        deck.style.display = 'block';
        sidebar.style.transform = 'translateX(0)';
        showCateBtn.style.display = 'none';
        hideCateBtn.style.display = 'block';
    } else {
        deck.style.display = 'none';
        sidebar.style.transform = 'translateX(-100%)';
        hideCateBtn.style.display = 'none';
        showCateBtn.style.display = 'block';
    }
}

// event
// xét các thuộc tính (inline) cho sidebar

// xử lý sự kiện khi chọn các nút trong sidebar
sidebarItems.forEach((item) => {
    item.onclick = () => {
        sidebarItems.forEach((it) => {
            it.classList.remove('active');
        });
        item.classList.add('active');
    }
});

// Trên màn hình ipad, điện thoại
// sử lý ẩn sidebar và lớp phủ khi click vào lớp phủ
deck.onclick = () => {
    handleShowHideCate(false)
}

// sử lý hiện sidebar và lớp phủ khi click vào showCateBtn
showCateBtn.onclick = () => {
    handleShowHideCate(true);
}

// sử lý ẩn sidebar và lớp phủ khi click vào hideCateBtn
hideCateBtn.onclick = () => {
    handleShowHideCate(false);
}

// xét lại các thuộc tính cho sidebar khi kích thước màn hình thay đổi
window.addEventListener('resize', (e) => {
    if (e.target.innerWidth > 1013) {
        deck.style.display = 'none';
        sidebar.style.transform = 'translateX(0)';
        sidebar.style.position = 'static';
        content.style.padding = '20px 40px 20px 20px';
    } else {
        handleShowHideCate(false);
        sidebar.style.position = 'fixed';
        content.style.padding = '20px';
    }
})

// sử lý ẩn hiện sidebar trên màn hình (width > 1013px)
btnShowHide.onclick = () => {
    if (window.getComputedStyle(sidebar).getPropertyValue("position") === 'static') {
        content.style.padding = '20px';
        sidebar.style.position = 'fixed';
        sidebar.style.transform = 'translateX(-100%)';
    } else {
        content.style.padding = '20px 40px 20px 20px';
        sidebar.style.position = 'static';
        sidebar.style.transform = 'translateX(0)';
    }
}


