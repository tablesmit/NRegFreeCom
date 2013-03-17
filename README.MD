# NRegFreeCom

COM is good for native interop and easy IPC, but lacks clean coded way to do such interaction without registry in .NET.
This library contains samples and reusable patters of such interation without registration.
Managed code is XCOPY on 32/64. Making native interlop also XCOPY by using pattern of deploying both versions of native libs, like windows libraries PInvokes works.

## Content
* PInvokes used to work with native libraries and reg free COM objects
* Sample how to load native library with module definition as assembly and providing it with managed COM services
* Samle of native library implementing managed COM interface searched and  instatiated by managed code using manifest
* Managed out of process COM server which uses Running Object Table to broadcast it instance
* Managed out of process COM server which does registration at start and unregisters when closing
* Working with native library resouces

# TODO:
* Enumerate DLL exports
* Make delegates for all standarts DEFs( DLL, COM)
* Make static typing of ROT usage
* Fix registration out of process
* Up comments,names,docs
* Add dependecny conflict for managed and for native and resolve both by SxS manifests 
* Imitate AppDomains based on runtime binding
* Add unit tests for in proc instantiation
* Add PE code (detecting managed headers, DEF and COM headers).
