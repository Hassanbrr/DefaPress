
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

    // Back to top button
    const backToTopBtn = document.querySelector('.back-to-top');

    window.addEventListener('scroll', function() {
        if (window.pageYOffset > 300) {
        backToTopBtn.classList.add('active');
        } else {
        backToTopBtn.classList.remove('active');
        }
      });

    backToTopBtn.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
      });

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

    // Add animation to category cards
    const categoryCards = document.querySelectorAll('.category-card');

      categoryCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.querySelector('.category-card-icon').style.transform = 'scale(1.2)';
        });

    card.addEventListener('mouseleave', function() {
        this.querySelector('.category-card-icon').style.transform = 'scale(1)';
        });
      });

    // Animate on scroll (simple implementation)
    function animateOnScroll() {
        const elements = document.querySelectorAll('.article-card, .popular-card, .category-card, .featured-article-content, .featured-article-image');

        elements.forEach(element => {
          const elementTop = element.getBoundingClientRect().top;
    const elementVisible = 150;

    if (elementTop < window.innerHeight - elementVisible) {
        element.classList.add('animate');
          }
        });
      }

    window.addEventListener('scroll', animateOnScroll);
    animateOnScroll(); // Run once on page load
    });




function scrollList(distance) {
    const list = document.getElementById('scrollable_Toplist');
    list.scrollBy({ left: distance, behavior: 'smooth' });
}