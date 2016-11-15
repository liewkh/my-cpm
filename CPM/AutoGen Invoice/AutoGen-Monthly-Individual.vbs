Dim objHTTP, result 
Set objHTTP = CreateObject("Microsoft.XMLHTTP") 
Set ObjFSO = CreateObject("Scripting.FileSystemObject")
Set objLog = objFSO.CreateTextFile("logAutoGen-Monthly-Individual.txt")


objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- STR OF LOG FILE ---------------------------------:  " & Now)
objLog.WriteLine("  ")

'Generate for Kelana Centre Point

objLog.WriteLine("1) Start AutoGen [KCP][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=1", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("1) End AutoGen   [KCP][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Kelana Jaya

objLog.WriteLine("2) Start AutoGen [PKJ][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=2", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText  
objLog.WriteLine("2) End AutoGen   [PKJ][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")
 

'Generate for Plaza Glomac

objLog.WriteLine("3) Start AutoGen [PGL][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=3", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("3) End AutoGen   [PGL][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Business Centre

objLog.WriteLine("4) Start AutoGen [GBC][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=4", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText   
objLog.WriteLine("4) End AutoGen   [GBC][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Dwi Tasik

objLog.WriteLine("5) Start AutoGen [PDT][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=5", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("5) End AutoGen   [PDT][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Subang

objLog.WriteLine("6) Start AutoGen [ACS][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=6", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("6) End AutoGen   [ACS][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Shoplex Mont Kiara

objLog.WriteLine("7) Start AutoGen [SHP][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=7", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("7) End AutoGen   [SHP][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Square

objLog.WriteLine("8) Start AutoGen [GSQ][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=8", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("8) End AutoGen   [GSQ][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Menara Genesis

objLog.WriteLine("9) Start AutoGen [MGE][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=9", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("9) End AutoGen   [MGE][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Puteri Park

objLog.WriteLine("11) Start AutoGen [PTP][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=11", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("11) End AutoGen   [PTP][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Aman Suria

objLog.WriteLine("12) Start AutoGen [AMS][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=12", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("12) End AutoGen   [AMS][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Grand Continental

objLog.WriteLine("13) Start AutoGen [GCH][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=13", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("13) End AutoGen   [GCH][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Kelana Business Centre

objLog.WriteLine("14) Start AutoGen [KBC][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=14", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("14) End AutoGen   [KBC][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Plaza Metro

objLog.WriteLine("15) Start AutoGen [PMK][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=15", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("15) End AutoGen   [PMK][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for PJ8
'Stopped autogen for PJ8 wef March 16

'objLog.WriteLine("17) Start AutoGen [PJ8][Monthly][Individual]: " & Now)
'objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=17", False 
'objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
'objHTTP.Send() 
'result = objHTTP.responseText 
'objLog.WriteLine("17) End AutoGen   [PJ8][Monthly][Individual]: " & Now)
'objLog.WriteLine("  ")


'Generate for Bandar Seri Damansara

objLog.WriteLine("18) Start AutoGen [BSD][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=18", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("18) End AutoGen   [BSD][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Merchant Square

objLog.WriteLine("19) Start AutoGen [MSQ][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=19", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("19) End AutoGen   [MSQ][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Port Tech Tower

objLog.WriteLine("20) Start AutoGen [PTH][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=20", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("20) End AutoGen   [PTH][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Galeria Hartamas

objLog.WriteLine("21) Start AutoGen [GHT][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=21", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("21) End AutoGen   [GHT][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Damansara

objLog.WriteLine("22) Start AutoGen [GLD][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=22", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("22) End AutoGen   [GLD][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Puchong

objLog.WriteLine("23) Start AutoGen [ACP][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=23", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("23) End AutoGen   [ACP][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Dataran Cascades

objLog.WriteLine("24) Start AutoGen [CSD][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=24", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("24) End AutoGen   [CSD][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for C180 Balakong

objLog.WriteLine("25) Start AutoGen [BLK][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=25", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("25) End AutoGen   [BLK][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for C180 Residences

objLog.WriteLine("26) Start AutoGen [BLR][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=26", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("26) End AutoGen   [BLR][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Casa

objLog.WriteLine("27) Start AutoGen [CST][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=27", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("27) End AutoGen   [CST][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")

'Generate for IAV

objLog.WriteLine("28) Start AutoGen [IAV][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=28", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("28) End AutoGen   [IAV][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")


objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- END OF LOG FILE ---------------------------------:  " & Now)
