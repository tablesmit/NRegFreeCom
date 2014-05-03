## What makes C# .NET ready for developing Windows desktop applications

Problem
---
.NET desktop applications take long time to start, are slow and CPU consuming while using runtime features like reflection, are slow and CPU consuming when integrating with native code (e.g. can call only high level COM API while native code can call hich performance API of native components).

Context
---
- User can open and close desktop application many times.
- Desktop application can be deployed and run in restricted environment, hence cannot put assemblies into GAC.
- Windows is unmanaged OS with all core user apps written in unmanaged code, hence .NET need to be well integrated into these to call API with high performance.
-  Main .NET language - C# - does not have compile time meta programming build in.
-  .NET has many places with runtime code generations which should avoided.

Enforces
---
Using 3rd party tools for compile time code degeneration instead of runtime. Loose will be mostly in ease of build and support. But this will allow .NET be good citizen e.g. as MS Office add-in or as applications which does auto start after window launch.

For now
---
- Parallel Just in Time(JIT) compilation and good Ahead of Time(AOT) support (NGEN). .NET 4.5 has both.
- Low overhead COM events support via ComEventsHelper. .NET 4.0 has it.
- More COM support like ICustomQueryInterface since .NET 4.0. Looks almost each new version of .NET adds more stuff.
- Fast first usage of XML serializes by default, faster start up time. .NET 4.5 has it.
- Coexsitence of big C++ and big C# heap in the same process. Looks almost each new version  of .NET adds more APIs for GC and CLR hosting tuning.
- Enough of free and opensource tools for .NET profiling.
- Opensource tools and libraries of COM and C interop [2] [3]

Better to have in future
---
- Widespread of .NET 4.5.
- Integration of COM with CLI metadata. WinRT apps has some. Need the same for Desktop.
- Compile time generics-templates, good tool for AOP or other kind of metaprogramming.
- Compile time Dependency Injection container.
- Compile time analysis of runtime reflection and code generation usage to prevent both.
- Support for fast interface/attribute based inter process communication(IPC) with pregeneration of proxy/stub like in [1]
- Some way to do RAII to release references counted objects (specifically STA COM) instead of calling Marshal.Release or wrapping into using directly.

[1] http://visualstudio.uservoice.com/forums/121579-visual-studio/suggestions/4034029-add-support-of-ms-rpc-into-c-generate-c-from-i
[2] https://clrinterop.codeplex.com/