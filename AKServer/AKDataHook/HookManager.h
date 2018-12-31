#ifndef __HOOKMANAGER_H__
#define __HOOKMANAGER_H__

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include "ImportHook.h"
#include "servermailslot.h"

#pragma warning(disable:4321)
#pragma warning(disable:4312)
#pragma warning(disable:4311)

class CHookManager {
public:
	static CHookManager* GetManager(HMODULE hModule = 0);
	void Release();
	void InitHooks();
	void ShutdownHooks(HMODULE hTarget);
	void HookModule();
	
private:

	#define STATUS_SUCCESS   ((UINT)0x00000000L)
	typedef enum _FINDEX_INFO_LEVELS
	{
	FindExInfoStandard
	} FINDEX_INFO_LEVELS;

	typedef enum _FINDEX_SEARCH_OPS
	{
	FindExSearchNameMatch, 
	FindExSearchLimitToDirectories, 
	FindExSearchLimitToDevices
	} FINDEX_SEARCH_OPS;
	typedef struct _UNICODE_STRING {
	USHORT  Length;
	USHORT  MaximumLength;
	PWSTR   Buffer;
	} UNICODE_STRING, *PUNICODE_STRING;

	typedef struct _STRING {
	USHORT  Length;
	USHORT  MaximumLength;
	PCHAR   Buffer;
	} ANSI_STRING, *PANSI_STRING;

	typedef struct _SYSTEM_THREAD_INFORMATION {
		LARGE_INTEGER KernelTime;
		LARGE_INTEGER UserTime;
		LARGE_INTEGER CreateTime;
		ULONG WaitTime;
		PVOID StartAddress;
		DWORD ClientId;
		DWORD Priority;
		LONG BasePriority;
		ULONG ContextSwitches;
		ULONG ThreadState;
		ULONG WaitReason;
	} SYSTEM_THREAD_INFORMATION, *PSYSTEM_THREAD_INFORMATION;

	typedef struct _SYSTEM_PROCESS_INFORMATION {
		DWORD          NextEntryDelta;           // relative offset
		DWORD          dThreadCount;
		DWORD          dReserved01;
		DWORD          dReserved02;
		DWORD          dReserved03;
		DWORD          dReserved04;
		DWORD          dReserved05;
		DWORD          dReserved06;
		FILETIME       ftCreateTime;     // relative to 01-01-1601
		FILETIME       ftUserTime;       // 100 nsec units
		FILETIME       ftKernelTime;     // 100 nsec units
		UNICODE_STRING ProcessName;
		DWORD          BasePriority;
		DWORD          dUniqueProcessId;
		DWORD          dParentProcessID;
		DWORD          dHandleCount;
		DWORD          dReserved07;
		DWORD          dReserved08;
		DWORD          VmCounters;
		DWORD          dCommitCharge;   // WCHARs
		SYSTEM_THREAD_INFORMATION  ThreadInfos[1];
	} SYSTEM_PROCESS_INFORMATION, *PSYSTEM_PROCESS_INFORMATION;

	typedef enum _SYSTEM_INFORMATION_CLASS {
		SystemBasicInformation = 0,
		SystemPerformanceInformation = 2,
		SystemTimeOfDayInformation = 3,
		SystemProcessInformation = 5,
		SystemProcessorPerformanceInformation = 8,
		SystemInterruptInformation = 23,
		SystemExceptionInformation = 33,
		SystemRegistryQuotaInformation = 37,
		SystemLookasideInformation = 45
	} SYSTEM_INFORMATION_CLASS;

// WARNING: compilers pad structures  - not very safe
// the following pragma prevents that on VC++ 
	#pragma pack(1)
	typedef struct {
		char oneB;
		long fourB;
	} Patch;

	// private instance methods
	CHookManager(HMODULE hModule); // private constructor to deny manual creation!
	char* FindSignature(char *start, int len, short *signature, int signatureLen);
	static void applyPatch( void *pAddr, Patch &patch );
	static DWORD GetFileLength();
	static void ReadMail();
	static void SetCaption();

	// internal hooking functions
	static void PacketChecker(WCHAR Opcode);
	static void GetTCPHookFunc2(DWORD *size, char *pData, WCHAR *opcode, DWORD unk1, DWORD unk2);
	static void GetTCPHookFunc(DWORD *size, char *pData, WCHAR *opcode, DWORD unk1, DWORD unk2);
	static void GetUdpHookFunc(DWORD *size, DWORD unk1, DWORD unk2, char *pData, WCHAR *opcode, DWORD unk3);
	static void GetUdpHookFunc2(DWORD *size, DWORD unk1, DWORD unk2, char *pData, WCHAR *opcode, DWORD unk3);
	static void SendTcpHookFunc(char *pData, WCHAR *opcode, DWORD *size, DWORD unk1, DWORD unk2);
	static void SendUdpHookFunc(char *pData, WCHAR *opcode, DWORD *size, DWORD unk1, DWORD unk2);
	static void PacketHandler(int PacketType, DWORD size, char *pData, WCHAR opcode, DWORD ReturnCode);
	static SIZE_T WINAPI MyVirtualQuery(LPCVOID lpAddress, PMEMORY_BASIC_INFORMATION lpBuffer, SIZE_T dwLength);
	static SIZE_T WINAPI MyVirtualQueryEx(HANDLE hProcess,LPCVOID lpAddress,PMEMORY_BASIC_INFORMATION lpBuffer,SIZE_T dwLength);
	static DWORD WINAPI MyNtQuerySystemInformation(DWORD SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength);
	static LRESULT CALLBACK GetMsgProc(int nCode, WPARAM wParam, LPARAM lParam);
	static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
	static void CreateMessageWindow();

	typedef SIZE_T (WINAPI *MyVirtualQuery_t)(LPCVOID lpAddress, PMEMORY_BASIC_INFORMATION lpBuffer, SIZE_T dwLength);
	typedef SIZE_T (WINAPI *MyVirtualQueryEx_t)(HANDLE hProcess,LPCVOID lpAddress,PMEMORY_BASIC_INFORMATION lpBuffer,SIZE_T dwLength);
	typedef DWORD (WINAPI *NtQuerySystemInformation_t)(DWORD SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength);

	// global class variables
	static SFunctionHook Vquery;
	static SFunctionHook VqueryEx;
	static SFunctionHook QuerySystem;
	static CHookManager *m_instance;
	static HMODULE m_thisModule;
	static Patch getTcpPatch;
	static Patch getUdpPatch;
	static Patch SendTcpPatch;
	static Patch SendUdpPatch;

	static void* pGetTcpOrig;
	static void* pGetUdpOrig;
	static void* pSendTcpOrig;
	static void* pSendUdpOrig;

	static CServerMailSlot *mail;

	static long GetIndexAddress;
	static HWND GUIWnd;
	static const int BUFFER_SIZE = 65536;

};

#endif // __HOOKMANAGER_H__

