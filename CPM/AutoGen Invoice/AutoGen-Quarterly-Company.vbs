Dim objHTTP, result 
Set objHTTP = CreateObject("Microsoft.XMLHTTP") 
Set ObjFSO = CreateObject("Scripting.FileSystemObject")
Set objLog = objFSO.CreateTextFile("logAutoGen-Quarterly-Company.txt")


objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- STR OF LOG FILE ---------------------------------:  " & Now)
objLog.WriteLine("  ")

'Generate for Kelana Centre Point

objLog.WriteLine("1) Start AutoGen [KCP][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=1", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("1) End AutoGen   [KCP][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Kelana Jaya

objLog.WriteLine("2) Start AutoGen [PKJ][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=2", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText  
objLog.WriteLine("2) End AutoGen   [PKJ][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")
 

'Generate for Plaza Glomac

objLog.WriteLine("3) Start AutoGen [PGL][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=3", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("3) End AutoGen   [PGL][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Business Centre

objLog.WriteLine("4) Start AutoGen [GBC][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=4", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText   
objLog.WriteLine("4) End AutoGen   [GBC][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Plaza Dwi Tasik

objLog.WriteLine("5) Start AutoGen [PDT][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=5", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("5) End AutoGen   [PDT][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Subang

objLog.WriteLine("6) Start AutoGen [ACS][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=6", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("6) End AutoGen   [ACS][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Shoplex Mont Kiara

objLog.WriteLine("7) Start AutoGen [SHP][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=7", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("7) End AutoGen   [SHP][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Square

objLog.WriteLine("8) Start AutoGen [GSQ][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=8", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("8) End AutoGen   [GSQ][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Menara Genesis

objLog.WriteLine("9) Start AutoGen [MGE][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=9", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("9) End AutoGen   [MGE][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")



'Generate for Puteri Park

objLog.WriteLine("11) Start AutoGen [PTP][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=11", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("11) End AutoGen   [PTP][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Aman Suria

objLog.WriteLine("12) Start AutoGen [AMS][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=12", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("12) End AutoGen   [AMS][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Grand Continental

objLog.WriteLine("13) Start AutoGen [GCH][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=13", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("13) End AutoGen   [GCH][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Kelana Business Centre

objLog.WriteLine("14) Start AutoGen [KBC][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=14", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("14) End AutoGen   [KBC][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")



'Generate for Plaza Metro

objLog.WriteLine("15) Start AutoGen [PMK][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=15", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("15) End AutoGen   [PMK][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for PJ8
'Stopped autogen for PJ8 wef from March 2016

'objLog.WriteLine("17) Start AutoGen [PJ8][Quarterly][Company]: " & Now)
'objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=17", False 
'objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
'objHTTP.Send() 
'result = objHTTP.responseText 
'objLog.WriteLine("17) End AutoGen   [PJ8][Quarterly][Company]: " & Now)
'objLog.WriteLine("  ")


'Generate for Bandar Seri Damansara

objLog.WriteLine("18) Start AutoGen [BSD][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=18", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("18) End AutoGen   [BSD][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")



'Generate for Merchant Square

objLog.WriteLine("19) Start AutoGen [MSQ][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=19", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("19) End AutoGen   [MSQ][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")



'Generate for Port Tech Tower

objLog.WriteLine("20) Start AutoGen [PTH][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=20", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("20) End AutoGen   [PTH][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")



'Generate for Galeria Hartamas

objLog.WriteLine("21) Start AutoGen [GHT][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=21", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("21) End AutoGen   [GHT][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Glomac Damansara

objLog.WriteLine("22) Start AutoGen [GLD][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=22", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("22) End AutoGen   [GLD][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Asia Cafe Puchong

objLog.WriteLine("23) Start AutoGen [ACP][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=23", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("23) End AutoGen   [ACP][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")

'Generate for Dataran Cascades

objLog.WriteLine("24) Start AutoGen [CSD][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=24", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("24) End AutoGen   [CSD][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")

'Generate for C180 Balakong

objLog.WriteLine("25) Start AutoGen [BLK][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=25", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("25) End AutoGen   [BLK][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")

'Generate for C180 Residences

objLog.WriteLine("26) Start AutoGen [BLR][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=26", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("26) End AutoGen   [BLR][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")


'Generate for Casa

objLog.WriteLine("27) Start AutoGen [CST][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=27", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("27) End AutoGen   [CST][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")

'Generate for IAV

objLog.WriteLine("28) Start AutoGen [IAV][Quarterly][Company]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceQuarterly?category=C&locationInfoId=28", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("28) End AutoGen   [IAV][Quarterly][Company]: " & Now)
objLog.WriteLine("  ")

objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- END OF LOG FILE ---------------------------------:  " & Now)
