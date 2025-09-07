
        document.addEventListener('DOMContentLoaded', function () {
         
            const mobileMenuButton = document.querySelector('.mobile-menu-button');
            const mobileMenuClose = document.querySelector('.mobile-menu-close');
            const mobileMenu = document.querySelector('.mobile-menu');
            const mobileMenuOverlay = document.querySelector('.mobile-menu-overlay');

        
            const scrollLeftButton = document.querySelector('.scroll_left');
            
            const scrollRightButton = document.querySelector('.scroll_right');
            const mainMenu = document.querySelector('.nav_menud');
            const DMegaSubmenu = document.querySelectorAll('.DMegaSubmenu');
          

         
            

       
        function closeMobileMenu() {
            mobileMenu.classList.remove('active');
            mobileMenuOverlay.classList.remove('active');
            document.body.style.overflow = ''; 
        }

           
           

          
            updateMegaSubmenuPosition();


            mainMenu.addEventListener('scroll', updateMegaSubmenuPosition);

            let scrollAmount = 200;

            scrollRightButton.addEventListener('click', function () {
                mainMenu.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
                updateMegaSubmenuPosition();
            });
            
            scrollLeftButton.addEventListener('click', function () {
                mainMenu.scrollBy({ left: scrollAmount, behavior: 'smooth' });
                updateMegaSubmenuPosition();
            });

            function updateMegaSubmenuPosition() {
                const mainMenuScrollLeft = mainMenu.scrollLeft;
                DMegaSubmenu.forEach(megaSubmenu => {
                    megaSubmenu.style.transform = `translateX(${-mainMenuScrollLeft}px)`;
                });
            }

  
            document.querySelectorAll('.mobile-menu-link').forEach(link => {
        link.addEventListener('click', closeMobileMenu);
            });
        });



        function openNav() {
            document.getElementById("mySidenav").style.width = "250px";
        }

        function closeNav() {
            document.getElementById("mySidenav").style.width = "0";
        }



   