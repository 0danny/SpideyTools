#include "spideyPipe.h"

main::spideyPipe* pipeObj = nullptr;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        pipeObj = new main::spideyPipe();
        pipeObj->init();
        break;
    case DLL_PROCESS_DETACH:
        if (pipeObj)
        {
            pipeObj->cleanup();
            delete pipeObj;
            pipeObj = nullptr;
        }
        break;
    }
    return TRUE;
}
