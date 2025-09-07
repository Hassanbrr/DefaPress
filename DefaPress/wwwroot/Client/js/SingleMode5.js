
        document.addEventListener("DOMContentLoaded", () => {
            // Mobile Menu Toggle
            const mobileMenuToggle = document.querySelector(".mobile-menu-toggle");
    const navMenu = document.querySelector(".nav-menu");

    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener("click", () => {
            navMenu.classList.toggle("active");
            document.body.classList.toggle("menu-open");
        });
            }

    // Back to Top Button
    const backToTopButton = document.getElementById("back-to-top");

            window.addEventListener("scroll", () => {
                if (window.pageYOffset > 300) {
        backToTopButton.classList.add("visible");
                } else {
        backToTopButton.classList.remove("visible");
                }
            });

            backToTopButton.addEventListener("click", () => {
        window.scrollTo({
            top: 0,
            behavior: "smooth",
        });
            });

    // Update current date
    function updateDate() {
                const dateElement = document.querySelector(".date-display span");
    if (dateElement) {
                    const now = new Date();

    // Format date in Persian
    const options = {weekday: "long", year: "numeric", month: "long", day: "numeric" };
    const persianDate = new Intl.DateTimeFormat("fa-IR", options).format(now);

    dateElement.textContent = persianDate;
                }
            }

    updateDate();

    // Comment Form Submission (simulated)
    const commentForm = document.querySelector(".comment-form form");
    if (commentForm) {
        commentForm.addEventListener("submit", function (e) {
            e.preventDefault();
            alert("نظر شما با موفقیت ثبت شد و پس از تایید نمایش داده خواهد شد.");
            this.reset();
        });
            }

    // Share Links (simulated)
    const shareLinks = document.querySelectorAll(".share-link");
            shareLinks.forEach(link => {
        link.addEventListener("click", function (e) {
            e.preventDefault();
            alert("در یک پیاده‌سازی واقعی، این لینک مقاله را در شبکه اجتماعی مربوطه به اشتراک می‌گذارد.");
        });
            });

    // Newsletter Form Submission (simulated)
    const newsletterForms = document.querySelectorAll(".newsletter-form, .subscribe-form form");
            newsletterForms.forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();
            alert("ایمیل شما با موفقیت در خبرنامه ثبت شد.");
            this.reset();
        });
            });

    // Add animation to article images on scroll
    const articleImages = document.querySelectorAll(".article-image img");

            const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = "1";
                entry.target.style.transform = "translateY(0)";
            }
        });
            }, {threshold: 0.1 });

            articleImages.forEach(image => {
        image.style.opacity = "0";
    image.style.transform = "translateY(20px)";
    image.style.transition = "opacity 0.5s ease, transform 0.5s ease";
    observer.observe(image);
            });
        });
