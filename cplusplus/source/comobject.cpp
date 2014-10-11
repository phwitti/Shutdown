
//

#include <Windows.h>

//

#include "comobject.hpp"

//

namespace pw
{
    ComObject::ComObject()
    {
        CoInitializeEx(nullptr, COINIT_MULTITHREADED);
    }

    ComObject::~ComObject()
    {
        CoUninitialize();
    }
}
