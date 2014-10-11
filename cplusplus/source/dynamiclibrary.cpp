
//

#include <Windows.h>
#include <exception>
#include <memory>

//

#include "dynamiclibrary.hpp"
#include "exception.hpp"

//

namespace pw
{
    DynamicLibrary::DynamicLibrary() :
        m_ptrModule(nullptr)
    {
    }

    DynamicLibrary::DynamicLibrary(const std::wstring &_path) :
        m_ptrModule(nullptr)
    {
        load(_path);
    }

    DynamicLibrary::~DynamicLibrary()
    {
        try
        {
            unload();
        }
        catch (...)
        {
            // Nothing to do
        }
    }

    //

    void DynamicLibrary::load(const std::wstring &_path)
    {
        Contract::ensure<ArgumentNullException>(_path != L"", "Input parameter cannot be an empty string");
        Contract::ensure<InvalidOperationException>(m_ptrModule == nullptr, "There's still a module loaded. Call Unload before loading another module.");

        m_ptrModule = LoadLibraryW(_path.c_str());

        if (m_ptrModule == nullptr)
        {
            throw Win32Exception();
        }
    }

    void DynamicLibrary::unload()
    {
        if (m_ptrModule != nullptr)
        {
            if (!FreeLibrary(static_cast<HMODULE>(m_ptrModule)))
            {
                throw Win32Exception();
            }
            m_ptrModule = nullptr;
        }
    }

    //

    std::wstring DynamicLibrary::loadString(unsigned int _id, int _maxStringLength) const
    {
        Contract::ensure<InvalidOperationException>(m_ptrModule != nullptr, "Module hasn't been loaded yet.");

        std::unique_ptr<wchar_t[]> charBuffer(new wchar_t[_maxStringLength]);
        LoadStringW(static_cast<HINSTANCE>(m_ptrModule), _id, charBuffer.get(), _maxStringLength);

        return std::wstring(charBuffer.get());
    }

    std::wstring DynamicLibrary::loadString(unsigned int _id, const std::wstring &_fallback, int _maxStringLength) const
    {
        try
        {
            std::wstring out = loadString(_id, _maxStringLength);
            if (out != L"")
            {
                return out;
            }

            throw InvalidOperationException("String could not be read from the module.");
        }
        catch (...)
        {
            return _fallback;
        }
    }
}
