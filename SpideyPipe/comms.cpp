#include "comms.h"

namespace pipe
{
	void comms::start(commsCallback cmsCallback)
	{
		utils::logger::log("Starting the comms.");

		this->cmsCallback = cmsCallback;

		//Start the thread;
		commsThread = std::thread([this] { this->listen(); });
	}

	void comms::cleanup()
	{
		if (commsThread.joinable())
			commsThread.join();
	}

	void comms::listen()
	{
		while (true)
		{
			HANDLE pipe = CreateNamedPipeA(
				pipeName,
				PIPE_ACCESS_INBOUND,
				PIPE_TYPE_BYTE | PIPE_WAIT,
				1,
				512,
				512,
				0,
				NULL);

			if (pipe == INVALID_HANDLE_VALUE)
			{
				utils::logger::log("Failed to create pipe. Error: ", GetLastError());
			}
			else
			{
				utils::logger::log("Waiting for client connection...");

				BOOL connected = ConnectNamedPipe(pipe, NULL) ? TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);

				if (connected)
				{
					utils::logger::log("Client connected.");

					char buffer[512];
					DWORD bytesRead;

					BOOL result = ReadFile(pipe, buffer, sizeof(buffer), &bytesRead, NULL);

					if (!result && GetLastError() != ERROR_MORE_DATA)
					{
						utils::logger::log("Failed to read from pipe. Error: ", GetLastError());
					}
					else
					{
						utils::logger::log("Message received from client: ", buffer);

						//Do something with message.
						cmsCallback(std::string(buffer));
					}
				}
				else
				{
					utils::logger::log("No client connected.");
				}

				CloseHandle(pipe);
			}
		}
	}
}