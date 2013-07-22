// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the CCOM_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// CCOM_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CCOM_EXPORTS
#define CCOM_API __declspec(dllexport)
#else
#define CCOM_API __declspec(dllimport)
#endif

// This class is exported from the CCom.dll
class CCOM_API CCCom {
public:
	CCCom(void);
	// TODO: add your methods here.
};

extern CCOM_API int nCCom;

CCOM_API HRESULT STDAPICALLTYPE Initialize(IUnknown* service);
CCOM_API void STDAPICALLTYPE GetStringResult(BSTR* str);

