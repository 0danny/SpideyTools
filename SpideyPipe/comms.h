#pragma once
#include <thread>
#include <Windows.h>

#include "logger.h"

typedef void (*commsCallback)(std::string message);

namespace pipe
{
	class comms
	{
	private:
		const char* pipeName = "\\\\.\\pipe\\SpideyPipe";

		std::thread commsThread;
		commsCallback cmsCallback;

	public:
		void start(commsCallback cmsCallback);
		void listen();
		void cleanup();
	};
}
