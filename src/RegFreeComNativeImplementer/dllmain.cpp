// dllmain.cpp : Implementation of DllMain.

#include "stdafx.h"
#include "resource.h"
#include "RegFreeComNativeImplementer_i.h"
#include "dllmain.h"
#include "xdlldata.h"

CRegFreeComNativeImplementerModule _AtlModule;

// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
		wchar_t* dir = (wchar_t*)malloc(255);
		GetDllDirectoryW(255,dir);
#ifdef _MERGE_PROXYSTUB
	if (!PrxDllMain(hInstance, dwReason, lpReserved))
		return FALSE;
#endif
	hInstance;
	return _AtlModule.DllMain(dwReason, lpReserved); 
}


