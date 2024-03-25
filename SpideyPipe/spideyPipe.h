#pragma once

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <iostream>
#include <cstdio>
#include <ios>
#include <vector>

#pragma comment(lib, "libMinHook-x86.lib")
#include "MinHook.h"

#include "logger.h"
#include "comms.h"
#include "module.h"
#include "characterSwap.h"

struct test {
	int unkInt1 = 1;
	int unkInt2 = 1;
	int unkInt3 = 1;
};

namespace main
{
	class spideyPipe
	{
	private:
		std::vector<std::shared_ptr<models::module>> modules;

		pipe::comms comms;
		bool debugMode = true;

	public:		
		void init();
		void cleanup();
		void setConsole();

		void loadMods();
		void initMods();

		static void commsCallback(std::string message);
	};
}

