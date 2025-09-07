document.addEventListener('DOMContentLoaded', function ()
{
    const btn_Dynamic = document.querySelectorAll('.btn_Dynamic');
    const mainMenu = document.querySelector('.nav_menud');
    function checkScrollButtons() {
        const listWidth = mainMenu.scrollWidth;
        const containerWidth = mainMenu.offsetWidth;

        if (listWidth <= containerWidth) {
            btn_Dynamic.forEach(btn => btn.style.display = 'none');
            mainMenu.style.margin = 'auto';

        } else {
            btn_Dynamic.forEach(btn => btn.style.display = 'block');
            mainMenu.style.margin = '';

        }
    }
    checkScrollButtons();
    window.addEventListener('resize', checkScrollButtons);



});
