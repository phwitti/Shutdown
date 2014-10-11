#ifndef PATH_HPP
#define PATH_HPP

//

#include <string>
#include <initializer_list>

//

namespace pw
{
    class Path
    {

    public:

        static const std::wstring combine(const std::initializer_list<std::wstring> &_ilParts);

    };
}

//

#endif
