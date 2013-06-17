// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NATIVELIBRARY_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NATIVELIBRARY_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NATIVELIBRARY_EXPORTS
#define NATIVELIBRARY_API __declspec(dllexport) 
#else
#define NATIVELIBRARY_API __declspec(dllimport)
#endif

// This class is exported from the NativeLibrary.dll
class NATIVELIBRARY_API CNativeLibrary {
public:
	CNativeLibrary(void);
	// TODO: add your methods here.
};

extern NATIVELIBRARY_API int nNativeLibrary;

NATIVELIBRARY_API  int  fnNativeLibrary(void);
