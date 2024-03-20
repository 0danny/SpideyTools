#pragma once
#include <thread>
#include <Windows.h>

#include "logger.h"

namespace pipe
{
	class comms
	{
	private:
		const char* pipeName = "\\\\.\\pipe\\SpideyPipe";

		std::thread commsThread;

	public:
		void start();
		void listen();
		void cleanup();
	};
}
