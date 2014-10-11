
//

#include <sstream>

//

#include "path.hpp"

//

namespace pw
{
    const std::wstring Path::combine(const std::initializer_list<std::wstring> &_ilParts)
    {
        std::wstringstream wstringstream;
        unsigned int i = 1;
        for (const std::wstring &wstring : _ilParts)
        {
            wstringstream << wstring;

            if (i != _ilParts.size())
                wstringstream << L"\\";

            i++;
        }
        return wstringstream.str();
    }
}
