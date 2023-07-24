function Popup(url, windowName) {
    window.open(url, windowName, 'width=500, height=500, menubar=yes, scrollbars=yes, resizable=yes');
    return false;
}


function searchSel(strTextbox, ddlOrig, ddlSearch) {
    var input = document.getElementById(strTextbox).value.toLowerCase();
    var output = document.getElementById(ddlSearch).options;
    var inp = document.getElementById(ddlOrig);

    var inpoption = document.getElementById(ddlOrig).options;

    var limit = inpoption.length;
    for (var j = 0; j < limit; j = j + 1) {

        inpoption[0] = null;
    }

    for (var i = 0; i < output.length; i++) {
        if (output[i].text.toLowerCase().indexOf(input) == 0) {

            addOption(inp, output[i].text, output[i].value);
        }


    }
}


function addOption(selectbox, text, value) {
    var optn = document.createElement("OPTION");
    optn.text = text;
    optn.value = value;
    selectbox.options.add(optn);
}

function LimitText(txt,limit)
{
 if( txt.value.length>limit )
    txt.value =txt.value.substring(0, limit)

}

function addDate(frmDte,toDte)
{
    var iyear, imonth, iday, ihour, imin;
   var  sDateTo = frmDte.value;
    //alert(frmDte.value.trim().length);
    if(frmDte != null && frmDte.value != "" && frmDte.value.trim().length == 16)
    {
        iyear = parseInt(sDateTo.substring(0,4));
        imonth = parseInt(sDateTo.substring(5,7));
        iday = parseInt(sDateTo.substring(8,10));
        ihour = parseInt(sDateTo.substring(11,13));
        imin = parseInt(sDateTo.substring(14,16));

        imonth--;

        var myDate = new Date(iyear, imonth, iday, ihour, imin);
        myDate.setDate(myDate.getDate() + 1);

        iyear = myDate.getFullYear();

        imonth = parseInt(myDate.getMonth())+1;
        if(imonth < 10)
            imonth = "0"+imonth;

        iday = myDate.getDate();
        if(iday < 10)
            iday = "0"+iday;

        ihour = parseInt(myDate.getHours());
        if(ihour < 10)
            ihour = "0"+ihour;

        imin = parseInt(myDate.getMinutes());
        if(imin < 10)
            imin = "0"+imin;

        toDte.value = iyear+"-"+imonth+"-"+iday+" "+ihour+":"+imin;
    }
}

