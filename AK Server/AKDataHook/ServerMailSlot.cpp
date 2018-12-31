#include "servermailslot.h"


CServerMailSlot::CServerMailSlot(const WCHAR *name)
{
	m_slotHandle = CreateMailslot(name, 
        0,                             // no maximum message size 
        MAILSLOT_WAIT_FOREVER,         // no time-out for operations 
        (LPSECURITY_ATTRIBUTES) NULL); // default security
 
}

CServerMailSlot* CServerMailSlot::CreateMailSlot(const WCHAR *name)
{
	WCHAR buff[MAX_PATH];

	StringCchPrintf(buff,MAX_PATH, L"\\\\.\\mailslot\\%s", name);
	CServerMailSlot *ms = new CServerMailSlot(buff);
	if (ms->m_slotHandle != INVALID_HANDLE_VALUE) {
		return ms;
	} else {
		delete ms;
	}
	return NULL;
}
BOOL CServerMailSlot::CheckMail(DWORD* len)
{
	DWORD cbMessage = 0;
	DWORD cMessage = 0;

	BOOL fResult = GetMailslotInfo(m_slotHandle,(LPDWORD)NULL,&cbMessage,&cMessage,(LPDWORD) NULL);

	if (!fResult || cbMessage == MAILSLOT_NO_MESSAGE) 
    { 
       return FALSE; 
    } 

	*len = cbMessage;

	return TRUE;

}
BOOL CServerMailSlot::Read(char* inBuffer,DWORD len)
{
	DWORD cbRead = 0;	
	
	inBuffer[len] = '\0';

	BOOL fResult = ReadFile(m_slotHandle,inBuffer,len,&cbRead,NULL); 

	return fResult;

}

void CServerMailSlot::Release()
{
	if (m_slotHandle != INVALID_HANDLE_VALUE) 
		CloseHandle(m_slotHandle);
	
	delete this;
}
