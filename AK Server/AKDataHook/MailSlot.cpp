#include "mailslot.h"

CMailSlot::CMailSlot(const WCHAR *name)
{
	m_slotHandle = CreateFile(name,				// file name
				GENERIC_WRITE,					// Only Write Permission
				FILE_SHARE_READ,				// required to write to a mailslot
				(LPSECURITY_ATTRIBUTES)NULL,	// SD
				OPEN_EXISTING,					// Opens the file. fails if file doesn't exist			
				FILE_ATTRIBUTE_NORMAL,			// file attributes
				(HANDLE)NULL					// handle to template file
			);
}

CMailSlot* CMailSlot::Connect(const WCHAR *name)
{
	WCHAR buff[MAX_PATH];

	swprintf_s(buff,MAX_PATH, L"\\\\.\\mailslot\\%s", name);
	CMailSlot *ms = new CMailSlot(buff);
	if (ms->m_slotHandle != INVALID_HANDLE_VALUE) {
		return ms;
	} else {
		delete ms;
	}
	return NULL;
}

BOOL CMailSlot::Write(LPVOID data, int length)
{
	DWORD dummySize;
	return WriteFile(m_slotHandle, data, length, &dummySize, NULL); //send data
}

void CMailSlot::Release()
{
	if (m_slotHandle != INVALID_HANDLE_VALUE) 
		CloseHandle(m_slotHandle);
	
	delete this;
}
