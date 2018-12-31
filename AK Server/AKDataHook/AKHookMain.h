#ifndef __AKHOOKMAIN_H__
#define __AKHOOKMAIN_H__

#include "hookmanager.h"
#include "logger.h"
#include "AKDllHideEng.h"

CHookManager *hm = NULL;
HINSTANCE TheDLL;

const WCHAR* GetCurrentProcessName();
BOOL APIENTRY DllMain(HANDLE hModule, DWORD  fdwReason, LPVOID lpReserved);
LRESULT CALLBACK HookProc(int nCode, WPARAM wParam, LPARAM lParam);
void WINAPI InstallHook();
void WINAPI RemoveHook();

#endif //AKHOOKMAIN