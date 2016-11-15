Dim objHTTP, result 
Set objHTTP = CreateObject("Microsoft.XMLHTTP") 
Set ObjFSO = CreateObject("Scripting.FileSystemObject")
Set objLog = objFSO.CreateTextFile("logAutoGen2-Quarterly-Individual.txt")


objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- STR OF LOG FILE ---------------------------------:  " & Now)
objLog.WriteLine("  ")

'Generate for Kelana Centre Point

objLog.WriteLine("1) Start AutoGen [KCP][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=1", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("1) End AutoGen   [KCP][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Kelana Jaya

objLog.WriteLine("2) Start AutoGen [PKJ][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=2", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText  
objLog.WriteLine("2) End AutoGen   [PKJ][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")
 

'Generate for Plaza Glomac

objLog.WriteLine("3) Start AutoGen [PGL][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=3", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("3) End AutoGen   [PGL][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Business Centre

objLog.WriteLine("4) Start AutoGen [GBC][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=4", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText   
objLog.WriteLine("4) End AutoGen   [GBC][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Dwi Tasik

objLog.WriteLine("5) Start AutoGen [PDT][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=5", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("5) End AutoGen   [PDT][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Subang

objLog.WriteLine("6) Start AutoGen [ACS][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=6", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("6) End AutoGen   [ACS][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Shoplex Mont Kiara

objLog.WriteLine("7) Start AutoGen [SHP][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=7", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("7) End AutoGen   [SHP][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Square

objLog.WriteLine("8) Start AutoGen [GSQ][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=8", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("8) End AutoGen   [GSQ][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Menara Genesis

objLog.WriteLine("9) Start AutoGen [MGE][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=9", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("9) End AutoGen   [MGE][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Puteri Park

objLog.WriteLine("11) Start AutoGen [PTP][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=11", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("11) End AutoGen   [PTP][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Aman Suria

objLog.WriteLine("12) Start AutoGen [AMS][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=12", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("12) End AutoGen   [AMS][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Grand Continental

objLog.WriteLine("13) Start AutoGen [GCH][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=13", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("13) End AutoGen   [GCH][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Kelana Business Centre

objLog.WriteLine("14) Start AutoGen [KBC][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=14", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("14) End AutoGen   [KBC][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Plaza Metro

objLog.WriteLine("15) Start AutoGen [PMK][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=15", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("15) End AutoGen   [PMK][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for PJ8
'Stopped autogen for PJ8 wef March 16

'objLog.WriteLine("17) Start AutoGen [PJ8][Quarterly][Individual]: " & Now)
'objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=17", False 
'objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
'objHTTP.Send() 
'result = objHTTP.responseText 
'objLog.WriteLine("17) End AutoGen   [PJ8][Quarterly][Individual]: " & Now)
'objLog.WriteLine("  ")


'Generate for Bandar Seri Damansara

objLog.WriteLine("18) Start AutoGen [BSD][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=18", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("18) End AutoGen   [BSD][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Merchant Square

objLog.WriteLine("19) Start AutoGen [MSQ][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=19", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("19) End AutoGen   [MSQ][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Port Tech Tower

objLog.WriteLine("20) Start AutoGen [PTH][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=20", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("20) End AutoGen   [PTH][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")



'Generate for Galeria Hartamas

objLog.WriteLine("21) Start AutoGen [GHT][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=21", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("21) End AutoGen   [GHT][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Damansara

objLog.WriteLine("22) Start AutoGen [GLD][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=22", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("22) End AutoGen   [GLD][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Puchong

objLog.WriteLine("23) Start AutoGen [ACP][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=23", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("23) End AutoGen   [ACP][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Dataran Cascades

objLog.WriteLine("24) Start AutoGen [CSD][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=24", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("24) End AutoGen   [CSD][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")

'Generate for BLK

objLog.WriteLine("25) Start AutoGen [BLK][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=25", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("25) End AutoGen   [BLK][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")

'Generate for BLK

objLog.WriteLine("26) Start AutoGen [BLR][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=26", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("26) End AutoGen   [BLR][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for Casa

objLog.WriteLine("27) Start AutoGen [CST][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=27", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("27) End AutoGen   [CST][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")


'Generate for IAV

objLog.WriteLine("28) Start AutoGen [IAV][Quarterly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=I&locationInfoId=28", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("28) End AutoGen   [IAV][Quarterly][Individual]: " & Now)
objLog.WriteLine("  ")

objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- END OF LOG FILE ---------------------------------:  " & Now)
