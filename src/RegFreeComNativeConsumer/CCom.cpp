// CCom.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "CCom.h"
#include <stdio.h>
#include <iostream>

#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 
#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif

using namespace RegFreeCom_Interfaces;

// This is an example of an exported variable
CCOM_API int nCCom=0;


CCOM_API HRESULT Initialize(IUnknown* service)
{

	CoInitialize(NULL);
    RegFreeCom_Interfaces::ISimpleObjectPtr obj = (RegFreeCom_Interfaces::ISimpleObjectPtr)service;
	std::cout << obj->FloatProperty << std::endl;
	RegFreeCom_Interfaces::ISimpleObject* obj2;
	auto result = service->QueryInterface<RegFreeCom_Interfaces::ISimpleObject>(&obj2);
	std::cout << obj2->FloatProperty << std::endl;
	return S_OK;
}

// This is the constructor of a class that has been exported.
// see CCom.h for the class definition
CCCom::CCCom()
{
	return;
}
