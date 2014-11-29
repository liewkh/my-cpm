// Return the Key pressed
function getKey(e) {
  if (window.event)
    return window.event.keyCode;
  else if (e)
    return e.which;
  else
    return null;
}


function clickTab(scr,frame,tab)
{
	window.location.href=scr;
}

function ShowModalDialogwin(url,mwidth,mheight)
{
 window.showModalDialog(url, "", 'help:0;resizable:1;dialogWidth:'+mwidth+'px;dialogHeight:'+mheight+'px');
}
function modelesswin(url,mwidth,mheight)
{
    if (document.all&&window.print) //if ie5
            eval('window.showModelessDialog(url,"","help:0;resizable:1;dialogWidth:'+mwidth+'px;dialogHeight:'+mheight+'px")')
    else
            eval('window.open(url,"","width='+mwidth+'px,height='+mheight+'px,resizable=1,scrollbars=1")')
}
function changeTab(fromTabName, toTabName) {
        document.all(fromTabName).style.display = "none";
        document.all(toTabName).style.display = "block";
}
function openWindow( href, w, h, t, l, windowName ) {
        var para = "toolbar=no, menubar=no, status=yes, scrollbars=yes, resizable=no, width=" + w + ", height=" + h + ", top=" + t + ", left=" + l;
        var newOne = window.open(href,windowName,para);
}

function changeVisibleObject(fromObjName, toObjName) {
	document.all(fromObjName).style.display = "none";
	document.all(toObjName).style.display = "inline";
}
function calAge(birthDate)
{
var curYear=new Date().getYear();
var birthYear=birthDate.substring(6,10);
var age=curYear-birthYear;
return age;	
}

function showCrystalReportDialog(url, width, height) {
var sFeatures;

sFeatures="dialogWidth:"+width+"px; ";
sFeatures+="dialogHeight:"+height+"px; ";
sFeatures+="help:no; ";
//sFeatures+="resizable:no; ";
//sFeatures+="scroll:no; ";
sFeatures+="status:no; ";
sFeatures+="unadorned:no; ";

var result;
result = window.showModalDialog(url, window.self, sFeatures)
//result = window.open(url, "", sFeatures)
return result;
}

function formatCurrency(num) {
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
return (((sign)?'':'-') + '' + num + '.' + cents);
}

/***************************************************************************************
    A function accept 1 valid date string and compare 1st one with Today's date(2nd date).
    The return value is an integer represent the different days between 1st and 2nd date,
    if 1st date is a date b4 2nd date, this function return negative value.

    Required Function	: getYear(Date), getMonth(Date), getDay(Date)
    Association	: IsDate(Expression)

    Returns: Integer. 0  => Date1 = Date2
                      1  => Date1 > Date2
                      -1 => Date1 < Date2

***************************************************************************************/
    function compareDateTime(pDate1, pDate2) {
        var Date1 = new Date(getYear(pDate1),getMonth(pDate1) - 1,getDay(pDate1));
        var Date2 = new Date(getYear(pDate2),getMonth(pDate2) - 1,getDay(pDate2));
        
        var ret=0;
        if (Date1.getDate() == Date2.getDate() && Date1.getMonth() == Date2.getMonth() && Date1.getYear() == Date2.getYear())
        {   ret=0; }
        else if (Date1.getYear() > Date2.getYear())
            ret=1;
        else if (Date1.getMonth() > Date2.getMonth() && Date1.getYear() == Date2.getYear()) {
            ret=1;
              }
        else if (Date1.getDate() > Date2.getDate() && Date1.getMonth() == Date2.getMonth() && Date1.getYear() == Date2.getYear())
            ret=1;
        else {
            ret=-1;
              }
        return (ret);
    }


           
