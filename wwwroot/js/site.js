

//--------------------
//(function () {
//    var originalAddClassMethod = jQuery.fn.addClass;
//    jQuery.fn.addClass = function (eml) {
//        var result = originalAddClassMethod.apply(this, arguments);
//        jQuery(this).trigger('classChanged');
//        return result;
//    }
//})();

//$(function () {
//    $("select.selectpicker").on('classChanged', function () {
//        //Отработает, как только сменится класс
//        if ($(this).hasClass('input-validation-error'))
//            $(this).parent().children('button').addClass('input-validation-error');
//        else
//            $(this).parent().children('button').removeClass('input-validation-error');
//    });
//    $('select.selectpicker').on('changed.bs.select', function () {
//        $(this).focus();
//    });

//});

//-------------------

//$.validator.addMethod('classicmovie', function (value, element, params) {
//    var genre = $(params[0]).val(), year = params[1], date = new Date(value);

//    // The Classic genre has a value of '0'.
//    if (genre && genre.length > 0 && genre[0] === '0') {
//        // The release date for a Classic is valid if it's no greater than the given year.
//        return date.getUTCFullYear() <= year;
//    }

//    return true;
//});

//$.validator.unobtrusive.adapters.add('classicmovie', ['year'], function (options) {
//    var element = $(options.form).find('select#Movie_Genre')[0];

//    options.rules['classicmovie'] = [element, parseInt(options.params['year'])];
//    options.messages['classicmovie'] = options.message;
//});