#ifndef JUMPTASK_HPP
#define JUMPTASK_HPP

//

#include <string>

//

namespace pw
{
    struct JumpTask
    {

        JumpTask();
        JumpTask(const std::wstring &_title, const std::wstring &_applicationPath, const std::wstring &_arguments, const std::wstring &_description, const std::wstring &_iconResourcePath, int _iconResourceIndex);
        
        //
        
        std::wstring title;
        std::wstring applicationPath;
        std::wstring arguments;
        std::wstring description;
        std::wstring iconResourcePath;
        int iconResourceIndex;

    };
}

//

#endif
