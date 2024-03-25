#pragma once
#include "module.h"
#include "logger.h"
#include "MinHook.h"

namespace mods
{
	class characterSwap : public models::module
	{
	private:	
		//Functions
		void* getCostumePatch = reinterpret_cast<void*>(0x005BE230);


	public:
		characterSwap()
		{
			moduleName = "characterSwap";
		}

		void runHooks() override;

		static char* detourGetCostume(int p_costumeStruct);


		//Typedefs
		typedef char* (*__cdecl f_getCostume)(int p_costumeStruct);
	};
}


