 // CCom.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "CCom.h"
#include <stdio.h>
#include <iostream>
#include "MyDispatchEventHandler.h"
#include "MyAtlEventHandler.h"
#include <comutil.h>

#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 
#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif

using namespace RegFreeCom_Interfaces;
using namespace ATL;

// can subscribe custom object withou using ATL (copy paste atlcom.h)
static HRESULT STDAPICALLTYPE  MyEventAdvise(
	_Inout_ IUnknown* pUnkCP,
	_Inout_opt_ IUnknown* pUnk,
	_In_ const IID& iid,
	_Out_ LPDWORD pdw)
{
	if(pUnkCP == NULL)
		return E_INVALIDARG;

	CComPtr<IConnectionPointContainer> pCPC;
	CComPtr<IConnectionPoint> pCP;
	HRESULT hRes = pUnkCP->QueryInterface(__uuidof(IConnectionPointContainer), (void**)&pCPC);
	if (SUCCEEDED(hRes))
		hRes = pCPC->FindConnectionPoint(iid, &pCP);
	if (SUCCEEDED(hRes))
		hRes = pCP->Advise(pUnk, pdw);
	return hRes;
}

CCOM_API void STDAPICALLTYPE GetStringResult(BSTR* str){
   *str =  ::SysAllocString(_TEXT("C string"));
}


CCOM_API  HRESULT STDAPICALLTYPE	Initialize(IUnknown* service)
{

	CoInitialize(NULL);
    RegFreeCom_Interfaces::ISimpleObjectPtr obj = (RegFreeCom_Interfaces::ISimpleObjectPtr)service;


	MyDispatchEventHandler* myHandler = new MyDispatchEventHandler();
	
	//TODO: add pure IUnknown subscriber like in
	//http://www.codeproject.com/Articles/25524/Sinking-events-from-managed-code-in-unmanaged-C#_rating
    
	// COM observer pattern
	DWORD cookie;
	IUnknown* cp;
	myHandler->QueryInterface(IID_IUnknown,(void**)&cp);
	MyEventAdvise(obj,cp,__uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents),&cookie);

	// custom observer patter
	//obj->PutRefCallbacks((RegFreeCom_Interfaces::ISimpleObjectEvents*)myHandler);

	//TODO: fix ATL
	// ATL over COM observer pattern
	//MyAtlEventHandler* atlHandler = new MyAtlEventHandler();	
	//HRESULT hr = atlHandler->DispEventAdvise(service);
		
	//std::cout << obj->GetFloatProperty() << std::endl;
	obj->RaiseEmptyEvent();
	//hr = atlHandler->DispEventAdvise(service);
	
	obj->PutFloatProperty(100);
	obj->RaisePassString();
	obj->RaisePassClass();
	obj->RaisePassStruct();
	obj->RaisePassStruct();
	RegFreeCom_Interfaces::ISimpleObject* obj2;
	auto result = service->QueryInterface<RegFreeCom_Interfaces::ISimpleObject>(&obj2);
	std::cout << obj2->GetFloatProperty() << std::endl;
	return S_OK;
}

// This is the constructor of a class that has been exported.
// see CCom.h for the class definition
CCCom::CCCom()
{
	return;
}
