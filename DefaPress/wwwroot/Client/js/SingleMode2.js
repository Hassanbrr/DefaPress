
        document.addEventListener('DOMContentLoaded', function() {
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

        // Carousel navigation
        const prevButtons = document.querySelectorAll('.prev-btn');
        const nextButtons = document.querySelectorAll('.next-btn');

      prevButtons.forEach(button => {
            button.addEventListener('click', function () {
                const target = this.getAttribute('data-target');
                const carousel = document.getElementById(target);
                carousel.scrollBy({ left: -300, behavior: 'smooth' });
            });
      });

      nextButtons.forEach(button => {
            button.addEventListener('click', function () {
                const target = this.getAttribute('data-target');
                const carousel = document.getElementById(target);
                carousel.scrollBy({ left: 300, behavior: 'smooth' });
            });
      });
    });
    