
//

#include "jumptask.hpp"

//

namespace pw
{
    JumpTask::JumpTask()
    {
    }

    JumpTask::JumpTask(const std::wstring &_title, const std::wstring &_applicationPath, const std::wstring &_arguments, const std::wstring &_description, const std::wstring &_iconResourcePath, int _iconResourceIndex) :
        title(_title),
        applicationPath(_applicationPath),
        arguments(_arguments),
        description(_description),
        iconResourcePath(_iconResourcePath),
        iconResourceIndex(_iconResourceIndex)
    {
    }
}
