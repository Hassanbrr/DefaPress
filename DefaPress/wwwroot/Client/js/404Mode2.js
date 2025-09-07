
    document.addEventListener('DOMContentLoaded', function() {
            // Mobile Menu Toggle
            const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
    const navMenu = document.querySelector('.nav-menu');

    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', function () {
            navMenu.classList.toggle('active');
            this.innerHTML = navMenu.classList.contains('active') ?
                '<i class="fas fa-times"></i>' :
                '<i class="fas fa-bars"></i>';
        });
            }

    // Update current date
    function updateDate() {
                const dateElement = document.querySelector('.date-display span');
    if (dateElement) {
                    const now = new Date();

    // Format date in Persian
    const options = {weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    const persianDate = new Intl.DateTimeFormat('fa-IR', options).format(now);

    dateElement.textContent = persianDate;
                }
            }

    updateDate();

    // Animation for error page elements
    function animateElements() {
                const errorCode = document.querySelector('.error-code');
    const errorTitle = document.querySelector('.error-title');
    const errorMessage = document.querySelector('.error-message');
    const errorActions = document.querySelector('.error-actions');
    const suggestedLinks = document.querySelector('.suggested-links');

                setTimeout(() => {
        errorCode.style.opacity = '1';
    errorCode.style.transform = 'translateY(0)';
                }, 300);

                setTimeout(() => {
        errorTitle.style.opacity = '1';
    errorTitle.style.transform = 'translateY(0)';
                }, 500);

                setTimeout(() => {
        errorMessage.style.opacity = '1';
    errorMessage.style.transform = 'translateY(0)';
                }, 700);

                setTimeout(() => {
        errorActions.style.opacity = '1';
    errorActions.style.transform = 'translateY(0)';
                }, 900);

                setTimeout(() => {
        suggestedLinks.style.opacity = '1';
    suggestedLinks.style.transform = 'translateY(0)';
                }, 1100);
            }

    // Add initial styles for animation
    const elementsToAnimate = [
    '.error-code',
    '.error-title',
    '.error-message',
    '.error-actions',
    '.suggested-links'
    ];

            elementsToAnimate.forEach(selector => {
                const elements = document.querySelectorAll(selector);
                elements.forEach(el => {
        el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    el.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                });
            });

    // Start animations
    animateElements();

    // Add hover effects to link cards
    const linkCards = document.querySelectorAll('.link-card');
            linkCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.querySelector('.link-icon').style.transform = 'scale(1.1) rotate(10deg)';
        });

    card.addEventListener('mouseleave', function() {
        this.querySelector('.link-icon').style.transform = 'scale(1) rotate(0deg)';
                });
            });
        });
