
    document.addEventListener('DOMContentLoaded', function () {
            // Tab Switching
            const tabs = document.querySelectorAll('.login-tab');
    const forms = document.querySelectorAll('.login-form');

            tabs.forEach(tab => {
        tab.addEventListener('click', function () {
            const tabId = this.getAttribute('data-tab');

            // Remove active class from all tabs and forms
            tabs.forEach(t => t.classList.remove('active'));
            forms.forEach(f => f.classList.remove('active'));

            // Add active class to current tab and form
            this.classList.add('active');
            document.getElementById(`${tabId}-form`).classList.add('active');
        });
            });

    // Back to Login Button
    const backToLoginBtn = document.querySelector('.back-to-login');
    if (backToLoginBtn) {
        backToLoginBtn.addEventListener('click', function () {
            tabs.forEach(t => t.classList.remove('active'));
            forms.forEach(f => f.classList.remove('active'));

            document.querySelector('[data-tab="login"]').classList.add('active');
            document.getElementById('login-form').classList.add('active');
        });
            }

    // Password Strength Meter
    const passwordInput = document.getElementById('register-password');
    const strengthMeter = document.querySelector('.password-strength');
    const strengthText = document.querySelector('.strength-text');

    if (passwordInput) {
        passwordInput.addEventListener('input', function () {
            const password = this.value;
            let strength = 0;

            // Calculate password strength
            if (password.length >= 8) strength += 1;
            if (password.match(/[a-z]/) && password.match(/[A-Z]/)) strength += 1;
            if (password.match(/\d/)) strength += 1;
            if (password.match(/[^a-zA-Z\d]/)) strength += 1;

            // Update strength meter
            strengthMeter.className = 'password-strength';

            if (password.length === 0) {
                strengthMeter.classList.add('strength-weak');
                strengthText.textContent = 'قدرت رمز عبور: ضعیف';
            } else if (strength < 2) {
                strengthMeter.classList.add('strength-weak');
                strengthText.textContent = 'قدرت رمز عبور: ضعیف';
            } else if (strength === 2) {
                strengthMeter.classList.add('strength-medium');
                strengthText.textContent = 'قدرت رمز عبور: متوسط';
            } else if (strength === 3) {
                strengthMeter.classList.add('strength-strong');
                strengthText.textContent = 'قدرت رمز عبور: قوی';
            } else {
                strengthMeter.classList.add('strength-very-strong');
                strengthText.textContent = 'قدرت رمز عبور: بسیار قوی';
            }
        });
            }

    // Update current date
    function updateDate() {
                const dateElement = document.querySelector('.header-date');
    if (dateElement) {
                    const now = new Date();

    // Format date in Persian
    const options = {weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    const persianDate = new Intl.DateTimeFormat('fa-IR', options).format(now);

    dateElement.textContent = persianDate;
                }
            }

    updateDate();
        });













document.addEventListener('DOMContentLoaded', function () {
    const btn_Dynamic = document.querySelectorAll('.btn_Dynamic');
    const mainMenu = document.querySelector('.nav_menud');
    function checkScrollButtons() {
        const listWidth = mainMenu.scrollWidth;
        const containerWidth = mainMenu.offsetWidth;

        if (listWidth <= containerWidth) {
            btn_Dynamic.forEach(btn => btn.style.display = 'none');
            mainMenu.style.margin = '15px auto 30px auto';

        } else {
            btn_Dynamic.forEach(btn => btn.style.display = 'block');
            mainMenu.style.margin = '';

        }
    }
    checkScrollButtons();
    window.addEventListener('resize', checkScrollButtons);



});