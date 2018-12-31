#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <Strsafe.h>
#include <time.h>
#include "logger.h"

//#pragma _CRT_SECURE_NO_DEPRECATE

CLog* CLog::m_instance = 0;

CLog::CLog(const WCHAR *szFileName)
{
	m_fileName = _wcsdup(szFileName);
	FILE *f;
	errno_t err = _wfopen_s(&f,m_fileName, L"a");
	const char *now = GetCurrentDateTime();
	fprintf(f, "\n*** Chat Log Opened: %s\n\n", now);
	fclose(f);
	delete now;

	//m_slotHandle = CreateFile(GetPacketLog(),	// file name
	//			GENERIC_WRITE,					// Only Write Permission
	//			FILE_SHARE_READ,				// required to write to a mailslot
	//			NULL,							// SD
	//			OPEN_ALWAYS,					// Opens the file. fails if file doesn't exist			
	//			FILE_ATTRIBUTE_NORMAL,			// file attributes
	//			NULL							// handle to template file
	//		);
}

CLog::~CLog()
{
	FILE *f;
	_wfopen_s(&f,m_fileName, L"a");
	const char *now = GetCurrentDateTime();
	fprintf(f, "\n*** Chat Log Closed: %s\n\n\n", now);
	fclose(f);
	delete m_fileName;
	delete now;

	/*if (m_slotHandle != INVALID_HANDLE_VALUE)
		CloseHandle(m_slotHandle);*/
}

CLog* CLog::GetLogger(const WCHAR *szFileName)
{
	if (m_instance == NULL) {
		m_instance = new CLog(szFileName);
	}
	return m_instance;
}

void CLog::Close()
{
	m_instance = NULL;
	delete this;
}

void CLog::AddLine(const char *line)
{
	FILE *f;
	_wfopen_s(&f,m_fileName, L"a");
	const char *now = GetCurrentTime();
	fprintf(f, "[%s] %s\n", now, line);
	fclose(f);
	delete now;
}
BOOL CLog::AddPacket(LPVOID data, int length)
{
	DWORD dummySize;
	return WriteFile(m_slotHandle, data, length, &dummySize, NULL); //send data
}
void CLog::Release()
{
	if (m_slotHandle != INVALID_HANDLE_VALUE) 
		CloseHandle(m_slotHandle);
}
const char* CLog::GetCurrentDateTime()
{
	struct tm newtime;
	char *t=new char[30];
	__time64_t aclock;

	time(&aclock);   // Get time in seconds
	_localtime64_s(&newtime,&aclock);   // Convert time to struct tm form
	strftime(t,30,"%a %b %d %H:%M:%S %Y",&newtime);

	return t;
}

const char* CLog::GetCurrentTime()
{
	struct tm newtime;
	char *t=new char[30];
	__time64_t aclock;

	time(&aclock);   // Get time in seconds
	_localtime64_s(&newtime,&aclock);   // Convert time to struct tm form
	strftime(t,20,"%H:%M:%S",&newtime);

	return t;
}

const WCHAR* CLog::GetDefaultLogName()
{
	static WCHAR LogFile[] = L"\0";
	HKEY  hKey;
	DWORD dwType;
	int cchDest = 260;
	TCHAR strDest[260];
	
	if (LogFile[0] == '\0') {
		LONG lResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, (L"Software\\AutoKillerServer\\"), 0, KEY_READ, &hKey);

		StringCchCopy(strDest, MAX_PATH,(L""));
		DWORD dwSize = cchDest * sizeof(WCHAR);
		lResult = RegQueryValueEx(hKey, (L"Log Path"), NULL, &dwType, (BYTE*)strDest, &dwSize);
		strDest[cchDest-1] = 0; // RegQueryValueEx doesn't NULL term if buffer too small
		RegCloseKey(hKey);

		StringCchCopy(LogFile,260,strDest);
	}

	return LogFile;
}
const WCHAR* CLog::GetPacketLog()
{

	static WCHAR LogFile[] = L"\0";
	HKEY  hKey;
	DWORD dwType;
	int cchDest = 260;
	TCHAR strDest[260];

	LONG lResult = RegOpenKeyEx( HKEY_LOCAL_MACHINE,
                                (L"Software\\AutoKillerServer\\"),
                                0, KEY_READ, &hKey );
	
	StringCchCopy( strDest,MAX_PATH, TEXT("") );
	DWORD dwSize = cchDest * sizeof(WCHAR);
	lResult = RegQueryValueEx( hKey, (L"Hook Path"), NULL,
								&dwType, (BYTE*)strDest, &dwSize );
	strDest[cchDest-1] = 0; // RegQueryValueEx doesn't NULL term if buffer too small
	RegCloseKey( hKey );

	StringCchCopy(LogFile,260,strDest);
	
	return LogFile;
}