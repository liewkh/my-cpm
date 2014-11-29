
function onMouseOverButton(f) {

	f.style.background="#F1F90C"; f.style.color="#77A44B";
}

function onMouseOutButton(f) {
	f.style.background="#587938"; f.style.color="#F1F90C";
}

function onMouseDownButton(f) {
	f.style.color="#FF0000";
}

function onMouseOverRow(theRow, currentRow) {
	var theTbl = theRow.parentNode.parentNode;
	var rowNum = theRow.rowIndex;

	if(currentRow)
		clearRow(theTbl, currentRow);

	highlightRow(theTbl, rowNum);
	return rowNum;
}

function onMouseOutRow(theRow) {
	var theTbl = theRow.parentNode.parentNode;
	var rowNum = theRow.rowIndex;

	clearRow(theTbl, rowNum);
	return true;
}

function clearRow(theTbl, rowNum) {

        if (rowNum%2 != 0)
          theTbl.rows[rowNum].className='ssCellEven';
        else
          theTbl.rows[rowNum].className='ssCellOdd';

	return true;
}

function highlightRow(theTbl, rowNum) {

  theTbl.rows[rowNum].className='ssRowOnMouseOver';
  theTbl.rows[rowNum].focus();
}

//For move the progress bar when scroll
   function getSize(type) {
     var myWidth = 0, myHeight = 0;
     if( typeof( window.innerWidth ) == 'number' ) {
         //Non-IE
         myWidth = window.innerWidth;
         myHeight = window.innerHeight;
     } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
         //IE 6+ in 'standards compliant mode'
         myWidth = document.documentElement.clientWidth;
         myHeight = document.documentElement.clientHeight;
     } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
         //IE 4 compatible
         myWidth = document.body.clientWidth;
         myHeight = document.body.clientHeight;
     }
     if (type == "W") {
       return myWidth;
     }
     if (type == "H") {
       return myHeight;
     }
   }

   function getScrollPosition(type) {
     
     var scrOfX = 0, scrOfY = 0;
     if( typeof( window.pageYOffset ) == 'number' ) {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
     } else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
     } else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
     }
     if (type == "T") {       
       return scrOfY;
     }
     if (type == "L") {
       return scrOfX;
     }
   }

   function move(){       
    document.getElementById('pnlPopup').style.top=getScrollPosition("T") + (getSize("H")  / 2);
    document.getElementById('pnlPopup').style.top=getScrollPosition("T")   + (getSize("H")  / 2);
    document.getElementById('pnlPopup').style.left=getScrollPosition("L") + (getSize("W")  / 2);
    document.getElementById('pnlPopup').style.left=getScrollPosition("L")  + (getSize("W")  / 2);
   }
   
//Scroll to top page after postback
function scrollTop()

            {
                window.document.body.scrollTop = 0;
                window.document.documentElement.scrollTop = 0;
            }
            
            function ResetScrollPosition()
{

    setTimeout("window.scrollTo(0,0)",0);

}   


