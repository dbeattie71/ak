#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <shlwapi.h> // for PathStripPath
#include <tchar.h> // for _T
#include "hookmanager.h"
#include "logger.h"
#include "mailslot.h"
#include <string>
using namespace std;

CHookManager* CHookManager::m_instance = 0;
HMODULE CHookManager::m_thisModule = 0;

CHookManager::Patch CHookManager::getTcpPatch;
CHookManager::Patch CHookManager::getUdpPatch;
CHookManager::Patch CHookManager::SendTcpPatch;
CHookManager::Patch CHookManager::SendUdpPatch;

void* CHookManager::pGetTcpOrig = 0;
void* CHookManager::pGetUdpOrig = 0;
void* CHookManager::pSendTcpOrig = 0;
void* CHookManager::pSendUdpOrig = 0;

CServerMailSlot *CHookManager::mail=0;

HWND CHookManager::GUIWnd=0;

long CHookManager::GetIndexAddress=0;

SFunctionHook CHookManager::Vquery = {"KERNEL32.DLL","VirtualQuery",CHookManager::MyVirtualQuery,0};
SFunctionHook CHookManager::VqueryEx = {"KERNEL32.DLL","VirtualQueryEx",CHookManager::MyVirtualQueryEx,0};
SFunctionHook CHookManager::QuerySystem = {"Ntdll.dll","NtQuerySystemInformation",MyNtQuerySystemInformation,0};

//NtQueryVirtualMemory

SIZE_T WINAPI CHookManager::MyVirtualQuery(LPCVOID lpAddress, PMEMORY_BASIC_INFORMATION lpBuffer, SIZE_T dwLength)
{
	MyVirtualQuery_t OldFn = (MyVirtualQuery_t)Vquery.OrigFn;

	SIZE_T Result = OldFn(lpAddress, lpBuffer, dwLength);

	if (lpBuffer->AllocationBase == m_thisModule) {
		lpBuffer->State &= ~MEM_COMMIT;
	}

	if( (((DWORD)lpBuffer->BaseAddress) + lpBuffer->RegionSize) == (DWORD)BASE_IMGADDR) 
          lpBuffer->RegionSize += (DWORD)BASE_IMGADDR; 

	return Result;
}
SIZE_T WINAPI CHookManager::MyVirtualQueryEx(HANDLE hProcess,LPCVOID lpAddress,PMEMORY_BASIC_INFORMATION lpBuffer,SIZE_T dwLength)
{
	MyVirtualQueryEx_t OldFn = (MyVirtualQueryEx_t)VqueryEx.OrigFn;
	
	SIZE_T Result = OldFn(hProcess, lpAddress, lpBuffer, dwLength);

	if (lpBuffer->AllocationBase == m_thisModule)
	{
		lpBuffer->State &= ~MEM_COMMIT;
	}

	if( (((DWORD)lpBuffer->BaseAddress) + lpBuffer->RegionSize) == (DWORD)BASE_IMGADDR) 
          lpBuffer->RegionSize += (DWORD)BASE_IMGADDR; 

	return Result;
}
DWORD WINAPI CHookManager::MyNtQuerySystemInformation(DWORD SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength)
{
	PSYSTEM_PROCESS_INFORMATION pSpiCurrent, pSpiPrec;
	WCHAR *pname = NULL;
	DWORD rc;	

	// 1st of all, get the return value of the function
	NtQuerySystemInformation_t OldFn = (NtQuerySystemInformation_t)QuerySystem.OrigFn;
	rc = OldFn(SystemInformationClass, SystemInformation, SystemInformationLength, ReturnLength);
	
	// if sucessfull, perform sorting
	if (rc == STATUS_SUCCESS)
	{
		// system info
		switch (SystemInformationClass)
		{
			// process list 
		case SystemProcessInformation:			
			pSpiCurrent = pSpiPrec = (PSYSTEM_PROCESS_INFORMATION) SystemInformation;
			DWORD pExplorerId = 0;
			
			while (1)
			{				
				// alloc memory to save process name in AINSI 8bits string charset
				pname = (WCHAR *) GlobalAlloc(GMEM_ZEROINIT, pSpiCurrent->ProcessName.Length + 2);
				if (pname == NULL) return (rc); // alloc failed ?
				
				// Convert unicode string to ainsi
				//WideCharToMultiWCHAR(CP_ACP, 0, pSpiCurrent->ProcessName.Buffer, pSpiCurrent->ProcessName.Length + 1, pname, pSpiCurrent->ProcessName.Length + 1, NULL, NULL);

				if( !_wcsnicmp(pname, L"Explorer.EXE", wcslen(L"Explorer.EXE")))
					pExplorerId = pSpiCurrent->dUniqueProcessId;
				
				if(!_wcsnicmp(pname, L"AKServer.Loader.exe", wcslen(L"AKServer.Loader.exe")) || 
					!_wcsnicmp(pname, L"DXMap.exe", wcslen(L"DXMap.exe")) || 
					!_wcsnicmp(pname, L"AutoKillerHunter.exe", wcslen(L"AutoKillerHunter.exe")) || 
					!_wcsnicmp(pname, L"Crafter.exe", wcslen(L"Crafter.exe"))) // if "hidden" process
				{
					if (pname) GlobalFree(pname); // free process's name memory

					if (pSpiCurrent->NextEntryDelta == 0) {
						pSpiPrec->NextEntryDelta = 0;
						break;
					}
					else {
						pSpiPrec->NextEntryDelta += pSpiCurrent->NextEntryDelta;
						pSpiCurrent = (PSYSTEM_PROCESS_INFORMATION) ((PCHAR) pSpiCurrent + pSpiCurrent->NextEntryDelta);
					}
				}
				else
				{
					if (pname) GlobalFree(pname); // free process's name memory

					if (pSpiCurrent->NextEntryDelta == 0) break;
					pSpiPrec = pSpiCurrent;
					
					// Walk the list
					pSpiCurrent = (PSYSTEM_PROCESS_INFORMATION) ((PCHAR) pSpiCurrent + pSpiCurrent->NextEntryDelta);
				}
			} // while
			break;
		} // switch
	} // if
	
	return (rc);
}

CHookManager::CHookManager(HMODULE hModule)
{
	m_thisModule = hModule;
}

CHookManager* CHookManager::GetManager(HMODULE hModule/* = 0*/)
{
	if (m_instance == NULL) {
		m_instance = new CHookManager(hModule);
	}
	return m_instance;
}

void CHookManager::Release()
{
	delete this;
}

void CHookManager::applyPatch( void * pAddr, Patch &patch )
{
	// we must make sure all registers are preserved
	//__asm pusha
	Patch tmp = *(Patch*)pAddr;
	*(Patch*)pAddr = patch;
	patch = tmp;
	//__asm popa

}
DWORD CHookManager::GetFileLength()
{
	static WCHAR strFileName[MAX_PATH];
	GetModuleFileName(GetModuleHandle(NULL), strFileName, sizeof(strFileName));
	
	HANDLE HFile = CreateFile(strFileName,GENERIC_READ,FILE_SHARE_READ|FILE_SHARE_WRITE,NULL,OPEN_EXISTING,0,0);
	if(HFile == INVALID_HANDLE_VALUE)
		return 0;		

	DWORD fileSize = GetFileSize(HFile,NULL);

	CloseHandle(HFile);

	return fileSize;
};

void CHookManager::InitHooks()
{
	char *lpOrg;
	char *baseaddress = (char*)0x400000;
	int SearchLength = GetFileLength();
	bool UseESI = true;
	bool UseGETUDP2 = true;

	short GetTCP1[] = {0x0056, 0x008B, 0x01FF, 0x00E8, 0x01FF, 0x00FD, 0x00FF, 0x00FF, 0x0085, 0x00C0, 0x0075, 0x0002, 0x01FF,  0x00C3};
	short GetTCP2[] = {0x0055, 0x008B, 0x01FF, 0x00E8, 0x01FF, 0x00FD, 0x00FF, 0x00FF, 0x0085, 0x00C0, 0x0075, 0x0002, 0x01FF,  0x00C3};
	
	/*short GetTCP1[] = {0x0056,0x008B,0x00F0,0x00E8,0x01FF,0x01FF,0x01FF,0x01FF,
	0x0085,0x00C0,0x0075,0x0002,0x01FF,0x00C3,0x0033,0x00C0,0x0066};

	short GetTCP2[] = {0x0055,0x008B,0x00EC,0x00E8,0x01FF,0x01FF,0x01FF,0x01FF,
	0x0085,0x00C0,0x0075,0x0002,0x005D,0x00C3,0x0066,0x00A1,0x01FF,0x01FF,0x01FF,0x01FF,0x0056,0x0050,0x00E8,
	0x01FF,0x01FF,0x01FF,0x01FF,0x008B,0x0075,0x0010};

	short GetTCP3[] = {0x0055,0x008B,0x00EC,0x00E8,0x01FF,0x01FF,0x01FF,0x01FF,
	0x0085,0x00C0,0x0075,0x0002,0x005D,0x00C3};*/
	short GetUDP1[] = {0x0055, 0x008B, 0x00EC, 0x0081, 0x00EC, 0x0000, 0x0001, 0x0000, 0x0000, 0x008B, 0x004D, 0x0008, 0x0056, 0x008D, 0x0034, 0x0008, 0x000F };
	short GetUDP2[] = {0x0055, 0x008B, 0x00EC, 0x00B8, 0x0000, 0x0001, 0x0000, 0x0000, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x008B, 0x0045, 0x0008 };
		
	short SendTCP1[] = {0x0055, 0x008B, 0x00EC, 0x00B8, 0x01FF, 0x0010, 0x0000, 0x0000, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x0083, 0x003D };
	short SendTCP2[] = {0x0055, 0x008B, 0x00EC, 0x00B8, 0x01FF, 0x0011, 0x0000, 0x0000, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x0083, 0x003D };

	short SendUDP1[] = {0x0055, 0x008B, 0x00EC, 0x0081, 0x00EC, 0x000C, 0x0008, 0x0000, 0x0000, 0x0083, 0x003D, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x0002, 0x0074, 0x0004, 0x0033, 0x00C0, 0x00C9, 0x00C3};
	short SendUDP2[] = {0x0055, 0x008B, 0x00EC, 0x00B8, 0x000C, 0x0008, 0x0000, 0x0000, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x0083, 0x003D, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x0002, 0x0074, 0x0004, 0x0033, 0x00C0, 0x00C9, 0x00C3};

	short ID1Index[] = {0x0039, 0x01FF, 0x00F0, 0x0000, 0x0000, 0x0000, 0x0074, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x008B, 0x00F8};
	short ID2Index[] = {0x0039, 0x01FF, 0x00F0, 0x0000, 0x0000, 0x0000, 0x0074, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x00E8, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x008B, 0x00F0};

	//GETTCP
	lpOrg = FindSignature(baseaddress,SearchLength,GetTCP1,sizeof(GetTCP1)/sizeof(GetTCP1[0]));
	if (lpOrg == NULL)
	{
		lpOrg = FindSignature(baseaddress,SearchLength,GetTCP2,sizeof(GetTCP2)/sizeof(GetTCP2[0]));
		UseESI = false;
	}
	
	if (lpOrg != NULL)
	{
		if (UseESI)
		{
			getTcpPatch.oneB = '\xE9';
			getTcpPatch.fourB = (long)((char*)GetTCPHookFunc - lpOrg - 5);
		}
		else
		{
			getTcpPatch.oneB = '\xE9';
			getTcpPatch.fourB = (long)((char*)GetTCPHookFunc2 - lpOrg - 5);
		}		

		DWORD dwOldProtect;
		VirtualProtect((LPVOID)lpOrg, 5, PAGE_EXECUTE_WRITECOPY, &dwOldProtect);//do this once

		applyPatch( (void*)lpOrg, getTcpPatch );
		pGetTcpOrig = (void*)lpOrg;

		#ifdef _DEBUG
			WCHAR tmp[100];
			wsprintf(tmp,L"lpOrg ret %d",(int)lpOrg);
			MessageBox(NULL, tmp, L"Get TCP Hook Point Attach Debugger now",MB_OK);
		#endif	                			
	}
	else
	{
		MessageBox(NULL, L"Could not find GetTCP, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
        TerminateProcess(GetCurrentProcess(),0);
	}
	
	//GETUDP
    lpOrg = FindSignature(baseaddress,SearchLength,GetUDP1,sizeof(GetUDP1)/sizeof(GetUDP1[0]));

	if (lpOrg == NULL)
	{
		lpOrg = FindSignature(baseaddress,SearchLength,GetUDP2,sizeof(GetUDP2)/sizeof(GetUDP2[0]));
		UseGETUDP2=false;
	}

	if (lpOrg != NULL)
	{
		if (UseGETUDP2)
		{
			getUdpPatch.oneB = '\xE9';
			getUdpPatch.fourB = (long)((char*)GetUdpHookFunc - lpOrg - 5);
		}
		else
		{
			getUdpPatch.oneB = '\xE9';
			getUdpPatch.fourB = (long)((char*)GetUdpHookFunc2 - lpOrg - 5);
		}		
		
		DWORD dwOldProtect;
		VirtualProtect((LPVOID)lpOrg, 5, PAGE_EXECUTE_WRITECOPY, &dwOldProtect);//do this once

		applyPatch( (void*)lpOrg, getUdpPatch );
		pGetUdpOrig = (void*)lpOrg;
	}
	else
	{
		MessageBox(NULL, L"Could not find GetUDP, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
        TerminateProcess(GetCurrentProcess(),0);
	}
	
	//SendTCP
	lpOrg = FindSignature(baseaddress,SearchLength,SendTCP1,sizeof(SendTCP1)/sizeof(SendTCP1[0]));

	if (lpOrg == NULL)
	{
		lpOrg = FindSignature(baseaddress,SearchLength,SendTCP2,sizeof(SendTCP2)/sizeof(SendTCP2[0]));
	}
	if (lpOrg != NULL)
	{
		SendTcpPatch.oneB = '\xE9';
		SendTcpPatch.fourB = (long)((char*)SendTcpHookFunc - lpOrg - 5);

		DWORD dwOldProtect;
		VirtualProtect((LPVOID)lpOrg, 5, PAGE_EXECUTE_WRITECOPY, &dwOldProtect);//do this once

		applyPatch( (void*)lpOrg, SendTcpPatch );
		pSendTcpOrig = (void*)lpOrg;
	}
	else
	{
		MessageBox(NULL, L"Could not find SendTCP, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
        TerminateProcess(GetCurrentProcess(),0);
	}

	//SendUdp
	lpOrg = FindSignature(baseaddress,SearchLength,SendUDP1,sizeof(SendUDP1)/sizeof(SendUDP1[0]));

	if (lpOrg == NULL)
		lpOrg = FindSignature(baseaddress,SearchLength,SendUDP2,sizeof(SendUDP2)/sizeof(SendUDP2[0]));

	if (lpOrg != NULL)
	{
		SendUdpPatch.oneB = '\xE9';
		SendUdpPatch.fourB = (long)((char*)SendUdpHookFunc - lpOrg - 5);

		DWORD dwOldProtect;
		VirtualProtect((LPVOID)lpOrg, 5, PAGE_EXECUTE_WRITECOPY, &dwOldProtect);//do this once

		applyPatch( (void*)lpOrg, SendUdpPatch );
		pSendUdpOrig = (void*)lpOrg;
	}
	else
	{
		MessageBox(NULL, L"Could not find SendUDP, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
        TerminateProcess(GetCurrentProcess(),0);
	}

	lpOrg = FindSignature(baseaddress,SearchLength, ID1Index,sizeof(ID1Index)/sizeof(ID1Index[0]));

	if (lpOrg != NULL)
	{
		int OffSet = ((int)*(lpOrg+15) & 0x000000FF) | ((int)*(lpOrg+16)*256 & 0x0000FFFF) | ((int)*(lpOrg+17)*256*256 & 0x00FFFFFF) | (int)*(lpOrg+18)*256*256*256;
		GetIndexAddress = (long)lpOrg+19+OffSet;

		#ifdef _DEBUG
			WCHAR tmp[100];
			wsprintf(tmp,L"GetIndexAddress ret %d",(int)GetIndexAddress);
			MessageBox(NULL, tmp, L"GetIndexAddress",MB_OK);
		#endif	
	}
	else
	{
		lpOrg = FindSignature(baseaddress,SearchLength, ID2Index,sizeof(ID2Index)/sizeof(ID2Index[0]));

		if (lpOrg != NULL)
		{
			int OffSet = ((int)*(lpOrg+15) & 0x000000FF) | ((int)*(lpOrg+16)*256 & 0x0000FFFF) | ((int)*(lpOrg+17)*256*256 & 0x00FFFFFF) | (int)*(lpOrg+18)*256*256*256;
			GetIndexAddress = (long)lpOrg+19+OffSet;
		}
		else
		{
			MessageBox(NULL, L"Could not find GetIndex Address, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
			TerminateProcess(GetCurrentProcess(),0);
		}		
	}

	mail = CServerMailSlot::CreateMailSlot(L"AKPACKET");

	CreateMessageWindow();

}
void CHookManager::CreateMessageWindow()
{

	WNDCLASSEX wc;

	memset(&wc, 0, sizeof(WNDCLASSEX));
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = CS_HREDRAW | CS_VREDRAW | CS_NOCLOSE;
	wc.lpfnWndProc = WndProc;
	wc.hInstance = m_thisModule;
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = (HBRUSH) GetStockObject(LTGRAY_BRUSH);
	wc.lpszClassName = L"AKMSGWINDOW";
	
	RegisterClassEx(&wc);

	/* create the window */
	GUIWnd = CreateWindowEx(WS_EX_WINDOWEDGE | WS_EX_CONTROLPARENT,
							L"AKMSGWINDOW", L"AKMSGWINDOW", CW_USEDEFAULT,
							CW_USEDEFAULT, CW_USEDEFAULT, 50, 50, NULL, NULL, m_thisModule, NULL);

}
LRESULT CALLBACK CHookManager::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	//int wmId, wmEvent;
	int pIndex = 0;

	//wmId    = LOWORD(wParam); 
	//wmEvent = HIWORD(wParam); 

	switch (message) 
	{
		case WM_USER+76:
			__asm
				{
					mov edi, wParam
					call GetIndexAddress
					mov pIndex, eax
				}

				return pIndex;
			
			break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}
void CHookManager::ReadMail()
{
	DWORD len;

	if (mail != NULL) 
	{
		if (mail->CheckMail(&len))
		{
			char* buf = new char[len];
			mail->Read(buf, len);	
			/*BlockPacket((unsigned char*)buf);*/
		}
	}
}
void CHookManager::ShutdownHooks(HMODULE hTarget)
{
	__try 
     { 
		if (!GetCurrentProcess() == NULL)
		{
			DestroyWindow(GUIWnd);//destroy message window
			applyPatch( pGetTcpOrig, getTcpPatch );
			applyPatch( pGetUdpOrig, getUdpPatch );
			applyPatch( pSendTcpOrig, SendTcpPatch );
			applyPatch( pSendUdpOrig, SendUdpPatch );

			UnHookAPICalls(&Vquery, hTarget);//unhook API hook
		}

     }  
     __except(EXCEPTION_EXECUTE_HANDLER) 
     { 
		
     } 
}
void CHookManager::HookModule()
{
	HookImportedFunction(&Vquery, BASE_IMGADDR);
	HookImportedFunction(&QuerySystem, BASE_IMGADDR);
	HookImportedFunction(&VqueryEx, BASE_IMGADDR);
}
void CHookManager::PacketChecker(WCHAR Opcode)
{
	if (Opcode == L'\x37')
	{
		MessageBox(NULL, L"AutoKiller blocking packet, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
        TerminateProcess(GetCurrentProcess(),0);
	}
}
void CHookManager::PacketHandler(int PacketType, DWORD size, char *pData, WCHAR opcode, DWORD ReturnCode) 
{
	if (ReturnCode == 0 || size == 0)
		return;
		
	try 
	{
		if ( PacketType == 1 || PacketType == 2)//gettcp getudp
		{
			DWORD LogUpdate = 175; 
			if (opcode == LogUpdate)
			{
				CLog *log = CLog::GetLogger();

				char *ln;
				ln = pData + 8;
				log->AddLine(ln);
			}

			CMailSlot *mail = CMailSlot::Connect(L"SERVERPACKET");
			if (mail != NULL) 
			{
				char *packet = new char[(int)size + 1]; 
				memcpy(packet+1, pData, (int)size); 
				memcpy(packet, &opcode, 1);

				mail->Write(packet, (int)size+1);
				mail->Release();
			}
		}
		else//sendtcp sendudp
		{
			CMailSlot *mail = CMailSlot::Connect(L"CLIENTPACKET");
			if (mail != NULL) 
			{
				char *packet = new char[(int)size + 1]; 
				memcpy(packet+1, pData, (int)size); 
				memcpy(packet, &opcode, 1);

				mail->Write(packet, (int)size+1);
				mail->Release();
			}	
		}
	}
	catch(...) // Handle all exceptions
	{  
		MessageBox(NULL, L"AutoKiller exception raised and caught, exiting game", L"AutoKiller Error", MB_OK | MB_TOPMOST);
		TerminateProcess(GetCurrentProcess(),0);			
	}
}
//use with push    esi	56
__declspec (naked) void CHookManager::GetTCPHookFunc(DWORD *size, char *pData, WCHAR *opcode, DWORD unk1, DWORD unk2)
{ 
	__asm
        {
                push ebp
                mov ebp, esp
                sub esp,8
				pusha		// 8 32bit registers in the stack, esp += 8*4
                mov [ebp-8],eax
        }
        applyPatch( pGetTcpOrig, getTcpPatch );

        __asm 
        {
                popa
                mov eax, [ebp-8]
                push [ebp+20]
                push [ebp+16]
                push [ebp+12]
                push [ebp+8]
                call pGetTcpOrig
                add  esp, 10h		// clean 4 parameters each parameter is 4
                mov [ebp-4],eax
				pusha
				push    eax			//save these
				push    esi
        }

        applyPatch( pGetTcpOrig, getTcpPatch );

        __asm
        {		//packettype, size, packet, opcode, returncode
				push [ebp-4] //check value

				mov     ecx, [ebp+12] //opcode
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				push    edx	//push opcode	

				push [ebp+8]	//packet

				mov     ecx, [ebp-8] //size
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				push    edx	//push size	

				push 1				// 1 for gettcp
                call PacketHandler
                add esp, 14h		// clean 5 parameters each parameter is 4
        }

        __asm
        {
				pop    esi			//restore these
				pop    eax			
                popa
                mov esp, ebp
                pop ebp
				retn
        }
}
//use with push    ebp	55
__declspec (naked) void CHookManager::GetTCPHookFunc2(DWORD *size, char *pData, WCHAR *opcode, DWORD unk1, DWORD unk2)
{ 
	__asm
        {
                push ebp
                mov ebp, esp
                push    ecx
				push    esi
				mov     [ebp-4], 0
				push    eax
				push    esi
        }
        applyPatch( pGetTcpOrig, getTcpPatch );

        __asm 
        {
                pop     esi
				pop     eax

                push [ebp+24]
                push [ebp+20]
                push [ebp+16]
                push [ebp+12]
				push [ebp+8]
                call pGetTcpOrig
                add  esp, 14h		// clean 4 parameters each parameter is 4
                mov [ebp-4],eax
				push    eax			//save these
				push    esi
        }

        applyPatch( pGetTcpOrig, getTcpPatch );

        __asm
        {		//packettype, size, packet, opcode, returncode
				push [ebp-4] //check value

				mov     ecx, [ebp+12] //opcode
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				push    edx	//push opcode	

				push [ebp+8]	//packet
			
				mov     ecx, [ebp+16] //size	
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				push    edx	//push size

				push 1				// 1 for gettcp		first
                call PacketHandler
                add esp, 14h		// clean 5 parameters each parameter is 4
        }

        __asm
        {
				pop     esi	//restore
				pop     eax
				mov     esp, ebp
                pop ebp
				retn
        }
}
__declspec (naked) void CHookManager::SendTcpHookFunc(char *pData, WCHAR *opcode, DWORD *size, DWORD unk1, DWORD unk2)
{ 
	__asm
        {
                push    ebp
				mov     ebp, esp
				push    ecx
				push    esi
				push    edi
				mov     edi, [ebp+12]
				push    edi
				call PacketChecker
				add esp, 4h		// clean 1 parameters each parameter is 4

				push    eax
				push    esi
        }

		applyPatch( pSendTcpOrig, SendTcpPatch );

        __asm
        {
				pop     esi
				pop     eax

				push [ebp+24] 
				push [ebp+20] 
				push [ebp+16]
				push [ebp+12]
				push [ebp+8]
                call pSendTcpOrig
                add esp, 14h		// clean 5 parameters each parameter is 4

				mov     [ebp-4], eax
				push	eax
				push    esi
        }

		applyPatch( pSendTcpOrig, SendTcpPatch );

        __asm
        {
				pop		esi			//restore these
				pop		eax
				mov     esi, [ebp-4]//check here

                mov     edx, [ebp+8]
				mov     eax, [ebp+16]
				push	esi //return code		last 

				push    edi	//opcode
				push    edx	//packet			
				push    eax	//size
				push    3	//first
				call	PacketHandler
				add     esp, 14h	// clean 5 parameters each parameter is 4

				pop		edi			//leaving
				mov     eax, esi
				pop     esi
				mov     esp, ebp
				pop     ebp
				retn
        }
}
__declspec (naked) void CHookManager::SendUdpHookFunc(char *pData, WCHAR *opcode, DWORD *size, DWORD unk1, DWORD unk2)
{ 
		__asm
        {
                push    ebp
				mov     ebp, esp
				push    ecx
				push    esi
				push    edi
				mov     edi, [ebp+12]
				push    edi
				call PacketChecker
				add esp, 4h		// clean 1 parameters each parameter is 4

				push    eax
				push    esi
        }

		applyPatch( pSendUdpOrig, SendUdpPatch );

        __asm
        {
				pop     esi
				pop     eax

				push [ebp+24] 
				push [ebp+20] 
				push [ebp+16]
				push [ebp+12]
				push [ebp+8]
                call pSendUdpOrig
                add esp, 14h		// clean 5 parameters each parameter is 4

				mov     [ebp-4], eax
				push	eax
				push    esi
        }

		applyPatch( pSendUdpOrig, SendUdpPatch );

        __asm
        {
				pop		esi			//restore these
				pop		eax
				mov     esi, [ebp-4]//check here

                mov     edx, [ebp+8]
				mov     eax, [ebp+16]
				push	esi //return code		last 
				push    edi	//opcode
				push    edx	//packet			
				push    eax	//size
				push    4	//first
				call	PacketHandler
				add     esp, 14h	// clean 5 parameters each parameter is 4

				pop		edi			//leaving
				mov     eax, esi
				pop     esi
				mov     esp, ebp
				pop     ebp
				retn
        }
}
//use with sub     esp, 100h
__declspec (naked) void CHookManager::GetUdpHookFunc(DWORD *size, DWORD unk1, DWORD unk2, char *pData, WCHAR *opcode, DWORD unk3)
{ 
	__asm
        {
                push ebp
                mov ebp, esp
                sub esp,8
				//pusha		// 8 32bit registers in the stack, esp += 8*4
				push    eax
				push    esi
        }
        applyPatch( pGetUdpOrig, getUdpPatch );
	
        __asm 
        {
				pop     esi
				pop     eax
                //popa
                push [ebp+24]
                push [ebp+20]
                push [ebp+16]
                push [ebp+12]
                push [ebp+8]
                call pGetUdpOrig
                add  esp, 14h		// clean 5 parameters each parameter is 4
				mov [ebp-4],eax
				pusha
				push    eax			//save these
				push    esi
        }

        applyPatch( pGetUdpOrig, getUdpPatch );

        __asm
        {
				mov     eax, [ebp-4]//check 
				push	eax //last

				mov     eax, [ebp+16]//packet
				mov     ecx, [ebp+20]//opcode
				
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				
				push    edx	//push opcode
				push    eax	//packet

				push    dword ptr [ebx] //size
				
				push 2			//first	
                call PacketHandler
                add esp, 14h		// clean 5 parameters each parameter is 4
        }

        __asm
        {		
				pop     esi
				pop     eax
                popa
                mov esp, ebp
                pop ebp
				retn
        }
}
//use with mov     eax, 100h
__declspec (naked) void CHookManager::GetUdpHookFunc2(DWORD *size, DWORD unk1, DWORD unk2, char *pData, WCHAR *opcode, DWORD unk3)
{ 
	__asm
        {
                push ebp
                mov ebp, esp
              	push    ecx
				push    esi
				//2710 in ecx size?
				mov     [ebp-4], 0
				push    eax
				push    esi
        }
        applyPatch( pGetUdpOrig, getUdpPatch );
	
        __asm 
        {
				pop     esi
				pop     eax
                //popa
                push [ebp+36]
                push [ebp+32]
                push [ebp+28]
                push [ebp+24]
                push [ebp+20]
				push [ebp+16]
				push [ebp+12]
				push [ebp+8]
                call pGetUdpOrig
                add  esp, 14h		// clean 5 parameters each parameter is 4
				mov [ebp-4],eax
				pusha
				push    eax			//save these
				push    esi
        }

        applyPatch( pGetUdpOrig, getUdpPatch );

        __asm
        {
				mov     eax, [ebp-4]//check 
				push	eax //last

				mov     eax, [ebp+20]//packet
				mov     ecx, [ebp+24]//opcode
				
				mov     edx, [ecx] //moves the data into edx, no longer a pointer basically
				
				push    edx	//push opcode
				push    eax	//packet

				mov     eax, [ebp+28]//size
				mov     ecx, [eax]//size
				push    ecx //size
				
				push 2			//first	
                call PacketHandler
                add esp, 14h		// clean 5 parameters each parameter is 4
        }

        __asm
        {		
				pop     esi
				pop     eax
                popa
                mov esp, ebp
                pop ebp
				retn
        }
}
char* CHookManager::FindSignature(char *start, int len, short *signature, int signatureLen)
{
     len -= signatureLen;
	 int i=0;
     while(len > 0)
     {
           if( *start == (char)(signature[0]& 0xFF) )
           {
               for(i=0;i<signatureLen;i++)
               {
                       if( signature[i]< 256 )
                       {
                           if( start[i]!=(char)(signature[i]& 0xFF))
							   break;
                       }
               }
               if( i == signatureLen ) 
				   break;
           }
           start++;
           len--;
     }
     if(len == 0) return NULL; else return start;
}

//GetTCP(pointer to length,packet,pointer to opcode,unk1,unk2)  first parameter in eax others are in stack
//GetUDP(pointer to length,unk,unk,packet,pointer to opcode,unk)  first parameter in eax others are in stack
//SendTCP(packet,opcode,length,unk,unk) all parameters are in stack
//SendUDP(packet,opcode,length,unk,unk) all parameters are in stack