#include <windows.h>
#include "ImportHook.h"


void HookImportedFunction(SFunctionHook* Hook, void *ImageBase)
{
	//const char *Dll, const char *FuncName, int Ordinal, void *Function)
    DWORD oldProtect = 0;
	void *PrevValue = NULL;

	IMAGE_DOS_HEADER *idh = NULL;
	IMAGE_FILE_HEADER *ifh = NULL;
	IMAGE_OPTIONAL_HEADER *ioh = NULL;
	IMAGE_IMPORT_DESCRIPTOR *iid = NULL;

	IMAGE_THUNK_DATA *pThunk = NULL, *pThunk2 = NULL;
	IMAGE_IMPORT_BY_NAME *pname = NULL;

	int ordinal = 0;
	char *name = NULL;

    /* image_base = (DWORD)GetModuleHandle(NULL); */
    idh = (IMAGE_DOS_HEADER *)ImageBase;
	/* idh->e_lfanew is the beginning of the PE header with the signature "PE" */
    ifh = (IMAGE_FILE_HEADER *)(UINT_PTR)((unsigned int)(UINT_PTR)ImageBase + idh->e_lfanew + sizeof(IMAGE_NT_SIGNATURE));
    ioh = (IMAGE_OPTIONAL_HEADER *)(UINT_PTR)((unsigned int)(UINT_PTR)ifh + sizeof(IMAGE_FILE_HEADER));
    iid = (IMAGE_IMPORT_DESCRIPTOR *)(UINT_PTR)((unsigned int)(UINT_PTR)ImageBase + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	/* set the entire IAT (IID + THUNKS) as READWRITE */
    VirtualProtect((void*)(UINT_PTR)((unsigned int)(UINT_PTR)ImageBase + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IAT].VirtualAddress),
        ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IAT].Size, PAGE_READWRITE, &oldProtect);

	/* loop through the IID until it's over */
    while(iid->Name)
    {
		/* if this is the IID of the dll we're after... */
        if(_stricmp(Hook->DLLName, (const char*)(UINT_PTR)((unsigned int)(UINT_PTR)ImageBase + iid->Name)) == 0)
        {
            pThunk = (IMAGE_THUNK_DATA *)(UINT_PTR)(iid->OriginalFirstThunk + (unsigned int)(UINT_PTR)ImageBase);
            pThunk2 = (IMAGE_THUNK_DATA *)(UINT_PTR)(iid->FirstThunk + (unsigned int)(UINT_PTR)ImageBase);

			/* loop through the IAT until it's over */
            while(pThunk->u1.AddressOfData)
            {
                /* Imported by ordinal only: */
                if(pThunk->u1.Ordinal & 0x80000000)
                    ordinal = pThunk->u1.Ordinal & 0xffff;
                else    /* Imported by name, with ordinal hint */
                {
                    pname = (IMAGE_IMPORT_BY_NAME *)(UINT_PTR)(pThunk->u1.AddressOfData + (unsigned int)(UINT_PTR)ImageBase);
                    ordinal = pname->Hint;
                    name = (char*)pname->Name;
                }

				/* hook if found by name */
                if(name != NULL && Hook->FuncName != NULL && strcmp(name, Hook->FuncName) == 0)
                {
					Hook->OrigFn = (void*)(UINT_PTR)pThunk2->u1.Function;
					Hook->OrigPtr = (void*)pThunk2;
                    pThunk2->u1.Function = (DWORD)(UINT_PTR)Hook->HookFn;

                } /* hook if found by ordinal */
                else if(ordinal == Hook->Ordinal)
                {
					Hook->OrigFn = (void*)(UINT_PTR)pThunk2->u1.Function;
					Hook->OrigPtr = (void*)pThunk2;
                    pThunk2->u1.Function = (DWORD)(UINT_PTR)Hook->HookFn;
                }

				pThunk++;
                pThunk2++;
            }
		}
		/* skip to the next IID */
        iid++;
    }
}
bool UnHookAPICalls( SFunctionHook* Hook, HMODULE hModule)
{
	if ( !Hook )
		return false;

	DWORD flOldProtect, flNewProtect, flDontCare;
    MEMORY_BASIC_INFORMATION mbi;

	// Get the current protection attributes                            
	VirtualQuery(Hook->OrigPtr, &mbi, sizeof(mbi));
	    
	// remove ReadOnly and ExecuteRead attributes, add on ReadWrite flag
	flNewProtect = mbi.Protect;
	flNewProtect &= ~(PAGE_READONLY | PAGE_EXECUTE_READ);
	flNewProtect |= (PAGE_READWRITE);
    
	if (!VirtualProtect(Hook->OrigPtr, (sizeof(PVOID)), flNewProtect, &flOldProtect))
	{
		return false;
	}
		
	memcpy(Hook->OrigPtr,&Hook->OrigFn,sizeof(Hook->OrigFn));//write old address back
		
	// Put the page attributes back the way they were.
	VirtualProtect(Hook->OrigPtr, (sizeof(PVOID)), flOldProtect, &flDontCare);

	return true;   
    
} 