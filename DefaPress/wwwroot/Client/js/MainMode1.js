
        document.addEventListener('DOMContentLoaded', function() {
      // Slider functionality
      const slides = document.querySelectorAll('.slide');
        const dots = document.querySelectorAll('.slider-dot');
        const prevBtn = document.querySelector('.slider-prev');
        const nextBtn = document.querySelector('.slider-next');
        let currentSlide = 0;
        let slideInterval;

        // Initialize slider
        function startSlider() {
            slideInterval = setInterval(nextSlide,4500); // Change slide every 3 seconds
      }

        // Stop slider on interaction
        function stopSlider() {
            clearInterval(slideInterval);
      }

        // Show specific slide
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
        currentSlide = n;
      }

        // Next slide function
        function nextSlide() {
            currentSlide = (currentSlide + 1) % slides.length;
        showSlide(currentSlide);
      }

        // Previous slide function
        function prevSlide() {
            currentSlide = (currentSlide - 1 + slides.length) % slides.length;
        showSlide(currentSlide);
      }

      // Event listeners for dots
      dots.forEach((dot, index) => {
            dot.addEventListener('click', () => {
                stopSlider();
                showSlide(index);
                startSlider();
            });
      });

      // Event listeners for arrows
      prevBtn.addEventListener('click', () => {
            stopSlider();
        prevSlide();
        startSlider();
      });

      nextBtn.addEventListener('click', () => {
            stopSlider();
        nextSlide();
        startSlider();
      });

        // Pause slider on hover
        const sliderContainer = document.querySelector('.slider-container');
        sliderContainer.addEventListener('mouseenter', stopSlider);
        sliderContainer.addEventListener('mouseleave', startSlider);

        // Start the slider
        startSlider();

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

function scrollList(distance) {
    const list = document.getElementById('scrollable_Toplist');
    list.scrollBy({ left: distance, behavior: 'smooth' });
}
    
