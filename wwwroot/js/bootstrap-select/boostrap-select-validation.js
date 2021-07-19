(function () {
    var originalAddClassMethod = jQuery.fn.addClass;
    jQuery.fn.addClass = function (eml) {
        var result = originalAddClassMethod.apply(this, arguments);
        jQuery(this).trigger('classChanged');
        return result;
    }
})();

$(function () {
    $("select.selectpicker").on('classChanged', function () {
        //Отработает, как только сменится класс
        if ($(this).hasClass('input-validation-error'))
            $(this).parent().children('button').addClass('input-validation-error');
        else
            $(this).parent().children('button').removeClass('input-validation-error');
    });
    $('select.selectpicker').on('changed.bs.select', function () {
        $(this).focus();
    });

});