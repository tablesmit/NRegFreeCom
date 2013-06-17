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
* Tested on XP SP3 and Windows 7 SP2

### Samples

* Vistual Studio 2010 SP1 (needs Visual C++)
* Debug x86 
* Windows 7 SP 1, not tested on other systems


## TODO:
* write documentation, describe samples, use cases
* Enumerate DLL exports
* Make delegates for all standarts DEFs( DLL, COM)
* Fix registration out of process Runtime Registration Com
* Add dependency conflicts for managed and for native and resolve both by SxS manifests 
* Imitate AppDomains based on runtime binding
* Add PE code (detecting managed headers, DEF and COM headers).

## Other semi automaic approaches of doing native interop
* SWIG
* CXXI
* C++/CLI