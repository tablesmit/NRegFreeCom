// dllmain.h : Declaration of module class.

class CRegFreeComNativeImplementerModule : public ATL::CAtlDllModuleT< CRegFreeComNativeImplementerModule >
{
public :
	DECLARE_LIBID(LIBID_RegFreeComNativeImplementerLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_REGFREECOMNATIVEIMPLEMENTER, "{CD829BD0-82A5-4E78-B9B6-77F0D210916A}")
};

extern class CRegFreeComNativeImplementerModule _AtlModule;
