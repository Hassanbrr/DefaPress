
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
    });
