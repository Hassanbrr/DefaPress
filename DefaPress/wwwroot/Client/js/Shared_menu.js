document.addEventListener('DOMContentLoaded', function () {
    const TitleMegaMeno = document.querySelectorAll('.has_MegaMeno');

    TitleMegaMeno.forEach(dropdown => {
        dropdown.addEventListener('mouseover', () => {
            dropdown.querySelector('.DMegaSubmenu').style.display = 'block';
        });

        dropdown.addEventListener('mouseout', () => {
            dropdown.querySelector('.DMegaSubmenu').style.display = 'none';
        });
    });


});






document.addEventListener('DOMContentLoaded', function () {
    const widthDynamicElement = document.querySelector('.WidthDynamic');
    const btnDynamicElements = document.querySelectorAll('.btn_Dynamic');
    const nav_container = document.querySelector('.nav-container');

    function checkElementWidth() {
        if (WidthDynamic >= 1100) {

            btnDynamicElements.forEach(btn => {
                btn.style.display = 'block';
            });
        } else {

            btnDynamicElements.forEach(btn => {
                btn.style.display = 'none';
            });
        }
    }
    function checkElementWidth2() {
        if (WidthDynamic <= 768) {

            btnDynamicElements.forEach(btn => {
                btn.style.display = 'none';
            });
        } else {

            btnDynamicElements.forEach(btn => {
                btn.style.display = 'block';
            });
        }
    }

    checkElementWidth();
    checkElementWidth2();


    window.addEventListener('resize', checkElementWidth);
    window.addEventListener('resize', checkElementWidth2);
});