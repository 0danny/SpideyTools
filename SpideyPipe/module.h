#pragma once
#include "string"

namespace models
{
    class module
    {
    public:
        std::string moduleName = "Unknown";
        
        virtual ~module() {}
        virtual void runHooks() = 0;
    };
}
