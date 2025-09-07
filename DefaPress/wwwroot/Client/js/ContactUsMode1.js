
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

        // FAQ Accordion
        const faqItems = document.querySelectorAll('.faq-item');

      faqItems.forEach(item => {
        const question = item.querySelector('.faq-question');

        question.addEventListener('click', () => {
            // Close all other items
            faqItems.forEach(otherItem => {
                if (otherItem !== item) {
                    otherItem.classList.remove('active');
                }
            });

        // Toggle current item
        item.classList.toggle('active');
        });
      });

        // Form validation
        const contactForm = document.getElementById('contact-form');
        const formSuccess = document.getElementById('form-success');
        const formError = document.getElementById('form-error');

        if (contactForm) {
            contactForm.addEventListener('submit', function (e) {
                e.preventDefault();

                // Reset form state
                formSuccess.style.display = 'none';
                formError.style.display = 'none';

                const name = document.getElementById('name').value;
                const email = document.getElementById('email').value;
                const subject = document.getElementById('subject').value;
                const message = document.getElementById('message').value;

                let isValid = true;

                // Validate name
                if (!name.trim()) {
                    document.getElementById('name').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('name').classList.remove('is-invalid');
                }

                // Validate email
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!email.trim() || !emailRegex.test(email)) {
                    document.getElementById('email').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('email').classList.remove('is-invalid');
                }

                // Validate subject
                if (!subject) {
                    document.getElementById('subject').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('subject').classList.remove('is-invalid');
                }

                // Validate message
                if (!message.trim()) {
                    document.getElementById('message').classList.add('is-invalid');
                    isValid = false;
                } else {
                    document.getElementById('message').classList.remove('is-invalid');
                }

                if (isValid) {
                    // Simulate form submission
                    setTimeout(() => {
                        formSuccess.style.display = 'block';
                        contactForm.reset();

                        // Hide success message after 5 seconds
                        setTimeout(() => {
                            formSuccess.style.display = 'none';
                        }, 5000);
                    }, 1000);
                } else {
                    formError.style.display = 'block';

                    // Hide error message after 5 seconds
                    setTimeout(() => {
                        formError.style.display = 'none';
                    }, 5000);
                }
            });
      }
    });
