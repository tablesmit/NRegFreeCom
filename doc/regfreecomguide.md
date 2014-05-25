
There are several options regarding registry:

Register in Registry during deployment.
---
Default way of Microsoft


Register in Registry during runtime
---

User `RegAsm`. It reflects upon CLR types and writes down them to registry. 
Is more low level (per type), managed (not uses native typelib implementation routines), not file based (like `regasm.exe`). 

Use Activation manifests
---

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


No registration and no COM activation. 
---

Use `ActivationContext.DangerouseCreateInstanceDirectly`(limited usage of COM activation) or `new`+`AssemblySystem` (custom COM activation). Direct `dll` loading and minimal marshalling without registry checks.

Register in Running Object Table (for out of process COM components)
---

For IPC, use `RunningObjectTable`.
