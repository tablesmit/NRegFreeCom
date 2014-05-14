#include "StdAfx.h"
#include "MyDispatchEventHandler.h"
#include <assert.h>
#include <iostream>

#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 

#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif


MyDispatchEventHandler::MyDispatchEventHandler(void)
    :m_refCount(1)
{
    
}


MyDispatchEventHandler::~MyDispatchEventHandler(void)
{
}


STDMETHODIMP MyDispatchEventHandler::FloatPropertyChanging(float NewValue, bool* Cancel)
{
	*Cancel = true;
    return S_OK;
}

STDMETHODIMP MyDispatchEventHandler::PassStuct(RegFreeCom_Interfaces::MyCoolStuct val)
{
	auto valval = val._Val;
	auto valval2 = val._Val2;
	m_val = val;
    return S_OK;
}

STDMETHODIMP MyDispatchEventHandler::EnsureGCIsNotObstacle()
{
	PassStuct(m_val);
	PassClass(m_obj);
	PassString(m_str);
    return S_OK;
}



STDMETHODIMP MyDispatchEventHandler::PassString(BSTR str)
{
	std::cout << "PassString" << str << std::endl;
	m_str = str;
    return S_OK;
}

STDMETHODIMP MyDispatchEventHandler::PassClass(RegFreeCom_Interfaces::IMyCoolClass* obj)
{
	BSTR val;
	auto mv = obj->GetMyValue2(&val);
	auto hr = obj->get_MyValue(&val);
	auto val2 = obj->MyValue;
	auto val3 = obj->GetMyValue();
	m_obj = obj;
    return S_OK;
}

STDMETHODIMP MyDispatchEventHandler::SimpleEmptyEvent()
{
		std::cout << "SimpleEmptyEvent" << std::endl;
    return S_OK;
}

HRESULT MyDispatchEventHandler::QueryInterface(REFIID riid, void __RPC_FAR *__RPC_FAR *ppvObject)
{
    if (! ppvObject)
    {
        return E_POINTER;
    }

    *ppvObject=0;

    if ((::IsEqualGUID(riid, IID_IUnknown)) ||
        (::IsEqualGUID(riid, IID_IDispatch)) ||
        (::IsEqualGUID(riid, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents))))
    {
        *ppvObject=this;
        AddRef();
        return S_OK;
    }
    return E_NOINTERFACE;
}

ULONG MyDispatchEventHandler::AddRef()
{
    return ++m_refCount;
}


ULONG MyDispatchEventHandler::Release()
{
    --m_refCount;
    ULONG l_refCount=m_refCount;
    if (! m_refCount)
    {
        delete this;
    }

    return l_refCount;
}


HRESULT MyDispatchEventHandler::GetTypeInfoCount(UINT *)
{
    return E_NOTIMPL;
}


HRESULT MyDispatchEventHandler::GetTypeInfo(UINT , LCID , ITypeInfo **)
{
    return E_NOTIMPL;
}


HRESULT MyDispatchEventHandler::GetIDsOfNames(REFIID , LPOLESTR *, UINT , LCID , DISPID *)
{
    return E_NOTIMPL;
}


HRESULT MyDispatchEventHandler::Invoke(DISPID dispIdMember, 
									REFIID riid, 
									LCID lcid, 
									WORD wFlags, 
									DISPPARAMS* pDispParams, 
									VARIANT* pvarResult,
									EXCEPINFO* pexcepinfo, 
									UINT* puArgErr)
{
    if (riid!=IID_NULL)
    {
        return DISP_E_UNKNOWNINTERFACE;
    }

    if (wFlags!=DISPATCH_METHOD)
    {
        return E_INVALIDARG;
    }

    if (! pDispParams)
    {
        return DISP_E_PARAMNOTOPTIONAL;
    }

    if (pDispParams->cNamedArgs!=0)
    {
        return DISP_E_NONAMEDARGS;
    }

    
    switch (dispIdMember)
    {

	case 1:
		{
			if (pDispParams->cArgs!=2)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{
				FloatPropertyChanging(pDispParams->rgvarg[1].fltVal,(bool*)pDispParams->rgvarg[0].pboolVal);

				return S_OK;
			}
		 break;
		}
	 	case 3:
		{
			if (pDispParams->cArgs!=1)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{
				auto myvariant = pDispParams->rgvarg[0].byref;

				RegFreeCom_Interfaces::MyCoolStuct mystuct;
				memcpy(&mystuct,myvariant,sizeof(RegFreeCom_Interfaces::MyCoolStuct));
				PassStuct(mystuct);

				return S_OK;
			}
		 break;
		}
	 case 5:
		{
			if (pDispParams->cArgs!=1)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{
				RegFreeCom_Interfaces::IMyCoolClass* pass; 
				auto hr = pDispParams->rgvarg[0].punkVal->QueryInterface<RegFreeCom_Interfaces::IMyCoolClass>(&pass);
				assert(hr == S_OK);
				PassClass(pass);

				return S_OK;
			}
		 break;
		}
	 case 7:
		{
			if (pDispParams->cArgs!=1)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{

				PassString(pDispParams->rgvarg[0].bstrVal);

				return S_OK;
			}
		 break;
		}
		case 9:
		{
			if (pDispParams->cArgs!=0)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{

				EnsureGCIsNotObstacle();

				return S_OK;
			}
		 break;
		}
		case 11:
		{
			if (pDispParams->cArgs!=0)
			{
				return DISP_E_BADPARAMCOUNT;
			}
			else
			{

				SimpleEmptyEvent();

				return S_OK;
			}
		 break;
		}
		break;
	
    }
	//ignore all other calls
	return S_OK;
}
