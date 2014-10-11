#ifndef DYNAMICLIBRARY_HPP
#define DYNAMICLIBRARY_HPP

//

#include <string>

//

namespace pw
{
    class DynamicLibrary
    {

    public:

        DynamicLibrary();
        DynamicLibrary(const std::wstring &_path);
        ~DynamicLibrary();

        void load(const std::wstring &_path);
        void unload();

        std::wstring loadString(unsigned int _id, int _maxStringLength = 256) const;
        std::wstring loadString(unsigned int _id, const std::wstring &_fallback, int _maxStringLength = 256) const;

    private:

        void *m_ptrModule;

    };
}

//

#endif
