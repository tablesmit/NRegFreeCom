#if _DEBUG
#import "..\..\build\Debug\RegFreeCom.Interfaces.tlb" 

#else 
#import "..\..\build\Release\RegFreeCom.Interfaces.tlb" 
#endif

#include <atlbase.h>
#include <atlcom.h>




const UINT SINK_ID = 234261341; 
class __declspec(uuid("0BFD3967-EC69-487E-8A35-C429CB305E23")) MyAtlEventHandler : 
	public IDispEventImpl<SINK_ID, MyAtlEventHandler,&__uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents),&__uuidof(RegFreeCom_Interfaces::__RegFreeCom_Interfaces)/*,1,2*/>
{
public:
    MyAtlEventHandler(void);
    virtual ~MyAtlEventHandler(void);





    BEGIN_SINK_MAP(MyAtlEventHandler)
        SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000001, FloatPropertyChanging,m_FloatPropertyChangingInfo)
		  SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000003, FloatPropertyChanging,m_FloatPropertyChangingInfo)
		SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000005, FloatPropertyChanging,m_FloatPropertyChangingInfo)

		  SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000007, PassString,m_PassStringInfo)
			SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000009, FloatPropertyChanging,m_FloatPropertyChangingInfo)
		  SINK_ENTRY_EX(SINK_ID, __uuidof(RegFreeCom_Interfaces::ISimpleObjectEvents), 0x00000011, SimpleEmptyEvent,m_SimpleEmptyEventInfo)
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

private:
	RegFreeCom_Interfaces::MyCoolStuct m_val;
RegFreeCom_Interfaces::IMyCoolClass* m_obj; 
BSTR m_str;
};

