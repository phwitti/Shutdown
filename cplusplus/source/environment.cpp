
//

#include <Windows.h>

//

#include "environment.hpp"

//

namespace pw
{
    std::wstring Environment::getSystemDirectory()
    {
        WCHAR systemDirectory[MAX_PATH];
        GetSystemDirectoryW(systemDirectory, _countof(systemDirectory));

        return systemDirectory;
    }

    std::wstring Environment::getCurrentLocaleName()
    {
        WCHAR localeName[LOCALE_NAME_MAX_LENGTH];
        GetUserDefaultLocaleName(localeName, LOCALE_NAME_MAX_LENGTH);

        return localeName;
    }
}
