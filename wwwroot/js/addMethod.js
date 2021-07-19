$.validator.addMethod("fulldatevalid",
    function (value, element, param) {
        var arr = value.split("-");
       
        var source_date = new Date(arr[0], arr[1], arr[2]);
        var hasNumber = value.match(/\d/);
        console.log(arr, $(element), param, hasNumber, source_date);
        if (isNaN(source_date))
            return false

        return true;
    });

$.validator.unobtrusive.adapters.addBool("fulldatevalid");