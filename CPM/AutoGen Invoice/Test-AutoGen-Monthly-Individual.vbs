Dim objHTTP, result 
Set objHTTP = CreateObject("Microsoft.XMLHTTP") 
Set ObjFSO = CreateObject("Scripting.FileSystemObject")
Set objLog = objFSO.CreateTextFile("test-logAutoGen-Monthly-Individual.txt")


objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- STR OF LOG FILE ---------------------------------:  " & Now)
objLog.WriteLine("  ")

'Generate for Kelana Centre Point

objLog.WriteLine("1) Start AutoGen [KCP][Monthly][Individual]: " & Now)
objHTTP.open "POST","http://localhost/CPM/WebService/WebService.asmx/GenerateInvoiceMonthly?category=I&locationInfoId=1&debtorid=7631", False 
objHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded" 
objHTTP.Send() 
result = objHTTP.responseText 
objLog.WriteLine("1) End AutoGen   [KCP][Monthly][Individual]: " & Now)
objLog.WriteLine("  ")




objLog.WriteLine("  ")
objLog.WriteLine("---------------------------------- END OF LOG FILE ---------------------------------:  " & Now)
