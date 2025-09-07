
    document.addEventListener('DOMContentLoaded', function() {
            // Mobile Menu Toggle
            const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
    const navMenu = document.querySelector('.nav-menu');

    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', function () {
            navMenu.classList.toggle('active');
        });
            }

    // Hero Slider
    const slides = document.querySelectorAll('.slide');
    const dots = document.querySelectorAll('.slider-dot');
    const prevBtn = document.querySelector('.slider-prev');
    const nextBtn = document.querySelector('.slider-next');
    let currentSlide = 0;
    let slideInterval;

    function showSlide(n) {
        // Hide all slides
        slides.forEach(slide => {
            slide.classList.remove('active');
        });

                // Remove active class from all dots
                dots.forEach(dot => {
        dot.classList.remove('active');
                });

    // Show current slide and activate dot
    slides[n].classList.add('active');
    dots[n].classList.add('active');

    // Update current slide index
    currentSlide = n;
            }

    function nextSlide() {
        showSlide((currentSlide + 1) % slides.length);
            }

    function prevSlide() {
        showSlide((currentSlide - 1 + slides.length) % slides.length);
            }

    // Start automatic slideshow
    function startSlideshow() {
        slideInterval = setInterval(nextSlide, 5000);
            }

    // Stop slideshow
    function stopSlideshow() {
        clearInterval(slideInterval);
            }

            // Initialize slider
            if (slides.length > 0) {
        // Add event listeners to dots
        dots.forEach((dot, index) => {
            dot.addEventListener('click', () => {
                showSlide(index);
                stopSlideshow();
                startSlideshow();
            });
        });

    // Add event listeners to arrows
    if (prevBtn && nextBtn) {
        prevBtn.addEventListener('click', () => {
            prevSlide();
            stopSlideshow();
            startSlideshow();
        });

                    nextBtn.addEventListener('click', () => {
        nextSlide();
    stopSlideshow();
    startSlideshow();
                    });
                }

    // Start slideshow
    startSlideshow();
            }

    // Category Tabs
    const tabItems = document.querySelectorAll('.category-tab');
    const tabPanes = document.querySelectorAll('.category-content');

            tabItems.forEach(item => {
        item.addEventListener('click', function () {
            // Remove active class from all tabs
            tabItems.forEach(tab => {
                tab.classList.remove('active');
            });

            // Hide all tab panes
            tabPanes.forEach(pane => {
                pane.classList.remove('active');
            });

            // Add active class to clicked tab
            this.classList.add('active');

            // Show corresponding tab pane
            const tabId = this.getAttribute('data-tab');
            document.getElementById(tabId).classList.add('active');
        });
            });

    // Load tab content dynamically (simulated)
  

    loadTabContent();

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

    // Video Play Button (simulated)
    const videoThumbnails = document.querySelectorAll('.video-thumbnail, .video-item-thumbnail');

            videoThumbnails.forEach(thumbnail => {
        thumbnail.addEventListener('click', function () {
            // In a real implementation, this would open a video player
            alert('در یک پیاده‌سازی واقعی، اینجا ویدیو پخش می‌شود.');
        });
            });
        });



function scrollList(distance) {
    const list = document.getElementById('btn-scroll-header');
    list.scrollBy({ left: distance, behavior: 'smooth' });
}