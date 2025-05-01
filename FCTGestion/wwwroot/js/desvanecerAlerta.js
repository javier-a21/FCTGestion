
    setTimeout(function () {
            var alerta = document.getElementById('alertaTemporal');
    if (alerta) {
        alerta.classList.remove('show');
    alerta.classList.add('fade');
    alerta.style.opacity = '0';


                setTimeout(() => alerta.remove(), 500);
            }
        }, 3000);
