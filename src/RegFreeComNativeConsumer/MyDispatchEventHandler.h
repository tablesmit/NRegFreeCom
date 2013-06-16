#pragma once

#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 

#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif

#include <atlbase.h>
#include <atlcom.h>


class __declspec(uuid("0492E62E-638B-4C8B-332E-F716C11D4E91")) MyDispatchEventHandler : 
	public RegFreeCom_Interfaces::ISimpleObjectEvents,
	public IDispatch
{
public:
    MyDispatchEventHandler(void);
    virtual ~MyDispatchEventHandler(void);

	//Need to be real COM IDispatch object to subsctibe to events
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void __RPC_FAR *__RPC_FAR *ppvObject);
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();

    virtual HRESULT STDMETHODCALLTYPE GetTypeInfoCount(UINT *pctinfo);
    virtual HRESULT STDMETHODCALLTYPE GetTypeInfo(UINT iTInfo, LCID lcid, ITypeInfo **ppTInfo);
    virtual HRESULT STDMETHODCALLTYPE GetIDsOfNames(REFIID riid, LPOLESTR *rgszNames, UINT cNames, LCID lcid, DISPID *rgDispId);
    virtual HRESULT STDMETHODCALLTYPE Invoke(DISPID dispIdMember,
                                            REFIID riid,
                                            LCID lcid,
                                            WORD wFlags,
                                            DISPPARAMS *pDispParams,
                                            VARIANT *pVarResult,
                                            EXCEPINFO *pExcepInfo,
                                            UINT *puArgErr);



  
    ULONG m_refCount;
   
    STDMETHOD(FloatPropertyChanging)(float NewValue, bool* Cancel);
	STDMETHOD(PassStuct)(RegFreeCom_Interfaces::MyCoolStuct val);
	STDMETHOD(PassClass)(RegFreeCom_Interfaces::IMyCoolClass* obj);
	STDMETHOD(PassString)(BSTR string);
	STDMETHOD(EnsureGCIsNotObstacle)();
	STDMETHOD(SimpleEmptyEvent)();
	
private:
	RegFreeCom_Interfaces::MyCoolStuct m_val;
RegFreeCom_Interfaces::IMyCoolClass* m_obj; 
BSTR m_str;
};

