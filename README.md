## NRegFreeCom

 COM is good for native interop and easy IPC, but lacks clean coded way to do such interaction without registry in .NET.

 This library contains samples and reusable patters of such interation without registration.

 Managed code is XCOPY on 32/64. Making native interlop also XCOPY by using pattern of deploying both versions of native libs and deciding in runtime, like OS libraries PInvokes work.
 Managed code have AppDomains with dll search and memory isolation, striving to do the same of native COM.

## Content
* PInvokes used to work with native libraries and reg free COM objects
* Sample how to load native library with module definition as assembly and providing it with managed COM services
* Sample of native library implementing managed COM interface searched and  instatiated by managed code using manifest
* Managed out of process COM server which uses Running Object Table to broadcast it instance (zero Windows Registy info)
* Managed out of process COM server which does registration at start and unregisters when closing
* Working with native library dependencies and resources

## Build and deployment

* `NRegFreeCom` is redistributable managed library, can be build in Release and Debug mode ShardDevelop or Visual Studio 2010 SP1. Is build for `Any Cpu`
* Library testing is done on XP SP3 and Windows 7 SP2

### Samples build

* Vistual Studio 2010 SP1 (needs Visual C++)
* Debug x86 
* Windows 7 SP 2


## Samples

```csharp

       [Test]
		[Description("Creates and calls managed registration free object")]
        public void CreateInstanceWithManifest_inProcessManagedServer_OK()
        {		    
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Implementations.dll.manifest");
            var guid = new Guid(RegFreeComIds.CLSID);

            var obj = ActivationContext.CreateInstanceWithManifest(guid, path);
            var inf = (IRegFreeCom)obj;
            var result = inf.Answer();
			
            Assert.IsTrue(result == 42);
        }
		
		[Test]
        [Description("Loads library from subfolder with dependencies searched in this subfolder")]
        public void TestDllDependenciesLoading()
        {
            var loader = new AssemblySystem();

            // C# loads C++ from Win32 or x64 subfolder
            var anyCpu = loader.GetAnyCpuPath(loader.BaseDirectory);
            loader.AddSearchPath(anyCpu);
            var module = loader.LoadFrom(anyCpu, "NativeLibraryConsumer.dll");
            var fn = module.GetDelegate<fnNativeLibraryConsumer>();
            object[] retval;

            Assert.IsTrue(42 == fn(out retval));
            loader.Dispose();
        }

```
See tests and samples in code for other functional.

##Notes

### In order to ease integration of C++ and C# I think could be good to:

* C libs loaded to provide HRESULT STDAPICALLTYPE exports with COM memory management methods used to be similar to COM lifecylte functions exported for uniformity of calls. 
* COM interfaces to be as simple as possible so these could be implemented manually (without wizards) by developer with low C++ skill.
* Strive to have expericene of new WinRT (*.winmd, C++/CX, WRL).


### Isolation and integration

 Native code does not have AppDomains, but there are 3 technologies which helps to solve problems of conflicting components:
* Windows provided features like Activation Contexts, application `manifest`s, `dll.2.config`, dynamic and controlled libraries loading and symbols binding, SxS
* Multiprocess architecture
* There are Dependency Injection and Inversion of Control tools for C++, but because of lack of runtime metadata in native code by default not so powerful as in .NET
* EasyHook - The reinvention of Windows API Hooking https://easyhook.codeplex.com/

This lib strives to make .NET engine to load native code in isolated maner.

## TODO:
* write documentation, describe samples, use cases
* Enumerate DLL exports
* Make delegates for all standarts DEFs( DLL, COM)
* Fix registration out of process Runtime Registration Com
* Add dependency conflicts for managed and for native and resolve both by SxS manifests 
* Imitate AppDomains based on runtime binding
* Add PE code (detecting managed headers, DEF and COM headers).

## Other semi automaic approaches of doing native interop

* C++/CLI
* IDL COM ATL Wizards
* C++/CX, WRL
* SWIG
* CXXI (Linux tech)