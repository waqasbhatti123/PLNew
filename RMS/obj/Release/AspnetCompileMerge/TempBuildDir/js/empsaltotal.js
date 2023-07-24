$(document).ready(function() {
        if ($(".txtBasicPay").val() == "") {
            //$(".txtBasicPay").val(0);
        }
        if ($(".txtHouseRent").val() == "") {
            //$(".txtHouseRent").val(0);
        }
        if ($(".txtUtilities").val() == "") {
            //$(".txtUtilities").val(0);
        }
        if ($(".txtFuelAll").val() == "") {
            //$(".txtFuelAll").val(0);
        }
        if ($(".txtAllounce").val() == "") {
            //$(".txtAllounce").val(0);
        }
        if ($(".txtTaxDed").val() == "") {
            //$(".txtTaxDed").val(0);
        }
        if ($(".txtMessDed").val() == "") {
            //$(".txtMessDed").val(0);
        }
        if ($(".txtSplAll").val() == "") {
            //$(".txtSplAll").val(0);
        }
        if ($(".txtOtherDed").val() == "") {
            //$(".txtOtherDed").val(0);
        }
        
        $(".txtBasicPay").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }

            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });


        $(".txtHouseRent").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });

        $(".txtUtilities").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });

        $(".txtFuelAll").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });

        $(".txtAllounce").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });

        $(".txtTaxDed").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d  + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });
        
        $(".txtMessDed").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });

        $(".txtSplAll").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }
            
            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
            
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });
        $(".txtOtherDed").keyup(function() {
            if($(".txtBasicPay").val() == "") { var a = 0; }
            else { var a = parseFloat($(".txtBasicPay").val()); } 
            if($(".txtHouseRent").val() == "") { var b = 0; }
            else { var b = parseFloat($(".txtHouseRent").val()); }
            if($(".txtUtilities").val() == "") { var c = 0; }
            else { var c = parseFloat($(".txtUtilities").val()); }

            if ($(".txtFuelAll").val() == "") { var d = 0; }
            else { var d = parseFloat($(".txtFuelAll").val()); }
                       
            if($(".txtAllounce").val() == "") { var e = 0; }
            else { var e = parseFloat($(".txtAllounce").val()); }
            if($(".txtTaxDed").val() == "") { var f = 0; }
            else { var f = parseFloat($(".txtTaxDed").val()); }
            if($(".txtMessDed").val() == "") { var m = 0; }
            else { var m = parseFloat($(".txtMessDed").val()); }
            if($(".txtSplAll").val() == "") { var g = 0; }
            else { var g = parseFloat($(".txtSplAll").val()); }
            if($(".txtOtherDed").val() == "") { var h = 0; }
            else { var h = parseFloat($(".txtOtherDed").val()); }
            
            var res = a + b + c + d + e - f + g - h - m;

            $(".RequiredFieldFinal").val(res)
        });
    });