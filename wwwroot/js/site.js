// Animazione caricamento pagine
window.addEventListener('load', () => {
    const container = document.getElementById('main-content');
    if (container) {
        container.classList.add('show');
    }

    const footerBar = document.getElementById('footer-navbar');
    
    if(footerBar) {
        var isMobile = window.matchMedia("(max-width: 768px)").matches;
        if (!isMobile) {
            return;
        }
        
        var hasScroll = document.documentElement.scrollHeight > window.innerHeight;

        function checkScroll() {
            if(!hasScroll) {
                footerBar.style.display = 'block';
            } else {
                if (window.scrollY > 0) {
                    footerBar.style.display = 'block';
                } else {
                    footerBar.style.display = 'none';
                }
            }
        }

        checkScroll();
        window.addEventListener('scroll', checkScroll);
    }
});

// Back to top button
const backToTopBtn = document.getElementById('backToTopBtn');
window.addEventListener("scroll", function () {
if (document.documentElement.scrollTop > 700) {
    backToTopBtn.style.display = "block";
} else {
    backToTopBtn.style.display = "none";
}
});

backToTopBtn.addEventListener("click", function () {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
});

// Menù mobile
const navMobile = document.getElementById('navbar-mobile');
const menu = document.getElementById('mobile-menu');

function openMenu() {
    document.documentElement.style.overflow = 'hidden';
    menu.style.display = 'flex';
    navMobile.style.display = 'none';
};
function closeMenu() {
    document.documentElement.style.overflow = 'auto';
    menu.style.display = 'none';
}

// Login e registrazione
const modalLogin = $('#modal-login');
const loginForm = document.getElementById('login-form');
const alertLogin = loginForm.querySelector('div #fail-login');

const modalRegistration = $('#modal-registration');
const registrationForm = document.getElementById('registration-form');
const alertRegistration = registrationForm.querySelector('div #fail-registration');

modalLogin.on('hidden.bs.modal', () => {
    loginForm.reset();
    alertLogin.style.display = 'none';
});
modalRegistration.on('hidden.bs.modal', () => {
    registrationForm.reset();
    alertRegistration.style.display = 'none';
});

const fromLoginGoToRegistration = document.querySelector('#modal-login #to-registration');
fromLoginGoToRegistration.addEventListener('click', () => {
    $('#modal-login').modal('hide');
    $('#modal-registration').modal('show');
});
const fromRegistrationGoToLogin = document.querySelector('#modal-registration #to-login');
fromRegistrationGoToLogin.addEventListener('click', () => {
    $('#modal-registration').modal('hide');
    $('#modal-login').modal('show');
});

// Login
loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const submitBtn = loginForm.querySelector('button[type="submit"]');
    submitBtn.setAttribute('disabled', '');
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';

    const formData = new FormData(loginForm);
    const url = loginForm.dataset.url;

    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });

    if(response.ok) {
        const user = await response.json();

        if(user.role === 'admin') {
            window.location.href = '/admin';
        } else {
            window.location.href = '/reservations';
        }
    } else {
        alertLogin.style.display = 'block';
        submitBtn.removeAttribute('disabled');
        submitBtn.innerHTML = 'Accedi';
    }
});

// Nuova registrazione
registrationForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const submitBtn = registrationForm.querySelector('button[type="submit"]');
    submitBtn.setAttribute('disabled', '');
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';

    const formData = new FormData(registrationForm);
    const url = registrationForm.dataset.url;

    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });

    if(response.ok) {
        window.location.href = "/";
    } else {
        alertRegistration.style.display = 'block';
        submitBtn.removeAttribute('disabled');
        submitBtn.innerHTML = 'Registrati';
    }
});