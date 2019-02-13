//if (typeof ($("#FechaInicial").datepicker) !== "undefined" && typeof ($("#FechaInicial").datepicker) !== "function")
if (typeof ($("#FechaInicial").datepicker) !== "undefined" )
{
        $("#FechaInicial").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            dateFormat: 'yy-mm-dd',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShorth: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa']
        });

        $("#FechaFinal").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            dateFormat: 'yy-mm-dd',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShorth: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa']
        });
}

ValidaSoloNumeros = function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    //alert(charCode);
    if (charCode > 47 && charCode < 58) {
        return true;
    }
    else {
        if (charCode == 13 || charCode == 32 || charCode == 8 )
            return true;
        else
            return false;
    }
};

ValidaNumDeci = function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    //alert(charCode);
    if (charCode > 47 && charCode < 58) {
        return true;
    }
    else
    {
        if (charCode == 13 || charCode == 32 || charCode == 8 || charCode == 44)
            return true;
        else
            return false;
    }
};



limipiarEspeciales = function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    //alert(charCode);
    if (charCode > 47 && charCode < 58) {
        return true;
    }
    else {
        if (charCode > 64 && charCode < 91) {
            return true;
        }
        else {
            if (charCode > 96 && charCode < 123) {
                return true;
            }
            else {
                if (charCode == 13 || charCode == 32 || charCode == 8)
                    return true;
                else
                    return false;
            }

        }
    }

};

//$.validator.methods.range = function (value, element, param) {
//  var globalizedValue = value.replace(",", ".");
//  return (
//    this.optional(element) ||
//    (globalizedValue >= param[0] && globalizedValue <= param[1])
//  );
//};

$.validator.methods.number = function(value, element) {
  return (
    this.optional(element) ||
    /-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value)
  );
};

    $(function () {

        $('body').on('click', '.list-group .list-group-item', function () {
            $(this).toggleClass('active');
        });
        $('.list-arrows button').click(function () {
            var $button = $(this), actives = '';
            if ($button.hasClass('move-left')) {
                actives = $('.list-right ul li.active');
                actives.clone().appendTo('.list-left ul');
                actives.remove();
            } else if ($button.hasClass('move-right')) {
                actives = $('.list-left ul li.active');
                actives.clone().appendTo('.list-right ul');
                actives.remove();
            }
        });
        $('.dual-list .selector').click(function () {
            var $checkBox = $(this);
            if (!$checkBox.hasClass('selected')) {
                $checkBox.addClass('selected').closest('.well').find('ul li:not(.active)').addClass('active');
                $checkBox.children('i').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
            } else {
                $checkBox.removeClass('selected').closest('.well').find('ul li.active').removeClass('active');
                $checkBox.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
            }
        });
        $('[name="SearchDualList"]').keyup(function (e) {
            var code = e.keyCode || e.which;
            if (code == '9') return;
            if (code == '27') $(this).val(null);
            var $rows = $(this).closest('.dual-list').find('.list-group li');
            var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
            $rows.show().filter(function () {
                var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
                return !~text.indexOf(val);
            }).hide();
        });

    });

