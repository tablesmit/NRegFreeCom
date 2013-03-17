Set obj = GetObject("CSExeCOMServer.ROTClass")
b=obj.TestFunc("a", 1)
obj.FloatProperty = 123
wscript.echo obj.FloatProperty
wscript.echo obj.ProcName
wscript.echo b