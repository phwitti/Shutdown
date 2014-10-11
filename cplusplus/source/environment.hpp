#ifndef ENVIRONMENT_HPP
#define ENVIRONMENT_HPP

//

#include <string>

//

namespace pw
{
    class Environment
    {

    public:

        static std::wstring getSystemDirectory();
        static std::wstring getCurrentLocaleName();
        
    private:
    
        Environment();
    
    };
}

//

#endif
