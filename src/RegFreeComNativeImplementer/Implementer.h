// Implementer.h : Declaration of the CImplementer

#pragma once
#include "resource.h"       // main symbols
#include <stdio.h>
#include <iostream>
#include <Winbase.h>
#include "..\NativeLibrary\NativeLibrary.h"
#include "RegFreeComNativeImplementer_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CImplementer

class ATL_NO_VTABLE CImplementer :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CImplementer, &CLSID_Implementer>,
	public IDispatchImpl<IImplementer, &IID_IImplementer, &LIBID_RegFreeComNativeImplementerLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IDispatchImpl<ILoadedByManagedImplementedByNative, &__uuidof(ILoadedByManagedImplementedByNative), &LIBID_RegFreeCom_Interfaces, /* wMajor = */ 1>
{
public:
	CImplementer()
	{
				wchar_t* dir = (wchar_t*)malloc(255);
		GetDllDirectoryW(255,dir);
		std::cout << dir << std::endl;
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_IMPLEMENTER)

	DECLARE_NOT_AGGREGATABLE(CImplementer)

	BEGIN_COM_MAP(CImplementer)
		COM_INTERFACE_ENTRY(IImplementer)
		COM_INTERFACE_ENTRY2(IDispatch, ILoadedByManagedImplementedByNative)
		COM_INTERFACE_ENTRY(ILoadedByManagedImplementedByNative)
	END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:




	// ILoadedByManagedImplementedByNative Methods
public:
	STDMETHOD(Set)(long data)
	{
		std::cout << data  <<std::endl;
		return S_OK;
	}
	STDMETHOD(Get)(BSTR * pRetVal)
	{
		wchar_t* dir = (wchar_t*)malloc(255);
		GetDllDirectoryW(255,dir);
		std::cout << dir << std::endl;
		auto uses = new CNativeLibrary();
		
		 *pRetVal = ::SysAllocString(_TEXT("I implemented managed interfaces"));
		return S_OK;
	}
};

OBJECT_ENTRY_AUTO(__uuidof(Implementer), CImplementer)
