// NativeLibraryConsumer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NativeLibraryConsumer.h"
#include "..\NativeLibrary\NativeLibrary.h"
#include <comutil.h>
#include <atlsafe.h>

// This is an example of an exported variable
NATIVELIBRARYCONSUMER_API int nNativeLibraryConsumer=0;

CNativeLibrary* uses;

// This is an example of an exported function.
NATIVELIBRARYCONSUMER_API int fnNativeLibraryConsumer(SAFEARRAY** retval)
{
	//sample of returning array from C++
	CComSafeArray<VARIANT>* arr = new CComSafeArray<VARIANT>(2);	
	VARIANT var;
	var.vt= VT_INT;
	var.intVal = 123;
	arr->SetAt(0,var);
	arr->SetAt(1,var);

	
	*retval = *(arr->GetSafeArrayPtr());
	uses = new CNativeLibrary();
	return 42;
}


