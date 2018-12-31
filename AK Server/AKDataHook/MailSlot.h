#ifndef __MAILSLOT_H__
#define __MAILSLOT_H__

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdio.h>

class CMailSlot {
public:
	static CMailSlot* Connect(const WCHAR *name);
	BOOL Write(LPVOID data, int length);
	void Release();
private:
	CMailSlot(const WCHAR *name);
	HANDLE m_slotHandle;
};

#endif // __MAILSLOT_H__
