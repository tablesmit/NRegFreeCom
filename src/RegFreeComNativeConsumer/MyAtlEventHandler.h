#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 

#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif

#include <atlbase.h>
#include <atlcom.h>


__interface __declspec(uuid("7b70c487-b741-4973-b915-b812a91bdf63"))
IMyAtlEventHandler : public IUnknown
{
      
};

const UINT SINK_ID = 234261341; 
class __declspec(uuid("0BFD3967-EC69-487E-8A35-C429CB305E23")) MyAtlEventHandler :
	//public CComObjectRootEx<CComMultiThreadModel>,
    // public CComCoClass<MyAtlEventHandler, &__uuidof(MyAtlEventHandler)>,
     //public IDispatchImpl<IMyAtlEventHandler, &__uuidof(IMyAtlEventHandler), &__uuidof(RegFreeCom_Interfaces::__RegFreeCom_Interfaces)/*,1,2*/>,
	public IDispEventImpl<SINK_ID, MyAtlEventHandler,&__uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents),&__uuidof(RegFreeCom_Interfaces::__RegFreeCom_Interfaces)/*,1,2*/>
{
public:
    MyAtlEventHandler(void);
    virtual ~MyAtlEventHandler(void);
	

    BEGIN_SINK_MAP(MyAtlEventHandler)
		 // SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 9, EnsureGCIsNotObstacle)
		  //SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 11, SimpleEmptyEvent)
    END_SINK_MAP()

  
    ULONG m_refCount;
    static _ATL_FUNC_INFO m_FloatPropertyChangingInfo;
	static _ATL_FUNC_INFO m_PassStringInfo;
	static _ATL_FUNC_INFO m_SimpleEmptyEventInfo;
    STDMETHOD(FloatPropertyChanging)(float NewValue, bool* Cancel);
	STDMETHOD(PassStuct)(RegFreeCom_Interfaces::MyCoolStuct val);
	STDMETHOD(PassClass)(RegFreeCom_Interfaces::IMyCoolClass* obj);
	STDMETHOD(PassString)(BSTR string);	
	STDMETHOD(EnsureGCIsNotObstacle)();
	STDMETHOD(SimpleEmptyEvent)();
	void Subscribe(IUnknown* unk);

private:
	RegFreeCom_Interfaces::MyCoolStuct m_val;
RegFreeCom_Interfaces::IMyCoolClass* m_obj; 
BSTR m_str;
};

