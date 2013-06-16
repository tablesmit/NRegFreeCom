// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NATIVELIBRARYCONSUMER_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NATIVELIBRARYCONSUMER_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NATIVELIBRARYCONSUMER_EXPORTS
#define NATIVELIBRARYCONSUMER_API __declspec(dllexport)
#else
#define NATIVELIBRARYCONSUMER_API __declspec(dllimport)
#endif


#include <comutil.h>
extern NATIVELIBRARYCONSUMER_API int nNativeLibraryConsumer;

NATIVELIBRARYCONSUMER_API int fnNativeLibraryConsumer(SAFEARRAY** retval);
