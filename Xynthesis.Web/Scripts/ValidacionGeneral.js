function validar(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // 3
    patron = /[\wA-Za-zñÑáéíóúÁÉÍÓÚ*\s]/; // 4
    te = String.fromCharCode(tecla); // 5
    return patron.test(te); // 6
}