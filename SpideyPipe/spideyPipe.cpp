#include "spideyPipe.h"

namespace main
{
	void spideyPipe::init()
	{
		if (debugMode)
		{
			setConsole();
		}

		utils::logger::log("Spidey Pipe injected -> ", GetLastError());

		//init MinHook.
		if (MH_Initialize() != MH_OK) {
			utils::logger::log("Failed to initialize MinHook.");
		}

		loadMods();
		initMods();

		//Enable all minhooks
		if (MH_EnableHook(MH_ALL_HOOKS) != MH_OK) {
			utils::logger::log("Failed to enable all hooks.");
		}

		comms.start(commsCallback);
	}

	void spideyPipe::commsCallback(std::string message)
	{
		//char __stdcall sub_4B1600(int a1, int a2)

		/*
		typedef char(__stdcall* Func)(test* a1);

		Func func = (Func)(0x0052A610);

		func(&testStruct);*/

		//utils::logger::log("Result: ", result);
		
		/*
		auto wideScreen = reinterpret_cast<int*>(0x00924DF0);

		*wideScreen = 1;

		utils::logger::log("Result: ", *wideScreen);*/
	}

	void spideyPipe::initMods()
	{
		for (auto& mod : modules) {
			mod->runHooks();
		}
	}

	void spideyPipe::loadMods()
	{
		modules.push_back(std::make_shared<mods::characterSwap>());
	}

	void spideyPipe::cleanup()
	{
		comms.cleanup();

		FreeConsole();
	}

	void spideyPipe::setConsole()
	{
		AllocConsole();

		{
			FILE* pCout;
			freopen_s(&pCout, "CONOUT$", "w", stdout);
			std::ios::sync_with_stdio();
		}

		SetConsoleTitle(L"Spidey Pipe Debug");
	}
}
