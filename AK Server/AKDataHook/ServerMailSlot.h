#ifndef __SERVERMAILSLOT_H__
#define __SERVERMAILSLOT_H__

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <strsafe.h>

class CServerMailSlot {
public:
	static CServerMailSlot* CreateMailSlot(const WCHAR *name);
	BOOL Read(char* inBuffer,DWORD len );
	void Release();
	BOOL CheckMail(DWORD* len);
private:
	CServerMailSlot(const WCHAR *name);
	HANDLE m_slotHandle;
};

#endif // __SERVERMAILSLOT_H__
