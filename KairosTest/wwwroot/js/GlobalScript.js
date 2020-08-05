function addCommas(angka) {
    var number_string = angka.toString().replace(/[^\d]/g, '').toString(),
        split = number_string.split(','),
        sisa = split[0].length % 3,
        rupiah = split[0].substr(0, sisa),
        ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    // tambahkan titik jika yang di input sudah menjadi angka ribuan
    if (ribuan) {
        separator = sisa ? ',' : '';
        rupiah += separator + ribuan.join(',');
    }

    rupiah = split[1] != undefined ? rupiah + ',' + split[1] : rupiah;
    return rupiah;
}

function addThousandSeparator(angka) {
    var number_string = angka.toString().replace(/[^\d]/g, '').toString(),
        split = number_string.split('.'),
        sisa = split[0].length % 3,
        rupiah = split[0].substr(0, sisa),
        ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    // tambahkan titik jika yang di input sudah menjadi angka ribuan
    if (ribuan) {
        separator = sisa ? '.' : '';
        rupiah += separator + ribuan.join('.');
    }

    rupiah = split[1] != undefined ? rupiah + '.' + split[1] : rupiah;
    return rupiah;
}


function removeComma(text) {
    var split = text.toString().split(",");
    return split.join("");
}

function removeThousandSeparator(text) {
    var split = text.toString().split(".");
    return split.join("");
}

function onlyNumbers(evt) {
    // Mendapatkan key code 
    var charCode = (evt.which) ? evt.which : event.keyCode;

    // Validasi hanya tombol angka
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}