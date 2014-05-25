## Isolation and integration : native vs. managed

  Managed code have AppDomains with dll search and memory isolation.
  Native code(C or COM) does not have AppDomains and have static linking for isolation. There are technologies which helps to solve/mitigate problems of conflicting components while dynamic linking of native components:
* Windows provided features like Activation Contexts, application `manifest`s, `dll.2.config`, dynamic and controlled libraries loading and symbols binding, SxS
* EasyHook - The reinvention of Windows API Hooking https://easyhook.codeplex.com/
  
##### Some isolation can be achieved by design:
* Multiprocess architecture
* There are Dependency Injection and Inversion of Control tools for C++, but because of lack of runtime metadata in native code by default not so powerful as in .NET
