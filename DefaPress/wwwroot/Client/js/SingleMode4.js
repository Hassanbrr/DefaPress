
    document.addEventListener('DOMContentLoaded', function() {
            // Mobile Menu Toggle
            const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
    const navMenu = document.querySelector('.nav-menu');

    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', function () {
            navMenu.classList.toggle('active');
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

    // Comment Form Submission (simulated)
    const commentForm = document.querySelector('.comment-form form');
    if (commentForm) {
        commentForm.addEventListener('submit', function (e) {
            e.preventDefault();
            alert('نظر شما با موفقیت ثبت شد و پس از تایید نمایش داده خواهد شد.');
            this.reset();
        });
            }

    // Share Links (simulated)
    const shareLinks = document.querySelectorAll('.share-link');
            shareLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            alert('در یک پیاده‌سازی واقعی، این لینک مقاله را در شبکه اجتماعی مربوطه به اشتراک می‌گذارد.');
        });
            });
        });
