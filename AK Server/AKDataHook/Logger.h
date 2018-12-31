#ifndef __LOGGER_H__
#define __LOGGER_H__

class CLog {
public:
	static CLog* GetLogger(const WCHAR *szFileName = NULL);
	void Close();
	void AddLine(const char *line);
	static const WCHAR* GetDefaultLogName();
	const WCHAR* GetPacketLog();
	BOOL AddPacket(LPVOID data, int length);
	void Release();
private:
	CLog(const WCHAR *szFileName);
	~CLog();
	const char* GetCurrentTime();
	const char* GetCurrentDateTime();
	const WCHAR *m_fileName;
	static CLog *m_instance;
	HANDLE m_slotHandle;
};

#endif // __LOGGER_H__
