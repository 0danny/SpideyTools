#pragma once

#include <string>
#include <iostream>

namespace utils
{
	class logger
	{
	public:
		static void log(const std::string& message) {
			std::cout << "[SpideyPipe]: " << message << std::endl;
		}

		template<typename... Args>
		static void log(const std::string& first, Args... args) {
			std::cout << "[SpideyPipe]: " << first;
			((std::cout << args), ...);
			std::cout << std::endl;
		}
	};
}