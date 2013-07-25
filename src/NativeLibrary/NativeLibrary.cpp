// NativeLibrary.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeLibrary.h"


// This is an example of an exported variable
NATIVELIBRARY_API int nNativeLibrary=0;

// This is an example of an exported function.
NATIVELIBRARY_API STDAPI fnNativeLibrary(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see NativeLibrary.h for the class definition
CNativeLibrary::CNativeLibrary()
{
	return;
}

NATIVELIBRARY_API  STDAPI  ReadRegistry()
{
	PHKEY p = NULL;
	auto result = CreateFileW(NULL,0,0,0,0,0,NULL);
	return S_OK;
}




