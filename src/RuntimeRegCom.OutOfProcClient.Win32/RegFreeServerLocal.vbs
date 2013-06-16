Dim actCtx As Object
Set actCtx = CreateObject("Microsoft.Windows.ActCtx")
actCtx.Manifest = "RegFreeCom.manifest"

Dim testObject As Object
Set testObject = actCtx.CreateObject("RegFreeCom.SimpleObject") 'This line throws... 

Dim text As String
text = thing.GetString(42)

Debug.Print text