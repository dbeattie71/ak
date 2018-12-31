// AKDataHook.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "AKHookMain.h"
#include <shlwapi.h>

#pragma data_seg (".HookSection")		
HHOOK hHook = NULL; // Shared instance for all processes.
#pragma data_seg ()


#ifdef _MANAGED
#pragma managed(push, off)
#endif


BOOL APIENTRY DllMain(HANDLE hModule, DWORD  fdwReason, LPVOID lpReserved)
{
	TheDLL = (HINSTANCE)hModule;
	const WCHAR *szProcessName = NULL;

	switch (fdwReason) {
		case DLL_PROCESS_ATTACH:
			DisableThreadLibraryCalls(TheDLL);
			szProcessName = GetCurrentProcessName();
			if (_wcsicmp(szProcessName, L"game.dll") == 0 || _wcsicmp(szProcessName, L"tgame.dll") == 0) 
			{
				int x = HideDll("AKDataHook.dll",true);
				hm = CHookManager::GetManager(TheDLL);
				hm->HookModule();
				hm->InitHooks();
				CLog::GetLogger(CLog::GetDefaultLogName());
			}
			break;
		case DLL_PROCESS_DETACH:
			if (!hm == NULL)
			{
				hm->ShutdownHooks(TheDLL);
				hm->Release();
				CLog::GetLogger()->Close();
			}			
			break;
	}

	return TRUE;
}
void WINAPI InstallHook()
{
	hHook = SetWindowsHookEx(WH_SHELL, HookProc, TheDLL, 0); 
}
LRESULT CALLBACK HookProc(int nCode, WPARAM wParam, LPARAM lParam) 
{
	return CallNextHookEx(hHook, nCode, wParam, lParam); 
}
void WINAPI RemoveHook()
{
	UnhookWindowsHookEx(hHook);
}
const WCHAR* GetCurrentProcessName()
{
	static WCHAR szName[MAX_PATH];
	GetModuleFileName(GetModuleHandle(NULL), szName, sizeof(szName));
        PathStripPath(szName);
	return szName;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

