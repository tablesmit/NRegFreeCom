// NativeLibraryConsumer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeLibraryConsumer.h"
#include "..\NativeLibrary\NativeLibrary.h"

// This is an example of an exported variable
NATIVELIBRARYCONSUMER_API int nNativeLibraryConsumer=0;

CNativeLibrary* uses;
// This is an example of an exported function.
NATIVELIBRARYCONSUMER_API int fnNativeLibraryConsumer()
{
	uses = new CNativeLibrary();
	return 42;
}


