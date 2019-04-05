/*
    Validator v1.1.0
    (c) Yair Even Or
    https://github.com/yairEO/validator

    MIT-style license.
*/

    //<![CDATA[
    function fileValidation() {
        var fileInput = document.getElementById('archivo');
        var filePath = fileInput.value;
        var allowedExtensions = /(.xlsx)$/i;
        if (!allowedExtensions.exec(filePath)) {
            alert('Solo se permiten archivos Excel');
            fileInput.value = '';
            return false;
        } else {
            //Image preview
            if (fileInput.files && fileInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('imagePreview').innerHTML = '<img src="' + e.target.result + '"/>';
                };
                reader.readAsDataURL(fileInput.files[0]);
            }
        }
    }