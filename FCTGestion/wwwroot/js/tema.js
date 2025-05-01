document.addEventListener('DOMContentLoaded', function () {
    const toggle = document.getElementById('toggleTema');
    if (!toggle) return;

    toggle.addEventListener('change', () => {
        const body = document.body;
        body.classList.toggle('modo-oscuro');
        const tema = body.classList.contains('modo-oscuro') ? 'oscuro' : 'claro';
        localStorage.setItem('tema', tema);
        document.cookie = `tema=${tema}; path=/; max-age=31536000`;
    });
});
