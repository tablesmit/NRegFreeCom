// Implementer.h : Declaration of the CImplementer

#pragma once
#include "stdafx.h"
#include "resource.h"       // main symbols
#include <stdio.h>
#include <iostream>
#include <Winbase.h>
#include <comutil.h>
#include <Unknwn.h>
#include <stdio.h>

#import "RegFreeComNativeInterfaces.tlb"
//#include "regfreecomnativeinterfaces.h"
//using namespace RegFreeComNativeInterfacesLib;


class MyUnk : public RegFreeComNativeInterfacesLib::IMyService
{
public:

     MyUnk()
     : m_counter( 0 )
     {}

	 HRESULT STDMETHODCALLTYPE QueryInterface( 
		/* [in] */ REFIID riid,
		/* [iid_is][out] */ __RPC__deref_out void __RPC_FAR *__RPC_FAR *ppvObject){
			if (IsEqualIID(riid,__uuidof(IMyService)) )
			{
				*ppvObject = static_cast<IMyService*>(this);
				return S_OK;
			}
			if (IsEqualIID(riid,IID_IUnknown))
			{
				*ppvObject = static_cast<IUnknown*>(this);
				return S_OK;
			}
			*ppvObject = NULL;
			return E_NOINTERFACE;
	};

	 ULONG STDMETHODCALLTYPE AddRef( void){
		return ++m_counter;
	};

	 ULONG STDMETHODCALLTYPE Release( void){
		return --m_counter;
	};



	 	 HRESULT STDMETHODCALLTYPE raw_GetSomeHandle( 
		/* [out] */ void **someHandle) {
			//VARIANT variant;
			//variant.vt= VT_BYREF;
			//variant.byref = this;//just for test
			*someHandle = this;
			return S_OK;
	}
private:
	ULONG m_counter;
};
