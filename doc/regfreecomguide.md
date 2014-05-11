
There are several options regarding registry:

1. Register in Registry during deployment
Default way of Microsoft

2. Register in Registry during runtime
User `RegAsm`. It reflects upong CLR types and writes down them to registy. 
Is more low level (per type), managed (not uses native typelib implementation routines), not file based (like `regasm.exe`). 

3. Use Activation manifests
Use `ActivationContext`

```csharp
            var manifest = Path.Combine(program, "MyDll.dll.manifest");
            NRegFreeCom.ActivationContext.UsingManifestDo(manifest,
                                                          () =>
                                                              {
                                                                  IMyCom comObj = new MyCom();// COM object with manifest
                                                              });
```
COM will load all dll dependencies mentioned in manifest automatically if these are located in the same folder (and with manifests if have).

4. No registration and no COM activation. 
Use `ActivationContext.DangerouseCreateInstanceDirectly`(limited usage of COM activation) or `new`+`AssemblySystem` (custom COM activation). Direct `dll` loading and minimal marshalling without registry checks.

5. Register in Running Object Table (for out of process COM components)
For IPC, use `RunningObjectTable`.
