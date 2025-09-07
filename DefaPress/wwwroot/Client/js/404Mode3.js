
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

    // Generate stars
    function generateStars() {
                const starsContainer = document.getElementById('stars');
    const starsCount = 200;

    for (let i = 0; i < starsCount; i++) {
                    const star = document.createElement('div');
    star.classList.add('star');

    // Random position
    const posX = Math.random() * 100;
    const posY = Math.random() * 100;

    // Random size
    const size = Math.random() * 3;

    // Random opacity
    const opacity = Math.random() * 0.8 + 0.2;

    // Random twinkle duration
    const twinkleDuration = Math.random() * 5 + 3;

    star.style.left = `${posX}%`;
    star.style.top = `${posY}%`;
    star.style.width = `${size}px`;
    star.style.height = `${size}px`;
    star.style.setProperty('--star-opacity', opacity);
    star.style.setProperty('--twinkle-duration', `${twinkleDuration}s`);

    starsContainer.appendChild(star);
                }
            }

    // Generate shooting stars
    function generateShootingStars() {
                const shootingStarsContainer = document.getElementById('shooting-stars');
    const shootingStarsCount = 5;

    for (let i = 0; i < shootingStarsCount; i++) {
                    const shootingStar = document.createElement('div');
    shootingStar.classList.add('shooting-star');

    // Random position
    const posX = Math.random() * 100;
    const posY = Math.random() * 100;

    // Random angle
    const angle = Math.random() * 60 - 30;

    // Random distance
    const distance = Math.random() * 200 + 100;

    // Random duration
    const duration = Math.random() * 2 + 1;

    // Random delay
    const delay = Math.random() * 15;

    shootingStar.style.top = `${posY}%`;
    shootingStar.style.right = `${posX}%`;
    shootingStar.style.setProperty('--shooting-angle', `${angle}deg`);
    shootingStar.style.setProperty('--shooting-distance', `${distance}px`);
    shootingStar.style.setProperty('--shooting-duration', `${duration}s`);
    shootingStar.style.setProperty('--shooting-delay', `${delay}s`);

    shootingStarsContainer.appendChild(shootingStar);
                }
            }

    // Initialize space elements
    generateStars();
    generateShootingStars();

    // Interactive mouse movement effect
    const spaceErrorSection = document.querySelector('.space-error-section');
    const ufoContainer = document.querySelector('.ufo-container');
    const astronautContainer = document.querySelector('.astronaut-container');

    spaceErrorSection.addEventListener('mousemove', function(e) {
                const x = e.clientX / window.innerWidth;
    const y = e.clientY / window.innerHeight;

    // Move UFO slightly with mouse
    if (ufoContainer) {
        ufoContainer.style.transform = `translate(${(x - 0.5) * 20}px, ${(y - 0.5) * 10}px)`;
                }

    // Move astronaut slightly with mouse in opposite direction
    if (astronautContainer) {
        astronautContainer.style.transform = `translate(${(0.5 - x) * 30}px, ${(0.5 - y) * 20}px) rotate(${(x - 0.5) * 10}deg)`;
                }
            });

    // Add hover effects to space link cards
    const spaceLinkCards = document.querySelectorAll('.space-link-card');
            spaceLinkCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.querySelector('.space-link-icon').style.transform = 'scale(1.1) rotate(10deg)';
        });

    card.addEventListener('mouseleave', function() {
        this.querySelector('.space-link-icon').style.transform = 'scale(1) rotate(0deg)';
                });
            });

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
        });
