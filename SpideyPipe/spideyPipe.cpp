#include "spideyPipe.h"

namespace main
{
	void spideyPipe::init()
	{
		if (debugMode)
		{
			setConsole();
		}

		utils::logger::log("Spidey Pipe injected.");

		//init MinHook.
		if (MH_Initialize() != MH_OK) {
			utils::logger::log("Failed to initialize MinHook.");
		}

		loadMods();
		initMods();

		//Enable all minhooks
		if (MH_EnableHook(MH_ALL_HOOKS) != MH_OK) {
			utils::logger::log("Failed to enable all hooks.");
			return;
		}

		comms.start();
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
