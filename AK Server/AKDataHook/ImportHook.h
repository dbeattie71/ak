#ifndef _IMPORTHOOK_H_INCLUDED
#define _IMPORTHOOK_H_INCLUDED

#define BASE_IMGADDR (void*)GetModuleHandle(NULL)

struct SFunctionHook
{
	char* DLLName;		// Name of the DLL, e.g. "DDRAW.DLL"
    char* FuncName;     // Function name, e.g. "DirectDrawCreateEx".
    void* HookFn;       // Address of your function.
    void* OrigFn;       // Stored by HookAPICalls, the address of the original function.
	void* OrigPtr;		//pointer to the original function
	int Ordinal;
};

void HookImportedFunction(SFunctionHook* Hook, void *ImageBase);
bool UnHookAPICalls( SFunctionHook* Hook, HMODULE hModule);

#endif
