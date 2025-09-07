
        document.addEventListener('DOMContentLoaded', function() {
      // Tab switching functionality
      const tabs = document.querySelectorAll('.login-tab');
        const forms = document.querySelectorAll('.login-form');

      tabs.forEach(tab => {
            tab.addEventListener('click', () => {
                const tabId = tab.getAttribute('data-tab');

                // Remove active class from all tabs and forms
                tabs.forEach(t => t.classList.remove('active'));
                forms.forEach(f => f.classList.remove('active'));

                // Add active class to current tab and form
                tab.classList.add('active');
                document.getElementById(`${tabId}-form`).classList.add('active');
            });
      });

        // Back to login button
        const backToLoginBtn = document.querySelector('.back-to-login');
        if (backToLoginBtn) {
            backToLoginBtn.addEventListener('click', () => {
                tabs.forEach(t => t.classList.remove('active'));
                forms.forEach(f => f.classList.remove('active'));

                document.querySelector('[data-tab="login"]').classList.add('active');
                document.getElementById('login-form').classList.add('active');
            });
      }

        // Password strength meter
        const passwordInput = document.getElementById('register-password');
        const strengthMeter = document.querySelector('.password-strength');

        if (passwordInput && strengthMeter) {
            passwordInput.addEventListener('input', () => {
                const password = passwordInput.value;
                let strength = 0;

                // Check password length
                if (password.length >= 8) {
                    strength += 1;
                }

                // Check for mixed case
                if (password.match(/[a-z]/) && password.match(/[A-Z]/)) {
                    strength += 1;
                }

                // Check for numbers
                if (password.match(/\d/)) {
                    strength += 1;
                }

                // Check for special characters
                if (password.match(/[^a-zA-Z\d]/)) {
                    strength += 1;
                }

                // Update strength meter
                strengthMeter.className = 'password-strength';

                if (password.length === 0) {
                    strengthMeter.classList.add('strength-weak');
                    strengthMeter.querySelector('.strength-text').textContent = 'قدرت رمز عبور: ضعیف';
                } else if (strength === 1) {
                    strengthMeter.classList.add('strength-weak');
                    strengthMeter.querySelector('.strength-text').textContent = 'قدرت رمز عبور: ضعیف';
                } else if (strength === 2) {
                    strengthMeter.classList.add('strength-medium');
                    strengthMeter.querySelector('.strength-text').textContent = 'قدرت رمز عبور: متوسط';
                } else if (strength === 3) {
                    strengthMeter.classList.add('strength-strong');
                    strengthMeter.querySelector('.strength-text').textContent = 'قدرت رمز عبور: قوی';
                } else if (strength === 4) {
                    strengthMeter.classList.add('strength-very-strong');
                    strengthMeter.querySelector('.strength-text').textContent = 'قدرت رمز عبور: بسیار قوی';
                }
            });
      }

        // Form validation
        const loginForm = document.getElementById('login-form');
        const registerForm = document.getElementById('register-form');
        const forgotForm = document.getElementById('forgot-form');

        if (loginForm) {
            loginForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const email = document.getElementById('login-email').value;
                const password = document.getElementById('login-password').value;

                let isValid = true;

                if (!email) {
                    document.getElementById('login-email').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('login-email').classList.remove('is-invalid');
                }

                if (!password) {
                    document.getElementById('login-password').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('login-password').classList.remove('is-invalid');
                }

                if (isValid) {
                    // Submit form or redirect
                    alert('ورود با موفقیت انجام شد!');
                    window.location.href = 'index.html';
                }
            });
      }

        if (registerForm) {
            registerForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const firstName = document.getElementById('register-firstname').value;
                const lastName = document.getElementById('register-lastname').value;
                const email = document.getElementById('register-email').value;
                const password = document.getElementById('register-password').value;
                const confirmPassword = document.getElementById('register-confirm-password').value;
                const terms = document.getElementById('terms').checked;

                let isValid = true;

                if (!firstName) {
                    document.getElementById('register-firstname').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('register-firstname').classList.remove('is-invalid');
                }

                if (!lastName) {
                    document.getElementById('register-lastname').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('register-lastname').classList.remove('is-invalid');
                }

                if (!email || !email.includes('@')) {
                    document.getElementById('register-email').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('register-email').classList.remove('is-invalid');
                }

                if (!password || password.length < 8) {
                    document.getElementById('register-password').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('register-password').classList.remove('is-invalid');
                }

                if (!confirmPassword || confirmPassword !== password) {
                    document.getElementById('register-confirm-password').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('register-confirm-password').classList.remove('is-invalid');
                }

                if (!terms) {
                    document.getElementById('terms').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('terms').classList.remove('is-invalid');
                }

                if (isValid) {
                    // Submit form or redirect
                    alert('ثبت‌نام با موفقیت انجام شد!');
                    window.location.href = 'index.html';
                }
            });
      }

        if (forgotForm) {
            forgotForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const email = document.getElementById('forgot-email').value;

                let isValid = true;

                if (!email || !email.includes('@')) {
                    document.getElementById('forgot-email').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('forgot-email').classList.remove('is-invalid');
                }

                if (isValid) {
                    // Submit form or show success message
                    alert('لینک بازیابی رمز عبور به ایمیل شما ارسال شد.');
                    // Reset form
                    forgotForm.reset();
                    // Switch back to login tab
                    document.querySelector('[data-tab="login"]').click();
                }
            });
      }

        // Update current date
        const persianDate = new Intl.DateTimeFormat('fa-IR', {
            weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      }).format(new Date());

        const dateElement = document.querySelector('.header-date');
        if (dateElement) {
            dateElement.textContent = persianDate;
      }
    });














document.addEventListener('DOMContentLoaded', function () {
    const btn_Dynamic = document.querySelectorAll('.btn_Dynamic');
    const mainMenu = document.querySelector('.nav_menud');
    function checkScrollButtons() {
        const listWidth = mainMenu.scrollWidth;
        const containerWidth = mainMenu.offsetWidth;

        if (listWidth <= containerWidth) {
            btn_Dynamic.forEach(btn => btn.style.display = 'none');
            mainMenu.style.margin = '20px auto 25px auto';
           
        } else {
            btn_Dynamic.forEach(btn => btn.style.display = 'block');
            mainMenu.style.margin = '';

        }
    }
    checkScrollButtons();
    window.addEventListener('resize', checkScrollButtons);



});