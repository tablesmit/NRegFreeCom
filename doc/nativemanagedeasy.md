## Guide to ease integration of C++ and C#:

### Manual
* C libs loaded to provide HRESULT STDAPICALLTYPE exports with COM memory management methods used to be similar to COM lifestyle functions exported for uniformity of calls.  
* No Windows registry involved. XCOPY deployment. 
* COM interfaces to be as simple as possible so these could be implemented manually (without wizards) by developer with low C++ skill.
* Strive to have experience of new WinRT (*.winmd, C++/CX, WRL).

### Semi- and automatic

* IDL COM ATL Wizards, tlbimp
* https://github.com/mono/CppSharp
* http://clrinterop.codeplex.com
* C++/CLI
* C++/CX, WRL
* SWIG
* CXXI (Linux, gcc)