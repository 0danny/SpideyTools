#include "characterSwap.h"

namespace mods
{
	//Variables
	static characterSwap::f_getCostume getCostume_og;

	void characterSwap::runHooks()
	{
		utils::logger::log("Running characterSwap hooks.");

		if (MH_CreateHook(getCostumePatch, &detourGetCostume, reinterpret_cast<LPVOID*>(&getCostume_og)) != MH_OK)
		{
			utils::logger::log("Failed to create hook for getCostumePatch.");
		}
	}

	char* characterSwap::detourGetCostume(int p_costumeStruct)
	{
		char* levelName = reinterpret_cast<char*>(*(int*)(p_costumeStruct + 4));

		utils::logger::log("Get costume called -> ", levelName);

		if (strcmp(levelName, "M0menu\\menu") == 0)
		{
			utils::logger::log("Patching for M0menu.");

			return reinterpret_cast<char*>(0x008D03C4);
		}

		return getCostume_og(p_costumeStruct);
	}
}
