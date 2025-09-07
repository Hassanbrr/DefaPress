document.addEventListener('DOMContentLoaded', function () {
    const menuItems = document.querySelectorAll('.has-submenu');

    menuItems.forEach(item => {
        item.addEventListener('click', (event) => {
            event.stopPropagation();

            // بررسی اینکه آیا زیرمنو باز است یا خیر
            const submenu = item.querySelector('.submenu');
            const submenuOrginal = item.querySelector('.submenuOrginal');

            if (item.classList.contains('active')) {
                item.classList.remove('active');
                submenu.style.display = 'none';
                submenuOrginal.style.display = 'none';
            } else {
                // بستن سایر زیرمنوها قبل از باز کردن جدید
                document.querySelectorAll('.has-submenu').forEach(menuItem => {
                    menuItem.classList.remove('active');
                    menuItem.querySelector('.submenu').style.display = 'none';
                    menuItem.querySelector('.submenuOrginal').style.display = 'none';
                });

                item.classList.add('active');
                submenu.style.display = 'grid';
                submenuOrginal.style.display = 'flex';
            }
        });
    });

    // بستن منو هنگام کلیک خارج از آن
    document.addEventListener('click', () => {
        document.querySelectorAll('.has-submenu').forEach(item => {
            item.classList.remove('active');
            item.querySelector('.submenu').style.display = 'none';
            item.querySelector('.submenuOrginal').style.display = 'none';
        });
    });



    
});





//window.addEventListener('scroll', function () {
//    const scrollTop = window.scrollY;

//    document.querySelectorAll('.submenuOrginal').forEach(element => {
//        if (scrollTop > 80) {
//            element.classList.add('hidden');
//        } else if (scrollTop <= 170) {
//            element.classList.remove('hidden');
//        }
//    });
//});













document.addEventListener('DOMContentLoaded', function () {
    const menuItems = document.querySelectorAll('.has-submenu');
    const submenuItems = document.querySelectorAll('.submenuOrginal');
    const sss = document.querySelectorAll('.Nav_subset');




    // تابع برای نمایش ساب‌منو
    function showSubmenu(item) {
        item.querySelector('.submenu').style.display = 'grid';
        item.querySelector('.submenuOrginal').style.display = 'flex';
    }

    // تابع برای مخفی کردن ساب‌منو
    function hideSubmenu(item) {
        item.querySelector('.submenu').style.display = 'none';
        item.querySelector('.submenuOrginal').style.display = 'none';
    }

    // مدیریت رویدادها برای هر آیتم منو
    menuItems.forEach(item => {
        // رویداد mouseenter
        item.addEventListener('mouseenter', () => {
            showSubmenu(item);
        });

        //// رویداد focus (برای کیبورد)
        //item.addEventListener('focus', () => {
        //    showSubmenu(item);
        //});

        // رویداد mouseleave
        item.addEventListener('mouseleave', () => {
            hideSubmenu(item);
        });

        // رویداد blur (برای کیبورد)
        item.addEventListener('blur', () => {
            hideSubmenu(item);
        });
    });

    //// بستن منو هنگام کلیک خارج از آن
    //document.addEventListener('click', (event) => {
    //    menuItems.forEach(item => {
    //        if (!item.contains(event.target)) {
    //            hideSubmenu(item);
    //        }
    //    });
    //});
});





document.addEventListener('DOMContentLoaded', function () {
    const navItems = document.querySelectorAll('.NavSide');

    navItems.forEach(item => {
        item.addEventListener('click', function () {
            const subMenu = this.nextElementSibling; // پیدا کردن زیرمنوی مرتبط

            // بستن تمام زیرمنوهای باز قبل از باز کردن جدید
            document.querySelectorAll('.Nav_subset').forEach(menu => {
                if (menu !== subMenu) { // فقط زیرمنوی قبلی را ببندیم، نه مورد فعلی
                    menu.style.display = 'none';
                }
            });

            // باز و بسته کردن زیرمنوی مورد نظر
            if (subMenu && subMenu.classList.contains('Nav_subset')) {
                subMenu.style.display = (subMenu.style.display === 'block') ? 'none' : 'block';
            }
        });
    });
});
