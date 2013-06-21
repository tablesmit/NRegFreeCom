// dllmain.h : Declaration of module class.

class CRegFreeComNativeInterfacesModule : public ATL::CAtlDllModuleT< CRegFreeComNativeInterfacesModule >
{
public :
	DECLARE_LIBID(LIBID_RegFreeComNativeInterfacesLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_REGFREECOMNATIVEINTERFACES, "{4F95F5C5-CBFB-4634-8556-47419F603EEF}")
};

extern class CRegFreeComNativeInterfacesModule _AtlModule;
